using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Newspaper : MonoBehaviour
{


    private Material mat;
    
    private void Start()
    {
        mat = GetComponent<Renderer>().material;
        EventDispatcher.Outer.AddEventListener(EventConst.EVENT_LEVELSELECT, OnLevelSelect);
    }

    private void OnLevelSelect(params object[] datas)
    {
        int levelIndex = (int)datas[0];
        string newspaper = LevelInfoModel.Instance.GetNewspaperNameByIndex(levelIndex);
        mat.mainTexture = ResourceLoader.Instance.Load<Texture>("Textures/" + newspaper);
    }

    private void OnDestroy()
    {
        EventDispatcher.Outer.RemoveListener(EventConst.EVENT_LEVELSELECT, OnLevelSelect);
    }
    
    
}
