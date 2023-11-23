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
    [SerializeField] GameObject gameOverUI;

    [SerializeField] CameraController cameraController;

    [SerializeField] List<GameObject> playerPrefabs;
    [SerializeField] List<GameObject> levels;

    public static bool gameOver;

    GameObject currentLevel;
    PlayerController player;

    int score;
    float _score;
    int shards;
    int highScore;

    void Start(){
        score = 0;
        shards = PlayerInfo.GetShards();
        highScore = PlayerInfo.GetHighScore();

        GameObject _p = Instantiate(playerPrefabs[PlayerInfo.GetShipIndex()], transform);
        player = _p.GetComponent<PlayerController>();

        cameraController.SetPlayerController(player);
        
        StartCoroutine(Fade(1f, 0f));
        GenerateNewLevel();

        AudioManager.instance.PlayMusic("ingame");
        AudioManager.instance.PlaySFX("start");
    }

    void Update(){
        if(!player.isAlive) return;

        if (LevelGenerator.levelCompleted) StartCoroutine(HandleTransition());

        HandleScoreUpdate();
        SetStatusUI();
    }

    public void HandleScoreUpdate(){
        _score += Time.deltaTime;
        score = (int) _score;
        highScore = score > highScore?score:highScore;
    }

    public void HandleCapture(string tag){
        if (tag == "Shard")
        {
            score += 10;
            shards++;
            ParticleManager.instance.CaptureShard(player.transform.position);
            AudioManager.instance.PlaySFX("shardcap");
        }
        else if (tag == "Jump")
        {
            player.jumps++;
            ParticleManager.instance.CaptureItem(player.transform.position);
            AudioManager.instance.PlaySFX("capture");
        }
        else if (tag == "Shield")
        {
            player.shields++;
            ParticleManager.instance.CaptureItem(player.transform.position);
            AudioManager.instance.PlaySFX("capture");
        }

    }

    public void EndGame(){

        PlayerInfo.SetHighScore(highScore);
        PlayerInfo.SetShards(shards);

        overlay.gameObject.SetActive(false);
        gameOverUI.SetActive(true);
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
        if(LevelGenerator.levelCompleted){
            GenerateNewLevel();
            player.HandleTransition();
        }
        yield return new WaitForSeconds(0.3f);
        yield return Fade(1f, 0f);
    }

    public int GetScore(){
        return score;
    }

    public int GetShards(){
        return shards;
    }

    public int GetHighScore(){
        return highScore;
    }

    void GenerateNewLevel(){
        if (currentLevel){
            Destroy(currentLevel);
            currentLevel = Instantiate(levels[Random.Range(0, levels.Count)], this.transform);
        }
        else{
            currentLevel = Instantiate(levels[0], this.transform);
        }
        currentLevel.transform.position = Vector3.zero;
        currentLevel.GetComponent<LevelGenerator>().setPlayerTransform(player.transform);
    }

}
