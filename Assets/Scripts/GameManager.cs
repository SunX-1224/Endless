using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    [SerializeField] List<GameObject> levels;
    [SerializeField] RawImage overlay;
    public Transform player;

    GameObject currentLevel;

    void Start(){
        overlay.enabled = true;
        StartCoroutine(HandleTransition(true));
    }

    void Update(){
        if(LevelGenerator.levelCompleted){
            StartCoroutine(HandleTransition());
        }
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
        player.position = Vector3.zero;
        player.rotation = Quaternion.identity;
        yield return new WaitForSeconds(1f);

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
        currentLevel.GetComponent<LevelGenerator>().setPlayerTransform(player);
    }
}
