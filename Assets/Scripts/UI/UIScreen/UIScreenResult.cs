using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIScreenResult : UIScreen
{
    public Text txt_Title;
	public Text txt_Result;
	public Button btn_Confirm;


	private bool isWin;
	protected override void InitComponent()
	{
		btn_Confirm.onClick.AddListener(OnConfirmBtnClicked);
	}

	protected override void InitData()
	{
		isWin = ParseDataByIndex<bool>(0);
        if (isWin)
        {
            PlayerModel.SetLevelUnlock(LevelInfoModel.Instance.GetUnlockLevel(LWGameManager.currentLevelID));
        }
	}

	protected override void InitView()
	{
        txt_Title.text = isWin ? "You Survived" : " You Died";
        txt_Result.text = isWin ? LevelInfoModel.Instance.GetLevelResultWord(LWGameManager.currentLevelID) : GameSetting.Text_Result_Lose;
        AudioManager.Instance.PlayMusic(isWin ? MusicAudio.VictoryAudio : MusicAudio.FailureAudio);       
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
		UIManager.Instance.Push<UIScreenLoading>(UIDepthConst.TopDepth);
		SceneManager.LoadSceneAsync("MainMenu").completed += delegate
        {
            UIManager.Instance.PopToBottom();
        };
    }
}
