using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class GameOverMenu : MonoBehaviour{
    
    [SerializeField] GameManager gameManager;
    [SerializeField] TMP_Text scoreText;
    [SerializeField] TMP_Text highScoreText;
    [SerializeField] TMP_Text shardsText;

    void OnEnable(){
        scoreText.text = gameManager.GetScore().ToString();
        highScoreText.text = gameManager.GetHighScore().ToString();
        shardsText.text = gameManager.GetShards().ToString(); 
    }
    public void LoadMenu(){
        SceneManager.LoadScene(0);
    }

    public void Restart(){
        SceneManager.LoadScene(1);
    }
}
