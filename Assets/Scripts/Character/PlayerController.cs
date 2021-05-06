using System.Collections;
using UnityEngine;
using UnityEngine.AI;

/// <summary>
/// 玩家控制类
/// </summary>
public class PlayerController : MonoBehaviour
{
    /// <summary>
    /// AI 代理
    /// </summary>
    private NavMeshAgent agent;
    /// <summary>
    /// 动画编辑器
    /// </summary>
    private Animator anim;
    /// <summary>
    /// 攻击目标
    /// </summary>
    private GameObject attackTarget;
    /// <summary>
    /// 攻击冷却
    /// </summary>
    private float lastAttackTime;
    /// <summary>
    /// 角色数据
    /// </summary>
    private CharacterStats characterStats;
    /// <summary>
    /// 是否死亡
    /// </summary>
    private bool isDead;
    /// <summary>
    /// 停止位置
    /// </summary>
    private float stopDistance;

    void Awake()
    {
        //初始化组件
        agent = GetComponent<NavMeshAgent>();
        anim = GetComponent<Animator>();
        characterStats = GetComponent<CharacterStats>();

        stopDistance = agent.stoppingDistance;
    }

    void Start()
    {
        //读取数据
        SaveManager.Instance.LoadPlayerData();
    }

    void OnEnable()
    {
        //注册玩家事件
        MouseManager.Instance.OnMouseClicked += MoveToTarget;
        MouseManager.Instance.OnEnemyClicked += EventAttack;
        //注册为被观察者
        //游戏中可直接根据GameManager获取玩家信息
        GameManager.Instance.RigisterPlayer(characterStats);
    }

    void OnDisable()
    {
        //注销玩家事件
        MouseManager.Instance.OnMouseClicked -= MoveToTarget;
        MouseManager.Instance.OnEnemyClicked -= EventAttack;
    }

    void Update()
    {
        SwichAnimation();

        //先判断是否死亡
        isDead = characterStats.CurrentHealth == 0;
        
        //玩家阵亡后广播给敌人（观察者）
        if (isDead)
        {
            GameManager.Instance.NotifyObserver();
        }

        //倒计时减少
        lastAttackTime -= Time.deltaTime;
    }

    #region 鼠标点击事件（移动、攻击）

    /// <summary>
    /// 移动到目标处
    /// </summary>
    /// <param name="target"></param>
    public void MoveToTarget(Vector3 target)
    {
        //关闭携程
        StopAllCoroutines();

        if (isDead) return;

        agent.stoppingDistance = stopDistance;
        agent.isStopped = false;
        //设置目的地
        agent.destination = target;
    }

    /// <summary>
    /// 攻击事件
    /// </summary>
    /// <param name="target"></param>
    private void EventAttack(GameObject target)
    {
        if (isDead) return;

        if (target != null)
        {
            attackTarget = target;

            //判断暴击率
            characterStats.isCritical = UnityEngine.Random.value < characterStats.attackData.criticalChance;

            //开始协程
            StartCoroutine(MoveToAttackTarget());
        }
    }

    #endregion

    #region 私有方法

    /// <summary>
    /// 设置动画
    /// </summary>
    private void SwichAnimation()
    {
        anim.SetFloat("Speed", agent.velocity.sqrMagnitude);
        anim.SetBool("Die", isDead);
    }

    /// <summary>
    /// 协程
    /// 移动&攻击指令
    /// </summary>
    /// <returns></returns>
    IEnumerator MoveToAttackTarget()
    {
        agent.isStopped = false;
        agent.stoppingDistance = characterStats.attackData.attackRange;

        transform.LookAt(attackTarget.transform);

        //设置攻击范围
        while (Vector3.Distance(attackTarget.transform.position, transform.position) > characterStats.attackData.attackRange)
        {
            //移动到攻击目标所在的位置
            agent.destination = attackTarget.transform.position;
            yield return null;
        }

        agent.isStopped = true;

        //Attack
        //冷却结束后执行攻击动画
        if (lastAttackTime < 0)
        {
            anim.SetTrigger("Attack");
            anim.SetBool("Critical", characterStats.isCritical);

            //重置冷却时间
            lastAttackTime = characterStats.attackData.coolDown;
        }
    }

    /// <summary>
    /// Animation Event
    /// 动作帧执行时调用
    /// </summary>
    void Hit()
    {
        if (attackTarget.CompareTag("Attckable"))
        {
            if (attackTarget.GetComponent<Rock>() && attackTarget.GetComponent<Rock>().rockStates == RockStates.HitNothing)
            {
                attackTarget.GetComponent<Rock>().rockStates = RockStates.HitEnemy;
                attackTarget.GetComponent<Rigidbody>().velocity = Vector3.one;
                attackTarget.GetComponent<Rigidbody>().AddForce(transform.forward * 20, ForceMode.Impulse);
            }
        }
        else
        {
            var targetStats = attackTarget.GetComponent<CharacterStats>();

            targetStats.TakeDamage(characterStats, targetStats);
        }
    }

    #endregion
}
