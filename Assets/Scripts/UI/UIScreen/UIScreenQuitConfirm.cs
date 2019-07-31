using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIScreenQuitConfirm : UIScreen {

    public Button btn_Confirm;
    public Button btn_Cancel;
    public Button btn_BG;


    protected override void InitComponent()
    {
        btn_Confirm.onClick.AddListener(OnConfirmBtnClicked);
        btn_Cancel.onClick.AddListener(OnCancelBtnClicked);
        btn_BG.onClick.AddListener(OnCancelBtnClicked);
    }

    protected override void InitData()
    {

    }

    protected override void InitView()
    {

    }


    public override void OnClose()
    {

    }

    public override void OnHide()
    {

    }

    public override void OnShow()
    {

    }


    private void OnConfirmBtnClicked()
    {
        //TODO: 动效后退出
        Application.Quit();
    }

    private void OnCancelBtnClicked()
    {
        UIManager.Instance.Pop(UIDepthConst.TopDepth);
    }
}
