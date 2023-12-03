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

    [HideInInspector] public int score;
    [HideInInspector] public int shards;
    float _score;
    int highScore;
    public static int REVIVAL_COST = 20;

    float del = -0.06f;
    float exposure = 6f;

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
        if(exposure <= 0.1f) del = 0.06f;
        else if(exposure >= 4f) del = -0.06f;
        exposure += del * Time.deltaTime;
        RenderSettings.skybox.SetFloat("_Exposure", exposure);
    }

    public void HandleScoreUpdate(){
        _score += Time.deltaTime * player.rb.velocity.z / 10f;
        score = (int) _score;
        highScore = score > highScore?score:highScore;
    }

    public void HandlePlayerCrash(){
        if(!player.revived && shards >= REVIVAL_COST){
            goMenuController.StartRevivalRoutine();
        }else{
            EndGame();
        }
    }

    public void BuyLife(){
        player.revived = true;
        goMenuController.StopRevival();
        shards -= REVIVAL_COST;
        PlayerInfo.SetShards(shards);
        player.CrashSurvival();
    }

    public void EndGame(){
        player.isAlive = false;
        player.gameObject.SetActive(false);
        AudioManager.instance.PlaySFX("death");
        StartCoroutine(EndGameTransition());
        PlayerInfo.SetHighScore(highScore);
        PlayerInfo.SetShards(shards);
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

    IEnumerator EndGameTransition(){
        yield return Fade(0f, 1f);
        goMenuController.ActivateGameOverUI();
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
