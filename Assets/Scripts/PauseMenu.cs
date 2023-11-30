using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class PauseMenu : MonoBehaviour{
    
    [SerializeField] GameObject pauseMenuUI;

    public void Pause(){
        pauseMenuUI.SetActive(true);
        AudioManager.instance.PauseSFX();
        Time.timeScale = 0f;
    }

    public void Resume(){
        AudioManager.instance.ResumeSFX();
        Time.timeScale = 1f;
        pauseMenuUI.SetActive(false);
    }
    
    public void LoadMenu(){
        Time.timeScale = 1f;
        SceneManager.LoadScene(0);
        AudioManager.instance.StopSFX();
    }

    public void Restart(){
        Time.timeScale = 1f;
        AudioManager.instance.StopSFX();
        SceneManager.LoadScene(1);
    }
}
