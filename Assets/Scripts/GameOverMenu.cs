using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class GameOverMenu : MonoBehaviour{
    
    [SerializeField] GameObject gameOverMenuUI;

    public void Activate(){
        gameOverMenuUI.SetActive(true);
    }
    public void LoadMenu(){
        SceneManager.LoadScene(0);
    }

    public void Restart(){
        SceneManager.LoadScene(1);
    }
}
