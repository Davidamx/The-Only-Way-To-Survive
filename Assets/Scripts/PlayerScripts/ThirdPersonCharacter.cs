using UnityEngine;
// 第三人称移动类，这边没有相机层，并且使用的是刚体和胶囊碰撞的组合，在用户控制的脚本ThirdPersonUserControl中只用到了Move方法来控制角色 
[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(CapsuleCollider))]
[RequireComponent(typeof(Animator))]
public class ThirdPersonCharacter : MonoBehaviour
{
    /// <summary>
    /// 移动中转向的速度
    /// </summary>
    [SerializeField] float m_MovingTurnSpeed = 360;//移动中转向的速度
                                                   /// <summary>
                                                   /// 站立中转向的速度
                                                   /// </summary>
    [SerializeField] float m_StationaryTurnSpeed = 180;//站立中转向的速度  
                                                       /// <summary>
                                                       /// 跳跃产生的力量
                                                       /// </summary>
    [SerializeField] float m_JumpPower = 12f;//跳跃产生的力量  
                                             /// <summary>
                                             /// 重力
                                             /// </summary>
    [Range(1f, 4f)] [SerializeField] float m_GravityMultiplier = 2f;//重力
                                                                    /// <summary>
                                                                    /// 腿偏移值
                                                                    /// </summary>
    [SerializeField] float m_RunCycleLegOffset = 0.2f; //腿偏移值  

    /// <summary>
    /// 移动速度
    /// </summary>
    [SerializeField] float m_MoveSpeedMultiplier = 1f;//移动速度

    /// <summary>
    /// 动画播放速度
    /// </summary>
    [SerializeField] float m_AnimSpeedMultiplier = 1f;//动画播放速度

    /// <summary>
    /// 地面检查的距离
    /// </summary>
    [SerializeField] float m_GroundCheckDistance = 0.3f;//地面检查的距离  
                                                        /// <summary>
                                                        /// 刚体
                                                        /// </summary>
    Rigidbody m_Rigidbody;//刚体
                          /// <summary>
                          /// 动画状态机
                          /// </summary>
    Animator m_Animator;//动画状态机
                        /// <summary>
                        /// 是否在地面上
                        /// </summary>
    bool m_IsGrounded;//是否在地面上
                      /// <summary>
                      /// 地面距离检测的起始值
                      /// </summary>
    float m_OrigGroundCheckDistance;//地面距离检测的起始值
                                    /// <summary>
                                    /// 一半
                                    /// </summary>
    const float k_Half = 0.5f;//一半
                              /// <summary>
                              /// 转向值
                              /// </summary>
    float m_TurnAmount;//转向值
                       /// <summary>
                       /// 前进值
                       /// </summary>
    float m_ForwardAmount;//前进值
                          /// <summary>
                          /// 地面法向量
                          /// </summary>
    Vector3 m_GroundNormal;//地面法向量
                           /// <summary>
                           /// 胶囊高度
                           /// </summary>
    float m_CapsuleHeight;//胶囊高度
                          /// <summary>
                          /// 胶囊的中心
                          /// </summary>
    Vector3 m_CapsuleCenter;//胶囊的中心
                            /// <summary>
                            /// 胶囊体
                            /// </summary>
    CapsuleCollider m_Capsule;//胶囊体
                              /// <summary>
                              /// 是否是蹲伏状态
                              /// </summary>
    bool m_Crouching;//是否是蹲伏状态

    bool dead;

    //初始化动画\刚体和胶囊体
    void Start()
    {
        EventDispatcher.Inner.AddEventListener(EventConst.EVENT_GAMELOSE, OnGameLose);
        m_Animator = GetComponent<Animator>();
        m_Rigidbody = GetComponent<Rigidbody>();
        m_Capsule = GetComponent<CapsuleCollider>();
        m_CapsuleHeight = m_Capsule.height;//胶囊高度
        m_CapsuleCenter = m_Capsule.center; //胶囊的中心

        //锁定刚体的 XYZ轴的旋转
        m_Rigidbody.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationY | RigidbodyConstraints.FreezeRotationZ;
        m_OrigGroundCheckDistance = m_GroundCheckDistance;//保存一下地面检查值  

     }




