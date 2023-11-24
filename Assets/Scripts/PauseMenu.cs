using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class PauseMenu : MonoBehaviour{
    
    [SerializeField] GameObject pauseMenuUI;
    [SerializeField] TMP_Text scoreText;
    [SerializeField] TMP_Text shardsText;
    [SerializeField] TMP_Text highScoreText;

    public void Pause(int score, int highScore, int shards){
        pauseMenuUI.SetActive(true);
        scoreText.text = score.ToString();
        shardsText.text = highScore.ToString();
        highScoreText.text = shards.ToString();

        AudioManager.instance.PauseSFX();
        
        Time.timeScale = 0f;
    }

    public void Resume(){
        pauseMenuUI.SetActive(false);
        Time.timeScale = 1f;
        AudioManager.instance.ResumeSFX();
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
