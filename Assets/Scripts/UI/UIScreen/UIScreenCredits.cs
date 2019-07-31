using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class UIScreenCredits : UIScreen
{
    public Button btnMore;
    public Button btnBack;
    public RectTransform rightPanel;


    protected override void InitComponent()
    {
        btnMore.onClick.AddListener(OnMoreButtonClicked);
        btnBack.onClick.AddListener(OnBackButtonClicked);
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
        rightPanel.DOAnchorPos(new Vector2(0, 0), 0.5f);
    }

    private void OnMoreButtonClicked()
    {
        Application.OpenURL("http://onlywaytosurvive.strikingly.com");
    }

    private void OnBackButtonClicked()
    {
        UIManager.Instance.Pop(UIDepthConst.MiddleDepth);
    }
}