    // 这是在FixedUpdate中调用的Move方法  
    /// <summary>
    /// 移动方法
    /// </summary>
    /// <param name="move">移动的方向</param>
    /// <param name="crouch">是否蹲伏</param>
    /// <param name="jump">是否跳跃</param>
    public void Move(Vector3 move, bool crouch, bool jump)
    {
        if (LevelManager.Instance.gameState != GameState.InGame)
            return;
        // 将一个世界坐标的输入转换为本地相关的转向和前进速度，需要考虑到角色头部的方向  
        if (move.sqrMagnitude > 1f) move.Normalize(); //向量大于1，则变为单位向量  

        move = transform.InverseTransformDirection(move);//将世界坐标系的方向和位置转换为自身坐标系

        //判断是否离开地面,
        //并修改:m_GroundNormal:地面法向量;
        //       m_IsGrounded //是否在地面上
        //       m_Animator.applyRootMotion//动画是否影响实际位置
        CheckGroundStatus();

        // [Vector3.ProjectOnPlane]圣典:投影向量到一个平面上,该平面由垂直到该法线的平面定义。
        move = Vector3.ProjectOnPlane(move, m_GroundNormal);//根据地面的法向量，产生一个对应平面的速度方向.

        //[Mathf.Atan2] :以弧度为单位计算并返回 y/x 的反正切值。返回值表示相对直角三角形对角的角，其中 x 是临边边长，而 y 是对边边长。
        //返回值为x轴和一个零点起始在(x,y)结束的2D向量的之间夹角。
        m_TurnAmount = Mathf.Atan2(move.x, move.z);//产生一个方位角，即与z轴的夹角，用于人物转向 
                                                   //Debug.Log("Mathf.Atan2(move.x, move.z)用于人物转向" + m_TurnAmount);


        m_ForwardAmount = move.z;//人物前进的数值

        //申请额外的旋转转
        ApplyExtraTurnRotation();//应用附加转弯  


        // 控制和速度处理，在地上和空中是不一样的  
        if (m_IsGrounded)//如果在地面上
        {
            //检测是否能跳
            HandleGroundedMovement(crouch, jump);
        }
        else
        {
            HandleAirborneMovement();
        }
        //缩小胶囊体,并判断是否可以站立
        ScaleCapsuleForCrouching(crouch);

        //在只能下蹲的区域保持下蹲
        PreventStandingInLowHeadroom();

        // 将输入和其他状态传递给动画组件  
        UpdateAnimator(move);
    }

    /// <summary>
    /// 缩小胶囊碰撞体
    /// </summary>
    /// <param name="crouch">是否蹲伏</param>
    void ScaleCapsuleForCrouching(bool crouch)
    {   //蹲下的一瞬间把胶囊高度和中心高度减半
        if (m_IsGrounded && crouch)
        {
            if (m_Crouching) return;
            m_Capsule.height = m_Capsule.height / 2f;
            m_Capsule.center = m_Capsule.center / 2f;
            m_Crouching = true;//吧正在蹲下设置为true,保证上面代码只执行一次
        }
        else
        {
            //创造一条刚体位置增加半径一半的位置，向上发射  
            Ray crouchRay = new Ray(m_Rigidbody.position + Vector3.up * m_Capsule.radius * k_Half, Vector3.up);

            //射线长度，胶囊原高度减少半径一半的位置,  
            float crouchRayLength = m_CapsuleHeight - m_Capsule.radius * k_Half;
            if (Physics.SphereCast(crouchRay, m_Capsule.radius * k_Half, crouchRayLength, Physics.AllLayers, QueryTriggerInteraction.Ignore))
            { //这边的意思是从角色底部向上丢一个球，然后那个k_Half相关的参数是为了放置在丢的时候就碰到了地面，而做的向上偏移  

                m_Crouching = true; //碰到了，说明角色无法回到站立状态  
                return;
            }
            // 没有碰到，回到初始的状态  
            m_Capsule.height = m_CapsuleHeight;
            m_Capsule.center = m_CapsuleCenter;
            m_Crouching = false;
        }
    }




