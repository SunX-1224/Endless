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
    [SerializeField] GameObject assetsMenu;

    [SerializeField] GameObject buyPrompt;
    [SerializeField] GameObject insuffPrompt;
    [SerializeField] TMP_Text costText;
    [SerializeField] TMP_Text shardsText;
    [SerializeField] TMP_Text highScoreText;
    [SerializeField] GameObject[] ships;

    int buyProcessShip = 0;

    void Awake(){
        UpdateScoreText();
        UpdateShardsText();
        UpdateShipStatus();
    }

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

    public void ResetAll(){
        PlayerPrefs.DeleteAll();
        UpdateScoreText();
        UpdateShardsText();
        UpdateShipStatus();
    }
    
    void UpdateShipStatus(){

        int shipsMask = PlayerInfo.GetShipsUnlockMask();
        int shipIndex = PlayerInfo.GetShipIndex();

        for(int i=0; i < ships.Length; i++){
            Image icon = ships[i].GetComponentInChildren<Image>();

            if((shipsMask & (1<<i)) > 0){
                if(i == shipIndex) icon.color = new Color(0.4f,1f,.8f,1f);
                else icon.color = new Color(1f,1f,1f,0.7f);
            }else{
                icon.color = new Color(0f,0f,0f,1f);
            }
        }
    }

    void UpdateScoreText(){
        int score = PlayerInfo.GetHighScore();
        highScoreText.text = score.ToString();
    }

    void UpdateShardsText(){
        int shards = PlayerInfo.GetShards();
        shardsText.text = shards.ToString();
    }

    public void SelectShip(int index){
        int shipUnlockMask = PlayerInfo.GetShipsUnlockMask();
        int mask = 1 << index;
        if((shipUnlockMask & mask) > 0){
            PlayerInfo.SetShipIndex(index);
            UpdateShipStatus();
        }else{
            PromptBuy(index);
        }
    }

    void PromptBuy(int index){
        buyPrompt.SetActive(true);
        assetsMenu.SetActive(false);
        costText.text = $"Buy this Ship for {GetCost(index)} shards?";
        buyProcessShip = index;
    }

    public void HandleBuy(){
        int shards = PlayerInfo.GetShards();
        if(shards >= GetCost(buyProcessShip)){
            PlayerInfo.SetShards(shards - GetCost(buyProcessShip));
            PlayerInfo.SetShipsUnlockMask(buyProcessShip);
            PlayerInfo.SetShipIndex(buyProcessShip);
            assetsMenu.SetActive(true);
            UpdateShardsText();
            UpdateShipStatus();
        }else{
            insuffPrompt.SetActive(true);
        }
    }

    int GetCost(int i){
        return 100*i;
    }
}
