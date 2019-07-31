using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class UIScreenHUD : UIScreen
{
    public Text txt_TimeLeft;
    public Image img_AttackedNotice;
    public Image img_CrouchCD;
    public Image img_Locking;

    private float totalTime;
    private float attackedNoticeTime;
    private EnemyBase lockingEnmey;

    protected override void InitComponent()
    {
        EventDispatcher.Outer.AddEventListener(EventConst.EVENT_UPDATETIMELEFT, UpdateTimeLeft);
        EventDispatcher.Outer.AddEventListener(EventConst.EVENT_UPDATENOTICEICON, UpdateAttackNotice);
        EventDispatcher.Outer.AddEventListener(EventConst.EVENT_UPDATECDICON, UpdateCrouchIcon);
        EventDispatcher.Outer.AddEventListener(EventConst.EVENT_ONENEMYFOUND, UpdateLockToEnemy);
    }

    protected override void InitData()
    {
        totalTime = ParseDataByIndex<float>(0);

    }

    protected override void InitView()
    {

    }
    
    public override void OnClose()
    {
        EventDispatcher.Outer.RemoveListener(EventConst.EVENT_UPDATETIMELEFT, UpdateTimeLeft);
        EventDispatcher.Outer.RemoveListener(EventConst.EVENT_UPDATENOTICEICON, UpdateAttackNotice);
        EventDispatcher.Outer.RemoveListener(EventConst.EVENT_UPDATECDICON, UpdateCrouchIcon);
        EventDispatcher.Outer.RemoveListener(EventConst.EVENT_ONENEMYFOUND, UpdateLockToEnemy);
    }

    public override void OnHide()
    {

    }

    public override void OnShow()
    {

    }

    private void UpdateTimeLeft(object[] data)
    {
        float timeLeft = (float) data[0];
        int t = (int) timeLeft;
        txt_TimeLeft.text = (t / 60).ToString() + ":" + (t % 60).ToString();
        txt_TimeLeft.color = Color.Lerp(Color.red, Color.green, timeLeft / totalTime);
    }

    private void UpdateAttackNotice(params object[] data)
    {
        float timing = (float)data[0];
        float total = (float)data[1];
        img_AttackedNotice.fillAmount = timing / total;
    }

    private void UpdateCrouchIcon(object[] data)
    {
        float tPast = (float) data[0];
        img_CrouchCD.fillAmount = tPast / GameSetting.StateSwitchCD;
    }

    private void UpdateLockToEnemy(object[] data)
    { 
        lockingEnmey = (EnemyBase) data[0];
        img_Locking.gameObject.SetActive(true);
        StartCoroutine(KeepLockingEnemy(lockingEnmey));
        img_Locking.transform.DOLocalRotate(new Vector3(0, 0, 180f), 1f).SetEase(Ease.Linear)
            .SetLoops(-1, LoopType.Incremental);
        img_Locking.transform.DOScale(Vector3.one * 0.6f, 1f);
    }

    private IEnumerator KeepLockingEnemy(EnemyBase enemy)
    {
        while (true)
        {
            Vector2 world2ScreenPos = CameraManager.Instance.mainCamera.WorldToScreenPoint(enemy.eyes.position);
            (img_Locking.transform as RectTransform).anchoredPosition = world2ScreenPos;
            yield return null;
        }
    }
    
    
    
}