    /// <summary>
    /// 在只能下蹲的区域保持下蹲  
    /// </summary>
    void PreventStandingInLowHeadroom()
    {
        // 在只能下蹲的区域保持下蹲  
        if (!m_Crouching)
        {                                                                       //radius半径
            Ray crouchRay = new Ray(m_Rigidbody.position + Vector3.up * m_Capsule.radius * k_Half, Vector3.up);
            float crouchRayLength = m_CapsuleHeight - m_Capsule.radius * k_Half;//扫描的长度
                                                                                //当球形扫描与任意碰撞器相交，返回true；否则返回false。   QueryTriggerInteraction:指定是否查询碰到触发器
            if (Physics.SphereCast(crouchRay, m_Capsule.radius * k_Half, crouchRayLength, Physics.AllLayers, QueryTriggerInteraction.Ignore))
            {
                m_Crouching = true;//下蹲
            }
        }
    }



    /// <summary>
    /// 用来更新动画状态机里的值
    /// </summary>
    /// <param name="move">移动参数</param>
    void UpdateAnimator(Vector3 move)
    {
        //更新动画参数
        m_Animator.SetFloat("Forward", m_ForwardAmount, 0.1f, Time.deltaTime);
        m_Animator.SetFloat("Turn", m_TurnAmount, 0.1f, Time.deltaTime);
        m_Animator.SetBool("Crouch", m_Crouching);
        m_Animator.SetBool("OnGround", m_IsGrounded);
        if (!m_IsGrounded)
        {
            m_Animator.SetFloat("Jump", m_Rigidbody.velocity.y);
        }

        // 计算哪只脚是在后面的，所以可以判断跳跃动画中哪只脚先离开地面  
        // 这里的代码依赖于特殊的跑步循环，假设某只脚会在未来的0到0.5秒内超越另一只脚  

        float runCycle = Mathf.Repeat(//获取当前是在哪个脚，Repeat相当于取模  
                m_Animator.GetCurrentAnimatorStateInfo(0).normalizedTime + m_RunCycleLegOffset, 1);

        float jumpLeg = (runCycle < k_Half ? 1 : -1) * m_ForwardAmount;
        if (m_IsGrounded)
        {
            m_Animator.SetFloat("JumpLeg", jumpLeg);
        }

        // 这边的方法允许我们在inspector视图中调整动画的速率，他会因为根运动影响移动的速度  
        if (m_IsGrounded && move.magnitude > 0)
        {
            m_Animator.speed = m_AnimSpeedMultiplier;
        }
        else
        {
            // 在空中的时候不用  
            m_Animator.speed = 1;
        }
    }

    /// <summary>
    /// 空中的处理,注意,空中跳跃和下蹲时不起作用的
    /// </summary>
    void HandleAirborneMovement()
    {
        //根据乘子引用额外的重力
        Vector3 extraGravityForce = (Physics.gravity * m_GravityMultiplier) - Physics.gravity;
        m_Rigidbody.AddForce(extraGravityForce);

        m_GroundCheckDistance = m_Rigidbody.velocity.y < 0 ? m_OrigGroundCheckDistance : 0.01f;//上升的时候不判断是否在地面上  
    }

