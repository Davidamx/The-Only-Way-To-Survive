using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameDynamicData : Singleton<GameDynamicData> {

    private static int currentLevelID;
    public static int CurrentLevelID
    {
        get { return currentLevelID; }
        set { currentLevelID = value; }
    }
}
