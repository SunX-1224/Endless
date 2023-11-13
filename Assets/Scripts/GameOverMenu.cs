using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class GameOverMenu : MonoBehaviour{
    
    [SerializeField] TMP_Text scoreText;
    [SerializeField] GameObject gameOverMenuUI;

    void Start(){
        gameOverMenuUI.SetActive(false);
    }

    public void Activate(){
        int score = GetComponent<GameManager>().GetScore();
        scoreText.text = $"Score\n{score}";
        gameOverMenuUI.SetActive(true);
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
