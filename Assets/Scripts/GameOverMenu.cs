using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class GameOverMenu : MonoBehaviour{
    
    [SerializeField] GameObject gameOverMenuUI;
    [SerializeField] TMP_Text scoreText;
    [SerializeField] TMP_Text highScoreText;
    [SerializeField] TMP_Text shardsText;

    public void Activate(int score, int highScore, int shards){
        gameOverMenuUI.SetActive(true);
        scoreText.text = score.ToString();
        highScoreText.text = highScore.ToString();
        shardsText.text = shards.ToString(); 
    }
    public void LoadMenu(){
        SceneManager.LoadScene(0);
    }

    public void Restart(){
        SceneManager.LoadScene(1);
    }
}
