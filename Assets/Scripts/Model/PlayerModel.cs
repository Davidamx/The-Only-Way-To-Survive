using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerModel{

    public static bool Opened = false;

    public static void Init()
    {
        SetLevelUnlock(LevelInfoModel.Instance.GetLevelNameByIndex(0));
        Opened = true;
    }

    public static bool GetLevelUnlockInfo(string levelName)
    {
        return PlayerPrefs.GetInt(levelName, 0) == 1;
    }

    public static void SetLevelUnlock(string levelName)
    {
        PlayerPrefs.SetInt(levelName, 1);
    }

}
