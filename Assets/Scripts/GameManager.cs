using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameManager : MonoBehaviour {
    [SerializeField] RawImage overlay;
    [SerializeField] TMP_Text ingameScore;
    [SerializeField] TMP_Text shardsText;

    [SerializeField] CameraController cameraController;

    [SerializeField] List<GameObject> playerPrefabs;
    [SerializeField] List<GameObject> levels;

    public static bool gameOver;
    public static bool gamePaused;

    GameObject currentLevel;
    PlayerController player;

    int totalScore;
    int levelScore;
    int shards;
    int highScore;

    void Start(){
        gameOver = false;
        gamePaused = false;

        levelScore = 0;
        totalScore = 0;

        shards = PlayerInfo.GetShards();
        highScore = PlayerInfo.GetHighScore();

        GameObject _p = Instantiate(playerPrefabs[PlayerInfo.GetShipIndex()], transform);
        player = _p.GetComponent<PlayerController>();

        cameraController.SetPlayerController(player);
        
        GenerateNewLevel();

        AudioManager.instance.PlayMusic("ingame");
        AudioManager.instance.PlaySFX("start");
    }

    void Update(){
        if(!player.isAlive) return;

        if (LevelGenerator.levelCompleted) StartCoroutine(HandleTransition());

        if (Input.GetKeyDown(KeyCode.Escape)) HandlePause();

        HandleScoreUpdate();
        SetStatusUI();
    }

    public void HandleScoreUpdate(){
        levelScore = (int)(player.transform.position.z/10f);
    }

    public void HandlePause(){

        if (gamePaused){
            GetComponent<PauseMenu>().Resume();
            overlay.gameObject.SetActive(true);
            AudioManager.instance.ResumeSFX();
        }else{
            GetComponent<PauseMenu>().Pause();
            overlay.gameObject.SetActive(false);
            AudioManager.instance.PauseSFX();
        }
    }

    public void HandleCapture(string tag){
        if (tag == "Shard")
        {
            totalScore += 20;
            shards++;
            AudioManager.instance.PlaySFX("shardcap");
        }
        else if (tag == "Jump")
        {
            player.jumps++;
            AudioManager.instance.PlaySFX("capture");
        }
        else if (tag == "Shield")
        {
            player.shields++;
            AudioManager.instance.PlaySFX("capture");
        }

    }

    public void EndGame(){

        PlayerInfo.SetHighScore(totalScore + levelScore);
        PlayerInfo.SetShards(shards);

        gameOver = true;
        overlay.gameObject.SetActive(false);
        GetComponent<GameOverMenu>().Activate();
    }

    void SetStatusUI(){
        ingameScore.text = $"Score\n{totalScore + levelScore}";
        shardsText.text = $"Shards\n{shards}";
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
        yield return new WaitForSeconds(0.3f);
        GenerateNewLevel();
        yield return Fade(1f, 0f);
    }

    public int GetScore(){
        return totalScore + levelScore;
    }

    void GenerateNewLevel(){
        totalScore += levelScore;
        levelScore = 0;

        if (currentLevel){
            GameObject t = Instantiate(levels[Random.Range(0, levels.Count)], this.transform);
            player.HandleTransition();
            Destroy(currentLevel);
            currentLevel = t;
        }
        else{
            currentLevel = Instantiate(levels[0], this.transform);
        }
        currentLevel.transform.position = Vector3.zero;
        currentLevel.GetComponent<LevelGenerator>().setPlayerTransform(player.transform);
    }
}