    /// <summary>
    /// 跳跃方法,检测是否能跳
    /// </summary>
    /// <param name="crouch">是否蹲伏</param>
    /// <param name="jump">是否跳跃</param>
    void HandleGroundedMovement(bool crouch, bool jump)
    {

        //检查是否允许跳条件是正确的
        //m_Animator.GetCurrentAnimatorStateInfo(0)获取当前动画状态信息
        if (jump && !crouch && m_Animator.GetCurrentAnimatorStateInfo(0).IsName("Grounded"))
        {
            // 跳!
            m_Rigidbody.velocity = new Vector3(m_Rigidbody.velocity.x, m_JumpPower, m_Rigidbody.velocity.z);// //保存x、z轴速度，并给以y轴向上的速度
            m_IsGrounded = false;//是否跳跃为false
            m_Animator.applyRootMotion = false;//设置动画不影响实际位置
            m_GroundCheckDistance = 0.1f;//检测地面距离0.1
            print(m_GroundCheckDistance);
        }
    }

    /// <summary>
    /// 帮助角色快速转向，这是动画中根旋转的附加项  
    /// </summary>
    void ApplyExtraTurnRotation()
    {
        //帮助这个角色将更快(这是除了根旋转的动画)
        // 帮助角色快速转向，这是动画中根旋转的附加项  
        float turnSpeed = Mathf.Lerp(m_StationaryTurnSpeed, m_MovingTurnSpeed, m_ForwardAmount);
        transform.Rotate(0, m_TurnAmount * turnSpeed * Time.deltaTime, 0);//转向.  
    }

    /// <summary>
    /// 这个方法来覆盖默认的根运动，这个方法允许我们移除位置的速度  
    /// </summary>
    public void OnAnimatorMove()
    {
        // 我们实现了使用这个方法来代替基础的移动，这个方法允许我们移除位置的速度  
        if(dead)
            return;
        //我们实现这个函数来覆盖默认的根运动。
        //这允许我们修改之前位置速度的应用。
        if (m_IsGrounded && Time.deltaTime > 0)
        {
            Vector3 v = (m_Animator.deltaPosition * m_MoveSpeedMultiplier) / Time.deltaTime;


            // 保护一下y轴的移动速度 
            v.y = m_Rigidbody.velocity.y;
            m_Rigidbody.velocity = v;
        }
    }

    /// <summary>
    /// 检测是否离开地面
    /// </summary>
    void CheckGroundStatus()
    {
        RaycastHit hitInfo;
#if UNITY_EDITOR

        // 用来在场景视图中辅助想象地面检查射线
        //在场景中显示地面检查线，从脚上0.1米处往下射m_GroundCheckDistance的距离，预制体默认是0.3  
        Debug.DrawLine(transform.position + (Vector3.up * 0.1f), transform.position + (Vector3.up * 0.1f) + (Vector3.down * m_GroundCheckDistance));
#endif

        // 0.1的射线是比较小的，基础包中预制体所设置的0.3是比较好的  
        if (Physics.Raycast(transform.position + (Vector3.up * 0.1f), Vector3.down, out hitInfo, m_GroundCheckDistance))
        {//射到了，保存法向量，改变变量，将动画的applyRootMotion置为true，true的含义是应用骨骼节点的位移
         //就是说动画的运动会对实际角色坐标产生影响，用于精确的播放动画  
            m_GroundNormal = hitInfo.normal;//将射线触碰到的物体的法向量赋值给m_GroundNormal
            m_IsGrounded = true;//是否在地面上
            m_Animator.applyRootMotion = true;//动画影响实际位置
        }
        else
        {
            m_IsGrounded = false;//是否在地面上为false
            m_GroundNormal = Vector3.up;
            m_Animator.applyRootMotion = false;//动画不影响实际位置,不过我感觉这边不设为false也是可以的  
        }
    }

    public void OnGameLose(object[] data)
    {
        dead = true;
        m_Rigidbody.velocity = Vector3.zero;
        m_Animator.SetBool("Dead", true);
    }

    private void OnDestroy()
    {
        EventDispatcher.Inner.RemoveListener(EventConst.EVENT_GAMELOSE, OnGameLose);
    }
}
