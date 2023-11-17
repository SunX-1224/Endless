using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameManager : MonoBehaviour
{
    [SerializeField] Mesh mesh;
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

        overlay.gameObject.SetActive(true);
        StartCoroutine(HandleTransition(true));
    }

    void Update(){
        if(player.isAlive && LevelGenerator.levelCompleted){
            StartCoroutine(HandleTransition());
        }

        if(Input.GetKeyDown(KeyCode.Escape))
            HandlePause();
        
        HandleScoreUpdate();
        SetStatusUI();
    }

    public void HandleScoreUpdate(){
        levelScore = (int) player.transform.position.z;
    }

    public void HandlePause(){
        if(gameOver) return;

        if(gamePaused){
            GetComponent<PauseMenu>().Resume();
            overlay.gameObject.SetActive(true);
        } else{
            GetComponent<PauseMenu>().Pause();
            overlay.gameObject.SetActive(false);
        }
    }

    public void HandleCapture(string tag){
        if(tag == "Shard"){
            totalScore += 20;
            shards++;
        }else if(tag == "Jump"){
            player.jumps++;
        }else if(tag == "Shield"){
            player.shields++;
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

    public int GetScore(){
        return totalScore + levelScore;
    }
    
    IEnumerator HandleTransition(bool initState = false){
        float a = 0f;
        while(a < 1f && !initState){
            a = Mathf.Clamp01(a + Time.deltaTime);
            Color _c = overlay.color;
            _c.a = a;
            overlay.color = _c; 
            yield return null;
        }
        player.transform.position = Vector3.zero;
        player.transform.rotation = Quaternion.identity;
        GenerateNewLevel();
        yield return new WaitForSeconds(0.3f);

        a = 1.0f;
        while(a > 0f){
            a = Mathf.Clamp01(a - Time.deltaTime);
            Color _c = overlay.color;
            _c.a = a;
            overlay.color = _c;
            yield return null;
        }
    }

    void GenerateNewLevel(){
        totalScore += levelScore;
        levelScore = 0;

       if(currentLevel){
            Destroy(currentLevel);
            currentLevel = Instantiate(levels[Random.Range(0, levels.Count)], this.transform);
        }else{
            currentLevel = Instantiate(levels[0], this.transform);
        }
        currentLevel.transform.position = Vector3.zero;
        currentLevel.GetComponent<LevelGenerator>().setPlayerTransform(player.transform);
    }
}
