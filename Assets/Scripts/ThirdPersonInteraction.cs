using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ThirdPersonInteraction : MonoBehaviour
{

    public Transform[] detectBodies;
    
    private ThirdPersonCharacter player;
    private Dictionary<EnemyBase, float> allEnemyNotice = new Dictionary<EnemyBase, float>(); //Key是处在危险区域的
    private EnemyBase[] enemies;
    private List<EnemyBase> allTriggeredEnemies = new List<EnemyBase>();

    private float timePastFromBeingDangerous = 0;
    private bool beenFound;

    private void Start()
    {
        enemies = GameObject.FindObjectsOfType<EnemyBase>();
        for (int i = 0, c = enemies.Length; i < c; i++)
        {
            allEnemyNotice.Add(enemies[i], 0.0f);
        }
    }
    

    private void OnTriggerEnter(Collider other)
    {
        EnemyBase enemy = other.GetComponentInParent<EnemyBase>();

        if (enemy != null)
        {
            if (!allTriggeredEnemies.Contains(enemy))
            {
                allTriggeredEnemies.Add(enemy);
            }
        }
    }

    private void OnTriggerStay(Collider other)
    {
        EnemyBase enemy = other.GetComponentInParent<EnemyBase>();

        if (enemy != null)
        {
            bool beenHide = true;
            for (int i = 0, c = detectBodies.Length; i < c; i++)
            {
                Debug.DrawLine(detectBodies[i].position, enemy.eyes.position, Color.red);
                float dis = Vector3.Distance(enemy.eyes.position, detectBodies[i].position);
                Vector3 dir = (enemy.eyes.position - detectBodies[i].position).normalized;
                if (!Physics.Raycast(detectBodies[i].position, dir, dis, ~(1 << 8)))
                {
                    if (enemy.enemyType == EnemyType.Sniper && Vector3.Angle(-dir, enemy.transform.forward) >45f)
                    {
                        beenHide = true;
                    }
                    else
                    {
                        beenHide = false;
                        break;
                    }
                    beenHide = false;
                    break;
                }
            }

            if (beenHide && allTriggeredEnemies.Contains(enemy))
            {
                allTriggeredEnemies.Remove(enemy);
            }else if (!beenHide && !allTriggeredEnemies.Contains(enemy))
            {
                allTriggeredEnemies.Add(enemy);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        EnemyBase enemy = other.GetComponentInParent<EnemyBase>();

        if (enemy != null)
        {
            if (allTriggeredEnemies.Contains(enemy))
            {
                allTriggeredEnemies.Remove(enemy);
            }
        }
    }

    private void Update()
    {
        if (beenFound)
            return;
        for (int i = 0, c = enemies.Length; i < c; i++)
        {
            if (allTriggeredEnemies.Contains(enemies[i]))
            {
                allEnemyNotice[enemies[i]] += Time.deltaTime;
                float max = enemies[i].enemyType == EnemyType.Sniper
                    ? GameSetting.NoticeTimeBeforeAttackOfSniper
                    : GameSetting.NoticeTimeBeforeAttackOfSoldier;
                if (allEnemyNotice[enemies[i]] > max)
                {
                    LevelManager.Instance.SwitchGameState(GameState.End);
                    enemies[i].OnFoundPlayer(this);
                    beenFound = true;
                }
            }
            else
            {
                if (allEnemyNotice[enemies[i]] > 0)
                {
                    allEnemyNotice[enemies[i]] -= Time.deltaTime;
                }
            }
        }

        Dictionary<EnemyBase, float> tmp;
        tmp = allEnemyNotice.OrderByDescending(x => x.Value).ToDictionary(p => p.Key, o => o.Value);
        EventDispatcher.Outer.DispatchEvent(EventConst.EVENT_UPDATENOTICEICON, tmp.First().Value, 
            tmp.First().Key.enemyType == EnemyType.Sniper ? GameSetting.NoticeTimeBeforeAttackOfSniper : GameSetting.NoticeTimeBeforeAttackOfSoldier);
    }
}
