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

  public static void SetShipsUnlockMask(int i){
    int mask = PlayerPrefs.GetInt("shipUnlockMask", 1);
    PlayerPrefs.SetInt("shipUnlockMask", mask | 1<<i);
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

  public static int GetShipsUnlockMask(){
    return PlayerPrefs.GetInt("shipUnlockMask", 1);
  }

  public static float GetSensitivity(){
    return PlayerPrefs.GetFloat("sensitivity", 1f);
  }

  public static void SetSensitivity(float value){
    PlayerPrefs.SetFloat("sensitivity", value);
  }

  public static ControlType GetControlType(){
    int controlType = PlayerPrefs.GetInt("controlType", 0);
    switch(controlType){
      case 1: return ControlType.TAP;
      case 2: return ControlType.TILT;
      default: return ControlType.NONE;
    };
  }

  public static void SetControlType(ControlType type){
    PlayerPrefs.SetInt("controlType", (int) type);
  }

  public static void Reset(){
    PlayerPrefs.DeleteAll();
  }
}

public enum ControlType {
  NONE = 0, TAP, TILT
};
