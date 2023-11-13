using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class PauseMenu : MonoBehaviour{

    [SerializeField] GameObject pauseMenuUI;
    [SerializeField] TMP_Text scoreText;


    void Start(){
        pauseMenuUI.SetActive(false);
    }

    public void Resume(){
        pauseMenuUI.SetActive(false);
        Time.timeScale = 1f;
        GameManager.gamePaused = false;
    }

    public void Pause(){
        int score = GetComponent<GameManager>().GetScore();
        scoreText.text = $"Score\n{score}";
        pauseMenuUI.SetActive(true);
        Time.timeScale = 0f;
        GameManager.gamePaused = true;
    }

    public void LoadMenu(){
        Time.timeScale = 1f;
        SceneManager.LoadScene(0);
    }

    public void Restart(){
        Time.timeScale = 1f;
        SceneManager.LoadScene(1);
    }
}
