using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameManager : MonoBehaviour
{
    [SerializeField] List<GameObject> levels;
    [SerializeField] RawImage overlay;
    [SerializeField] TMP_Text ingameScore;
    
    GameObject currentLevel;
    GameObject player;
    int score = 0;

    public static bool gameOver;
    public static bool gamePaused;

    void Start(){
        gameOver = false;
        gamePaused = false;

        player = GameObject.Find("Player");
        setScoreUI();

        overlay.gameObject.SetActive(true);
        StartCoroutine(HandleTransition(true));
    }

    void Update(){
        if(player.GetComponent<PlayerController>().player.isAlive && LevelGenerator.levelCompleted){
            StartCoroutine(HandleTransition());
        }

        if(Input.GetKeyDown(KeyCode.Escape))
            HandlePause();
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
            score += 5;
            setScoreUI();
        }
    }

    public void EndGame(){
        gameOver = true;
        overlay.gameObject.SetActive(false);
        GetComponent<GameOverMenu>().Activate();
    }

    void setScoreUI(){
        ingameScore.text = $"{score}";
    }

    public int GetScore(){
        return score;
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
        GenerateNewLevel();
        player.transform.position = Vector3.zero;
        player.transform.rotation = Quaternion.identity;
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
