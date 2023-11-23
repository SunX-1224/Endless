using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class PauseMenu : MonoBehaviour{
    
    [SerializeField] GameManager gameManager;
    [SerializeField] TMP_Text scoreText;
    [SerializeField] TMP_Text shardsText;
    [SerializeField] TMP_Text highScoreText;

    void OnEnable(){
        scoreText.text = gameManager.GetScore().ToString();
        shardsText.text = gameManager.GetShards().ToString();
        highScoreText.text = gameManager.GetHighScore().ToString();

        AudioManager.instance.PauseSFX();
        
        Time.timeScale = 0f;
    }

    void OnDisable(){
        Time.timeScale = 1f;
        AudioManager.instance.ResumeSFX();
    }
    
    public void LoadMenu(){
        SceneManager.LoadScene(0);
    }

    public void Restart(){
        SceneManager.LoadScene(1);
    }
}
