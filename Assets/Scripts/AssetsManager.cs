using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using TMPro;

public class AssetsManager: MonoBehaviour{

    [SerializeField] GameObject buyPrompt;
    [SerializeField] GameObject insuffPrompt;
    [SerializeField] TMP_Text costText;
    [SerializeField] TMP_Text shardsText;
    [SerializeField] TMP_Text highScoreText;
    [SerializeField] List<GameObject> ships;

    int buyProcessShip = 0;

    void OnEnable(){
        UpdateScoreText();
        UpdateShardsText();
        UpdateShipStatus();
    }

    void UpdateShipStatus(){

        int shipsMask = PlayerInfo.GetShipsUnlockMask();
        int shipIndex = PlayerInfo.GetShipIndex();

        for(int i=0; i < ships.Count; i++){
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
        costText.text = $"Buy this Ship for {GetCost(index)} shards?";
        buyProcessShip = index;
    }

    public void HandleBuy(){
        int shards = PlayerInfo.GetShards();
        if(shards >= GetCost(buyProcessShip)){
            PlayerInfo.SetShards(shards - GetCost(buyProcessShip));
            PlayerInfo.SetShipsUnlockMask(buyProcessShip);
            PlayerInfo.SetShipIndex(buyProcessShip);
            UpdateShardsText();
            UpdateShipStatus();
            buyPrompt.SetActive(false);
        }else{
            buyPrompt.SetActive(false);
            insuffPrompt.SetActive(true);
        }
    }

    int GetCost(int i){
        return 10*i;
    }
}
