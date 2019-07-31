using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathZoom : MonoBehaviour {


    private void OnTriggerEnter(Collider other)
    {
        ThirdPersonInteraction player = other.GetComponent<ThirdPersonInteraction>();
        if(player != null)
        {
            EventDispatcher.Inner.DispatchEvent(EventConst.EVENT_GAMELOSE);
            UIManager.Instance.Push<UIScreenResult>(UIDepthConst.MiddleDepth, true, false);
            LevelManager.Instance.SwitchGameState(GameState.End);
        }
    }
}
