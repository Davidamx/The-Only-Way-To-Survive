using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIDepth : MonoBehaviour {

    public UIDepthConst depth;

    public void Init()
    {
        gameObject.GetComponent<Canvas>().sortingOrder = (int)depth;
        gameObject.name = "Canvas_Depth" + depth.ToString();
    }
}
