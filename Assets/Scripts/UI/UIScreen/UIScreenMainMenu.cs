using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using DG.Tweening;

public class UIScreenMainMenu : UIScreen
{
    public Image img_title;
    public Button btn_LevelSelection;
    public Button btn_Options;
    public Button btn_Credits;
    public Button btn_Quit;

    protected override void InitComponent()
    {
        btn_LevelSelection.onClick.AddListener(OnLevelSelectionButtonClicked);
        btn_Options.onClick.AddListener(OnOptionsButtonClicked);
        btn_Credits.onClick.AddListener(OnCreditsButtonClicked);
        btn_Quit.onClick.AddListener(OnQuitButtonClicked);
    }

    protected override void InitData()
    {

    }

    protected override void InitView()
    {
        AudioManager.Instance.PlayMusic(MusicAudio.MainMenuAudio);
        if(PlayerModel.Opened == false)
        {
            img_title.DOFade(1, 2f).onComplete = () =>
            {
                img_title.DOFade(0, 2f);
            };
        }
    }

    public override void OnClose()
    {

    }

    public override void OnHide()
    {

    }

    public override void OnShow()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    private void OnLevelSelectionButtonClicked()
    {
        UIManager.Instance.Push<UIScreenLevelSelection>(UIDepthConst.MiddleDepth, true);
    }

    private void OnOptionsButtonClicked()
    {
        UIManager.Instance.Push<UIScreenOptions>(UIDepthConst.MiddleDepth, true);
    }

    private void OnCreditsButtonClicked()
    {
        UIManager.Instance.Push<UIScreenCredits>(UIDepthConst.MiddleDepth, false);
    }

    private void OnQuitButtonClicked()
    {
        UIManager.Instance.Push<UIScreenQuitConfirm>(UIDepthConst.TopDepth, false);
    }
}
