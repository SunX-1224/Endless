using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using TMPro;

public class StoreManager : MonoBehaviour{

    [SerializeField] GameObject buyPrompt;
    [SerializeField] TMP_Text costText;
    [SerializeField] TMP_Text shardsText;

    [SerializeField] List<GameObject> ships;

    int buyProcessShip = 0;

    void OnEnable(){
        UpdateShardsText();
        UpdateShipStatus();
    }

    void UpdateShipStatus(){

        int shipsMask = PlayerInfo.GetShipsUnlockMask();
        int shipIndex = PlayerInfo.GetShipIndex();

        for(int i=0; i < ships.Count; i++){
            RawImage icon = ships[i].GetComponentInChildren<RawImage>();

            if((shipsMask & (1<<i)) > 0){
                if(i == shipIndex) icon.color = new Color(1f,1f,1f,1f);
                else icon.color = new Color(1f,1f,1f,0.5f);
            }else{
                icon.color = new Color(0f,0f,0f,1f);
            }
        }
    }

    void UpdateShardsText(){
        int shards = PlayerInfo.GetShards();
        shardsText.text = $"Shards\n{shards}";
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
        costText.text = $"Unlock Ship\n{GetCost(index)} shards";
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
        }
        buyPrompt.SetActive(false);
    }

    int GetCost(int i){
        return 10*i;
    }
}
