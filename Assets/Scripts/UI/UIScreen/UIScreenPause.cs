using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIScreenPause : UIScreen
{
    public Button btnContinue;
    public Button btnQuit;


    protected override void InitComponent()
    {
        btnContinue.onClick.AddListener(OnContinueBtnClicked);
        btnQuit.onClick.AddListener(OnQuitBtnClicked);
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
        Time.timeScale = 1;
    }

    public override void OnShow()
    {
        Time.timeScale = 0;
    }

    private void OnQuitBtnClicked()
    {
        UIManager.Instance.Push<UIScreenLoading>(UIDepthConst.TopDepth);
        SceneManager.LoadSceneAsync("MainMenu").completed += delegate
        {
            UIManager.Instance.PopToBottom();
        };
    }

    private void OnContinueBtnClicked()
    {
        UIManager.Instance.Pop(UIDepthConst.TopDepth);
    }
}
