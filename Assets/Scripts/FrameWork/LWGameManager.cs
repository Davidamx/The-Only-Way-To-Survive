using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LWGameManager : MonoSingleton<LWGameManager> {

    public static int currentLevelID;

    public void Init()
    {
        EventDispatcher.Inner.AddEventListener(EventConst.EVENT_GAMELOSE, GameOver);
    }
    
    public void GameStart()
    {
        UIManager.Instance.Push<UIScreenLoading>(UIDepthConst.TopDepth);
        SceneManager.LoadSceneAsync(LevelInfoModel.Instance.GetSceneName(currentLevelID)).completed += delegate
            {
                UIManager.Instance.Pop(UIDepthConst.TopDepth);
            };
    }

    public void LevelStart()
    {
        UIManager.Instance.Push<UIScreenHUD>(UIDepthConst.MiddleDepth, true,
            LevelInfoModel.Instance.GetTimeLimit(currentLevelID));
        StopAllCoroutines();
        StartCoroutine(LevelTimeLeftCounter());
    }

    private IEnumerator LevelTimeLeftCounter()
    {
        float t = LevelInfoModel.Instance.GetTimeLimit(currentLevelID);
        while (t > 0)
        {
            yield return null;
            t -= Time.deltaTime;
            EventDispatcher.Outer.DispatchEvent(EventConst.EVENT_UPDATETIMELEFT, t);
        }
        UIManager.Instance.Push<UIScreenResult>(UIDepthConst.MiddleDepth, true, false);
        EventDispatcher.Inner.DispatchEvent(EventConst.EVENT_GAMELOSE);
    }

    public void GameOver(object[] data)
    {
        StopAllCoroutines();
        EventDispatcher.Inner.RemoveListener(EventConst.EVENT_GAMELOSE, GameOver);
    }
    

}
