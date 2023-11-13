using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class GameOverMenu : MonoBehaviour{
    
    [SerializeField] TMP_Text scoreText;
    [SerializeField] TMP_Text highScoreText;
    [SerializeField] TMP_Text shardsText;
    [SerializeField] GameObject gameOverMenuUI;

    void Start(){
        gameOverMenuUI.SetActive(false);
    }

    public void Activate(){
        int score = GetComponent<GameManager>().GetScore();

        scoreText.text = $"Score\n{score}";
        highScoreText.text = $"HighScore\n{PlayerInfo.GetHighScore()}";
        shardsText.text = $"Shards\n{PlayerInfo.GetShards()}";

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
