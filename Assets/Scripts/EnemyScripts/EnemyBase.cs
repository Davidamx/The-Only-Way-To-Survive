using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EnemyType
{
    Sniper = 0,
    Soldier = 1
}
public class EnemyBase : MonoBehaviour
{
    public EnemyType enemyType;
    public Transform eyes;
    private Animator anim;
    private float FireInterval = 4.0f;
    private float tmpT;

    public AudioClip[] gunAudios;
    private AudioSource audios;
    private AudioClip gunAudio;

    private void Start() 
    {
        anim = GetComponent<Animator>();
        audios = GetComponent<AudioSource>();
        gunAudio = gunAudios[(int)Random.Range(0, gunAudios.Length)];
        tmpT = FireInterval;
    }

    private void Update()
    {
        tmpT -= Time.deltaTime;
        if (tmpT < 0)
        {
            audios.PlayOneShot(gunAudio);
            tmpT = Random.Range(FireInterval, FireInterval * 3);
        }
    }


    public void OnFoundPlayer(ThirdPersonInteraction player)
    {
        anim.SetBool("Engaging", true);
        EventDispatcher.Outer.DispatchEvent(EventConst.EVENT_ONENEMYFOUND, this);
        StartCoroutine(StartAimingPlayer(player.transform));
        //插入士兵的声音
    }

    private IEnumerator StartAimingPlayer(Transform target)
    {
        float t = 2.0f;
        bool end = false;
        while (!end)
        {
            yield return null;
            t -= Time.deltaTime;
            RotateToTarget(target);

            if (t < 0 && !end)
            {
                end = true;
                //GameOver
                EventDispatcher.Inner.DispatchEvent(EventConst.EVENT_GAMELOSE);
                UIManager.Instance.Push<UIScreenResult>(UIDepthConst.MiddleDepth, true, false);
            }
        }
    }

    private void RotateToTarget(Transform target)
    {
        Quaternion newRot = Quaternion.LookRotation((target.position - transform.position).normalized, Vector3.up);
        transform.rotation = Quaternion.Slerp(transform.rotation, newRot, Time.deltaTime * 8);
    }
    
}