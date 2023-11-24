using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameManager : MonoBehaviour {

    [SerializeField] RawImage overlay;
    [SerializeField] TMP_Text ingameScore;
    [SerializeField] TMP_Text shardsText;
    [SerializeField] TMP_Text highScoreText;

    [SerializeField] PlayerController player;

    [SerializeField] List<GameObject> levels;

    PauseMenu pauseMenuController;
    GameOverMenu goMenuController;
    LevelGenerator currentLevel;

    int score;
    float _score;
    int shards;
    int highScore;

    void Start(){
        pauseMenuController = GetComponent<PauseMenu>();
        goMenuController = GetComponent<GameOverMenu>();

        score = 0;
        shards = PlayerInfo.GetShards();
        highScore = PlayerInfo.GetHighScore();

        StartCoroutine(Fade(1f, 0f));
        GenerateNewLevel();

        AudioManager.instance.PlayMusic("ingame");
        AudioManager.instance.PlaySFX("start");
    }

    void Update(){
        if(!player.isAlive) return;

        if (currentLevel.levelCompleted) StartCoroutine(HandleTransition());

        HandleScoreUpdate();
        SetStatusUI();
    }

    public void HandleScoreUpdate(){
        _score += Time.deltaTime * player.velocity.z / 10f;
        score = (int) _score;
        highScore = score > highScore?score:highScore;
    }

    public void HandlePause(){
        pauseMenuController.Pause();
    }

    public void AddScore(int x){
        score += x;
    }

    public void AddShards(int x){
        shards += x;
    }
    
    public void EndGame(){

        PlayerInfo.SetHighScore(highScore);
        PlayerInfo.SetShards(shards);

        overlay.gameObject.SetActive(false);
        goMenuController.Activate();
    }

    void SetStatusUI(){
        ingameScore.text = score.ToString();
        shardsText.text = shards.ToString();
        highScoreText.text = highScore.ToString();
    }

    IEnumerator Fade(float start, float target){
        Color c = overlay.color;
        float t = 0f;

        while(t < 1f){
            c.a = Mathf.Lerp(start, target, t);
            overlay.color = c;
            t += Time.deltaTime;
            yield return null;
        }
        c.a = target;
        overlay.color = c;
    }

    IEnumerator HandleTransition(){
        yield return Fade(0f, 1f);
        if(currentLevel.levelCompleted){
            GenerateNewLevel();
            player.HandleTransition();
        }
        yield return new WaitForSeconds(0.3f);
        yield return Fade(1f, 0f);
    }

    void GenerateNewLevel(){
        if (currentLevel){
            Destroy(currentLevel.gameObject);
            currentLevel = Instantiate(levels[Random.Range(0, levels.Count)], this.transform).GetComponent<LevelGenerator>();
        }
        else{
            currentLevel = Instantiate(levels[0], this.transform).GetComponent<LevelGenerator>();
        }
        currentLevel.transform.position = Vector3.zero;
        currentLevel.setPlayerTransform(player.transform);
    }

}
