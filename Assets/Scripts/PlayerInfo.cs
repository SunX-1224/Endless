using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInfo : MonoBehaviour {
    
    public static void SetHighScore(int score){
        int highScore = PlayerPrefs.GetInt("highScore", 0);
        if(score > highScore){
            PlayerPrefs.SetInt("highScore", score);
        }
    }

    public static void SetShards(int shards){
        PlayerPrefs.SetInt("shards", shards);
    }

    public static void SetShipIndex(int i){
        PlayerPrefs.SetInt("shipIndex", i);
    }

    public static int GetHighScore(){
        return PlayerPrefs.GetInt("highScore", 0);
    }

    public static int GetShards(){
        return PlayerPrefs.GetInt("shards", 0);
    }

    public static int GetShipIndex(){
        return PlayerPrefs.GetInt("shipIndex", 0);
    }
}
