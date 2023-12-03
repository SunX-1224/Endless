using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;
using UnityEngine.SceneManagement;

public class GameOverMenu : MonoBehaviour{
    
    [SerializeField] GameObject inGameOverlay;
    [SerializeField] GameObject gameOverMenuUI;
    [SerializeField] GameObject reviveUI;
    [SerializeField] Image fillUI;

    Coroutine revivalRoutine;

    public void StartRevivalRoutine(){
        Time.timeScale = 0f;
        inGameOverlay.SetActive(false);
        reviveUI.SetActive(true);
        AudioManager.instance.PlaySFX("freeze");
        revivalRoutine = StartCoroutine(RevivalRoutine());
    }

    public void StopRevival(){
        Time.timeScale = 1f;
        StopCoroutine(revivalRoutine);
        inGameOverlay.SetActive(true);
        reviveUI.SetActive(false);
    }

    IEnumerator RevivalRoutine(){
        fillUI.fillAmount = 0f;
        while(fillUI.fillAmount < 1f){
            fillUI.fillAmount += 0.4f * Time.unscaledDeltaTime;
            yield return null;
        }
        reviveUI.SetActive(false);
        inGameOverlay.SetActive(true);
        Time.timeScale = 1f;
        GetComponent<GameManager>().EndGame();
    }

    public void ActivateGameOverUI(){
        inGameOverlay.SetActive(false);
        gameOverMenuUI.SetActive(true);
    }
    public void LoadMenu(){
        SceneManager.LoadScene(0);
    }

    public void Restart(){
        SceneManager.LoadScene(1);
    }
}
