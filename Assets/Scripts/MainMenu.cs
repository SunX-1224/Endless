using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class MainMenu : MonoBehaviour{

  [SerializeField] TMP_Text volumeText;
  [SerializeField] TMP_Text viewDistanceText;
  [SerializeField] TMP_Dropdown qualityDropdown;
  [SerializeField] Slider volumeSlider;
  [SerializeField] Slider viewDistSlider;
  [SerializeField] Toggle fppToggle;
  [SerializeField] GameObject savingPromptUI;
  [SerializeField] GameObject assetsMenu;
  [SerializeField] Image HTPimage;

  [SerializeField] GameObject buyPrompt;
  [SerializeField] GameObject insuffPrompt;
  [SerializeField] TMP_Text costText;
  [SerializeField] TMP_Text shardsText;
  [SerializeField] TMP_Text highScoreText;
  [SerializeField] GameObject[] ships;
  [SerializeField] Sprite[] HTPsprites;

  [SerializeField] GameObject controlPrompt;
  [SerializeField] Button tiltButton;
  [SerializeField] Button tapButton;

  int buyProcessShip = 0;
  int htpIndex= 0;

  void Awake(){
    UpdateScoreText();
    UpdateShardsText();
    UpdateShipStatus();
  }

  void Start(){
    HandleControlCheck();
    UpdateControlTypeUI();

    viewDistSlider.value = PlayerPrefs.GetFloat("viewDistance", 80f);
    volumeSlider.value = PlayerPrefs.GetFloat("volume", 1.0f);
    qualityDropdown.value = PlayerPrefs.GetInt("quality", 1);
    fppToggle.isOn = PlayerPrefs.GetInt("fpp", 0) > 0;

    HTPimage.sprite = HTPsprites[htpIndex];

    QualitySettings.SetQualityLevel(qualityDropdown.value);
    AudioListener.volume = volumeSlider.value;

    AudioManager.instance.StopSFX();
    AudioManager.instance.PlayMusic("bg");
  }

  void HandleControlCheck(){
    ControlType control = PlayerInfo.GetControlType();
    if(control == ControlType.NONE){
      //set the prompt active
      controlPrompt.SetActive(true);
      PlayerInfo.SetControlType(ControlType.TAP);
    }
  }

  public void UpdateControlTypeUI(){
    ControlType control = PlayerInfo.GetControlType();

    if(control == ControlType.TAP) {
      tapButton.GetComponent<Image>().color = tapButton.colors.selectedColor;
      tiltButton.GetComponent<Image>().color = tiltButton.colors.normalColor;
    }
    else if(control == ControlType.TILT) {
      tapButton.GetComponent<Image>().color = tapButton.colors.normalColor;
      tiltButton.GetComponent<Image>().color = tiltButton.colors.selectedColor;
    }
  }

  public void LoadGame(){
    SceneManager.LoadScene(1);
  }

  public void QuitGame(){
    Application.Quit();
  }

  public void SetControlType(int controlType){
    if(controlType == 1) PlayerInfo.SetControlType(ControlType.TAP);
    else if(controlType == 2) PlayerInfo.SetControlType(ControlType.TILT);

    UpdateControlTypeUI();
  }

  public void SetVolume(float volume){
    volumeText.text = volume.ToString("0.0");
    AudioListener.volume = volume;
  }

  public void SaveGameplaySettings(){
    StartCoroutine(SavingPrompt());
  }

  public void SetViewDist(float distance){
    viewDistanceText.text = distance.ToString("0.0");
    PlayerPrefs.SetFloat("viewDistance", distance);
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

  public void HowToPlayUpdate(){
    htpIndex= (htpIndex+1) % HTPsprites.Length;
    HTPimage.sprite = HTPsprites[htpIndex];
  }

  public void SetQuality(int index){
    QualitySettings.SetQualityLevel(index);
  }

  public void SaveQuality(){
    PlayerPrefs.SetInt("quality", QualitySettings.GetQualityLevel());
    StartCoroutine(SavingPrompt());
  }

  public void FppModeToggle(bool value){
    PlayerPrefs.SetInt("fpp", value?1:0);
  }

  public void ResetAll(){
    PlayerInfo.Reset();
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
        if(i == shipIndex) icon.color = new Color(1f,1f,1f,1f);
        else icon.color = new Color(1f,1f,1f,0.7f);
      }else{
        icon.color = new Color(1f,1f,1f,0.2f);
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
    return 400 * i;
  }
}
