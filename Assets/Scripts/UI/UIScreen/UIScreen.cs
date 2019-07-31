using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class UIScreen : MonoBehaviour
{

    private object[] _datas;

    public virtual void OnInit(object[] datas)
    {
        _datas = datas;
        InitComponent();
        InitData();
        InitView();
    }

    protected abstract void InitComponent();
    protected abstract void InitData();
    protected abstract void InitView();
    public abstract void OnShow();
    public abstract void OnHide();
    public abstract void OnClose();


    public T ParseDataByIndex<T>(int index)
    {
        return (T)_datas[index];
    }
}
