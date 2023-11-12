using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Audio;

public class MainMenu : MonoBehaviour{
    
    public AudioMixer audioMixer;

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
