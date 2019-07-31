using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class UIScreenLevelSelection : UIScreen
{
    public Button[] BtnsLevel;
    public Button btnClose;
    public Button btnStart;

    public Text txt_LevelName;
    public Text txt_LevelDescription;
    public Text txt_TimeLimit;

    protected override void InitComponent()
    {
        for (int i = 0; i < BtnsLevel.Length; i++)
        {
            int index = i;
            BtnsLevel[i].onClick.AddListener(() => OnClickLevelSelectionButton(index));
            bool unlock = PlayerModel.GetLevelUnlockInfo(LevelInfoModel.Instance.GetLevelNameByIndex(i));
            BtnsLevel[i].GetComponent<Image>().color = unlock ? Color.red : Color.black;
            BtnsLevel[i].interactable = unlock;
        }
        btnClose.onClick.AddListener(OnClickCloseButton);
        btnStart.onClick.AddListener(OnClickStartButton);
        
        
    }

    protected override void InitData()
    {

    }

    protected override void InitView()
    {
        OnClickLevelSelectionButton(0);
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

    private void OnClickLevelSelectionButton(int index)
    {
        for (int i = 0, c = BtnsLevel.Length; i < c; i++)
        {
            if(i == index)
                BtnsLevel[i].transform.localScale = Vector3.one * 1.4f;
            else
                BtnsLevel[i].transform.localScale = Vector3.one;
        }
        
        txt_LevelName.text = LevelInfoModel.Instance.GetLevelNameByIndex(index);
        txt_LevelDescription.text = LevelInfoModel.Instance.GetLevelDescriptionByIndex(index);
        txt_TimeLimit.text = LevelInfoModel.Instance.GetTimeLimitByIndex(index).ToString();
        LWGameManager.currentLevelID = LevelInfoModel.Instance.GetIdByIndex(index);
        EventDispatcher.Outer.DispatchEvent(EventConst.EVENT_LEVELSELECT, index);
    }

    private void OnClickStartButton()
    {
        UIManager.Instance.Pop(UIDepthConst.MiddleDepth);
        UIManager.Instance.Pop(UIDepthConst.BottomDepth);
        MenuCamera.Instance.StartCamMove();
        MenuCharacter.Instance.StartStand();
        DOVirtual.DelayedCall(2.0f, () => { LWGameManager.Instance.GameStart(); });

    }

    private void OnClickCloseButton()
    {
        UIManager.Instance.Pop(UIDepthConst.MiddleDepth);
    }

}
