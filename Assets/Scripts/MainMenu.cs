using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Audio;
using TMPro;

public class MainMenu : MonoBehaviour{
    
    public AudioMixer audioMixer;
    [SerializeField] TMP_Text highScoreText;
    [SerializeField] TMP_Text shardsText;

    void Start(){
        highScoreText.text = $"HighScore\n{PlayerInfo.GetHighScore()}";
        shardsText.text = $"Shards\n{PlayerInfo.GetShards()}";

        AudioManager.instance.StopSFX();
        AudioManager.instance.PlayMusic("bg");
    }

    public void LoadMenu(){
        SceneManager.LoadScene(0);
    }
    
    public void LoadGame(){
        SceneManager.LoadScene(1);
    }

    public void QuitGame(){
        Application.Quit();
    }

    public void SetVolume(float volume){
        audioMixer.SetFloat("volume", volume);
    }

    public void SetQuality(int index){
        QualitySettings.SetQualityLevel(index);
    }
}
