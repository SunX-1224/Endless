using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.Audio;
using UnityEngine.UI;
using TMPro;

public class MainMenu : MonoBehaviour{
    
    [SerializeField] TMP_Text volumeText;
    [SerializeField] TMP_Dropdown qualityDropdown;
    [SerializeField] Slider slider;
    [SerializeField] GameObject savingPromptUI;

    void Start(){
        slider.value = PlayerPrefs.GetFloat("volume", 1.0f);
        qualityDropdown.value = PlayerPrefs.GetInt("quality", 1);

        QualitySettings.SetQualityLevel(qualityDropdown.value);
        AudioListener.volume = slider.value;

        AudioManager.instance.StopSFX();
        AudioManager.instance.PlayMusic("bg");
    }

    public void LoadGame(){
        SceneManager.LoadScene(1);
    }

    public void QuitGame(){
        Application.Quit();
    }

    public void SetVolume(float volume){
        volumeText.text = volume.ToString("0.0");
        AudioListener.volume = volume;
    }

    public void SaveVolume(){
        PlayerPrefs.SetFloat("volume", AudioListener.volume);
        StartCoroutine(SavingPrompt());
    }

    IEnumerator SavingPrompt(){
        savingPromptUI.SetActive(true);
        yield return new WaitForSeconds(1f);
        savingPromptUI.SetActive(false);
    }

    public void SetQuality(int index){
        QualitySettings.SetQualityLevel(index);
    }

    public void SaveQuality(){
        PlayerPrefs.SetInt("quality", QualitySettings.GetQualityLevel());
        StartCoroutine(SavingPrompt());
    }
}
