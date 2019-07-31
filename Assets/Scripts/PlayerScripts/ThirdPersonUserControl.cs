using System;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;

[RequireComponent(typeof(ThirdPersonCharacter))]
public class ThirdPersonUserControl : MonoBehaviour
{
    private ThirdPersonCharacter m_Character; //ThirdPersonCharacter的对象的引用
    private Transform m_Cam; // 主摄像机的位置
    private Vector3 m_CamForward; // 当前相机的正前方
    private Vector3 m_Move; //根据相机的正前方和用户的输入,计算世界坐标相关的移动方向
    private bool m_Jump;
    private bool crouch;
    private float timePastCrouch;

    private void Start()
    {

        //获取主相机,这边的实现跟求控制器是一样的
        if (Camera.main != null)
        {
            m_Cam = Camera.main.transform;
        }
        else
        {
            // 在这个例子中我们使用世界坐标来控制，也许这不是他们做希望的，不过我们至少警告一下他们!
            Debug.LogWarning("不存在主摄像机,需要有一个MinCamera来跟随Player", gameObject);
        }

        // 获取第三人称的移动脚本，这个不能为空  
        m_Character = GetComponent<ThirdPersonCharacter>();
    }

    private void Update()
    {
        if (!m_Jump) //不在跳跃状态下，如果读到跳跃则更新变量  
        {
            m_Jump = Input.GetButtonDown("Jump");
        }
    }

    // 固定帧数，用于物理的同步  
    private void FixedUpdate()
    {

        if (LevelManager.Instance.gameState != GameState.InGame)
        {
            return;
        }
        timePastCrouch += Time.fixedDeltaTime;
        EventDispatcher.Outer.DispatchEvent(EventConst.EVENT_UPDATECDICON, timePastCrouch);
        // 获取用户的输入  
        //CrossPlatformInputManager是用来跨平台使用的,input在windows平台没问题,在其他品台可能就会出问题
        float h = CrossPlatformInputManager.GetAxis("Horizontal");
        float v = CrossPlatformInputManager.GetAxis("Vertical");
        if (Input.GetKeyDown(KeyCode.E) && CanCrouch())
        {
            crouch = !crouch;
            timePastCrouch = 0;
        }

        // 计算移动方向，并传递给角色  
        if (m_Cam != null)
        {
            //计算相机关联方向，这边同样强调了前方向  
            //把摄像机.forword的Y轴设为0并规范化
            m_CamForward = Vector3.Scale(m_Cam.forward, new Vector3(1, 0, 1)).normalized;
            //  print("m_Cam.forward  =  :" + m_Cam.forward);
            //  print("m_CamForward = "+m_CamForward);
            //  print("H = "+h);
            //  print("V = " + v);
            //v*摄像机的forWord
            //h*摄像机的right
            m_Move = v * m_CamForward + h * m_Cam.right;
            //  print("m_move = " + m_Move);
        }
        else
        {
            // 当没有相机时，直接以世界坐标轴作为参考  
            m_Move = v * Vector3.forward + h * Vector3.right;
        }
        //  将所有的参数传递给移动类  
        m_Character.Move(m_Move, crouch, m_Jump);
        m_Jump = false; //跳跃是个冲力，只要传一次就够了  
    }

    private bool CanCrouch()
    {
        if (timePastCrouch > GameSetting.StateSwitchCD)
            return true;
        return false;
    }
}