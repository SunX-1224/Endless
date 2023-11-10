using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] List<GameObject> levels;
    [SerializeField] TransitionHandler transitionHandler;
    public Transform player;

    GameObject currentLevel;

    void Start(){
        GenerateNewLevel();
        player.position = Vector3.zero;
        player.rotation = Quaternion.identity;
    }

    void Update(){
        if(LevelGenerator.levelCompleted){
            transitionHandler.StartFadeIn();
            StartCoroutine(FinishTransition());
        }
    }

    IEnumerator FinishTransition(){
        player.position = Vector3.zero;
        player.rotation = Quaternion.identity;
        yield return new WaitForSeconds(2);
        GenerateNewLevel();
        transitionHandler.StartFadeOut();
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
