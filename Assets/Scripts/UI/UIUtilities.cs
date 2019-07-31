using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

public class UIUtilities : MonoBehaviour {

    //UI的淡入淡出效果
    public static void DoFadeUI(GameObject uiObj, float endValue, float duration, Ease ease)
    {
        Image[] imgs = uiObj.GetComponentsInChildren<Image>();
        for (int i = 0; i < imgs.Length; i++)
        {
            imgs[i].DOFade(endValue, duration).SetEase(ease);
        }
    }

}
