using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [SerializeField] List<GameObject> levels;
    [SerializeField] RawImage overlay;
    [SerializeField] GameObject gameOverUI;
    
    GameObject currentLevel;
    GameObject player;

    void Start(){
        overlay.enabled = true;
        player = GameObject.Find("Player");
        StartCoroutine(HandleTransition(true));
    }

    void Update(){
        if(player.GetComponent<PlayerController>().player.isAlive && LevelGenerator.levelCompleted){
            StartCoroutine(HandleTransition());
        }
    }

    public void EndGame(){
        gameOverUI.SetActive(true);
    }
    
    public void Restart(){
        SceneManager.LoadScene(1);
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
