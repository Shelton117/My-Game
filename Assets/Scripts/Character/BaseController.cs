using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

/// <summary>
/// 角色状态
/// </summary>
public enum EnemyStates
{
    /// <summary>
    /// 警卫
    /// </summary>
    GUARD,
    /// <summary>
    /// 巡逻
    /// </summary>
    PATROL,
    /// <summary>
    /// 追击
    /// </summary>
    CHASE,
    /// <summary>
    /// 死亡
    /// </summary>
    DEAD
}

/// <summary>
/// 角色类型
/// </summary>
public enum EnemyOrFriendlyForces
{
    Player,
    Enemy
}

/// <summary>
/// 预添加组件
/// 通用角色控制器
/// 后续修改成抽象类
/// </summary>
[RequireComponent(typeof(NavMeshAgent))]
[RequireComponent(typeof(CharacterStats))]
public class BaseController : MonoBehaviour
{
    /// <summary>
    /// 敌人的状态
    /// </summary>
    [SerializeField]
    private EnemyStates enemyStates;
    /// <summary>
    /// AI 代理
    /// </summary>
    private NavMeshAgent agent;
    /// <summary>
    /// 动画编辑器
    /// </summary>
    protected Animator anim;
    /// <summary>
    /// 碰撞体
    /// </summary>
    private Collider coll;
    /// <summary>
    /// 角色数据
    /// </summary>
    protected CharacterStats characterStats;

    /// <summary>
    /// 怪物视觉范围
    /// </summary>
    [Header("Basic Settings")]
    [SerializeField]
    private float sightRadius;
    /// <summary>
    /// 敌人移动的速度
    /// </summary>
    private float speed;
    /// <summary>
    /// 敌人的状态
    /// </summary>
    [SerializeField]
    private bool isGruard;
    /// <summary>
    /// 攻击目标
    /// </summary>
    protected GameObject attackTarget;
    /// <summary>
    /// 侦查时间
    /// </summary>
    [SerializeField]
    private float lookAtTime;
    /// <summary>
    /// 保持侦查时间
    /// </summary>
    private float remainLookAtTime;
    /// <summary>
    /// 攻击冷却时间
    /// </summary>
    private float lastAttackTime;

    /// <summary>
    /// 巡逻范围
    /// </summary>
    [Header("Patrol Settings")]
    [SerializeField] private float patrolRange;
    /// <summary>
    /// 巡逻点
    /// </summary>
    private Vector3 wayPoint;
    /// <summary>
    /// 初始位置
    /// </summary>
    private Vector3 guardPos;
    /// <summary>
    /// 初始方向
    /// </summary>
    private Quaternion guardRot;
    /// <summary>
    /// 友军或者怪物
    /// </summary>
    [SerializeField]
    private EnemyOrFriendlyForces enemyOrFriendlyForces;

    [Header(" ")]
    //控制动画状态
    protected bool isWalk;
    protected bool isChase;
    private bool isFollow;
    private bool isDead;
    protected bool isPlayerDead;

    void Awake()
    {
        //初始化组件
        agent = GetComponent<NavMeshAgent>();
        anim = GetComponent<Animator>();
        characterStats = GetComponent<CharacterStats>();
        coll = GetComponent<BoxCollider>();
        //初始化参数
        speed = agent.speed;
        guardPos = transform.position;
        guardRot = transform.rotation;
        remainLookAtTime = lookAtTime;
    }

    protected virtual void Start()
    {
        //设置状态
        if (isGruard)
        {
            enemyStates = EnemyStates.GUARD;
        }
        else
        {
            enemyStates = EnemyStates.PATROL;
            //如果是巡逻的敌人需要先设置个巡逻点
            GetNewWayPoint();
        }
    }

    void Update()
    {
        isDead = characterStats.CurrentHealth == 0;

        if (!isPlayerDead)
        {
            SwichStates();
            SwichAnimation();

            lastAttackTime -= Time.deltaTime;
        }
    }

    /// <summary>
    /// 可视化
    /// </summary>
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, patrolRange);
    }

    /// <summary>
    /// 刷新动画
    /// </summary>
    private void SwichAnimation()
    {
        anim.SetBool("Walk", isWalk);
        anim.SetBool("Chase", isChase);
        anim.SetBool("Follow", isFollow);
        anim.SetBool("Die", isDead);

        //anim.SetFloat("Speed", agent.velocity.sqrMagnitude);
    }

    #region 私有方法

    /// <summary>
    /// 刷新状态
    /// </summary>
    private void SwichStates()
    {
        if (isDead)
        {
            enemyStates = EnemyStates.DEAD;
        }
        else if (IsFoundPlayer())
        {
            //遇到玩家就追击
            enemyStates = EnemyStates.CHASE;
        }

        switch (enemyStates)
        {
            case EnemyStates.GUARD:
                isChase = false;

                if (transform.position != guardPos)
                {
                    isWalk = true;
                    agent.isStopped = false;
                    agent.destination = guardPos;

                    if (Vector3.SqrMagnitude(guardPos - transform.position) <= agent.stoppingDistance)
                    {
                        isWalk = false;
                        transform.rotation = Quaternion.Lerp(transform.rotation, guardRot, 0.01f);
                    }
                }
                break;
            case EnemyStates.PATROL:
                isChase = false;
                agent.speed = speed * 0.5f;

                //是否到随机点
                if (Vector3.Distance(wayPoint, transform.position) <= agent.stoppingDistance)
                {
                    isWalk = false;
                    if (remainLookAtTime > 0)
                    {
                        remainLookAtTime -= Time.deltaTime;
                    }
                    else
                    {
                        GetNewWayPoint();
                    }
                }
                else
                {
                    isWalk = true;
                    agent.destination = wayPoint;
                }
                break;
            case EnemyStates.CHASE:
                isWalk = false;
                isChase = true;
                agent.speed = speed;

                if (!IsFoundPlayer())
                {
                    isFollow = false;
                    if (remainLookAtTime > 0)
                    {
                        agent.destination = transform.position;
                        remainLookAtTime -= Time.deltaTime;
                    }
                    else if (isGruard)
                    {
                        enemyStates = EnemyStates.GUARD;
                    }
                    else
                    {
                        enemyStates = EnemyStates.PATROL;
                    }
                }
                else
                {
                    isFollow = true;
                    agent.isStopped = false;
                    agent.destination = attackTarget.transform.position;
                }

                if (TargetInAttckRange() || TargetInSkillRange())
                {
                    isFollow = false;
                    agent.isStopped = true;

                    if (lastAttackTime < 0)
                    {
                        lastAttackTime = characterStats.attackData.coolDown;

                        //判断暴击率
                        characterStats.isCritical = Random.value < characterStats.attackData.criticalChance;

                        //攻击动画
                        Attack();
                    }
                }
                break;
            case EnemyStates.DEAD:
                coll.enabled = false;
                agent.radius = 0;

                Destroy(gameObject, 2f);
                break;
        }
    }

    /// <summary>
    /// 是否在攻击范围内
    /// 普通攻击
    /// </summary>
    /// <returns></returns>
    private bool TargetInAttckRange()
    {
        if (attackTarget != null)
        {
            return Vector3.Distance(attackTarget.transform.position, transform.position) <= characterStats.attackData.attackRange;
        }
        return false;
    }

    /// <summary>
    /// 是否在攻击范围内
    /// 特殊攻击，距离太近就执行近战
    /// </summary>
    /// <returns></returns>
    private bool TargetInSkillRange()
    {
        if (attackTarget != null)
        {
            return Vector3.Distance(attackTarget.transform.position, transform.position) <=
                   characterStats.attackData.skillRange &&
                   Vector3.Distance(attackTarget.transform.position, transform.position) >
                   characterStats.attackData.attackRange;
        }
        return false;
    }

    /// <summary>
    /// 调用攻击动画
    /// </summary>
    private void Attack()
    {
        transform.LookAt(attackTarget.transform);

        if (TargetInAttckRange())
        {
            anim.SetTrigger("Attack");
            anim.SetBool("Critical", characterStats.isCritical);
        }

        if (TargetInSkillRange())
        {
            anim.SetTrigger("Skill");
            anim.SetBool("Critical", characterStats.isCritical);
        }
    }

    /// <summary>
    /// 判断是否找到玩家
    /// </summary>
    /// <returns></returns>
    private bool IsFoundPlayer()
    {
        var colliders = Physics.OverlapSphere(transform.position, sightRadius);

        foreach (var target in colliders)
        {
            if (target.CompareTag(enemyOrFriendlyForces.ToString()))
            {
                //将攻击目标（Player）储存
                attackTarget = target.gameObject;
                return true;
            }
        }

        //目标丢失则清除储存
        attackTarget = null;
        return false;
    }

    /// <summary>
    /// 随机获得一个巡逻点
    /// </summary>
    private void GetNewWayPoint()
    {
        remainLookAtTime = lookAtTime;

        float randemX = Random.Range(-patrolRange, patrolRange);
        float randemZ = Random.Range(-patrolRange, patrolRange);

        Vector3 ramdomPoint = new Vector3(guardPos.x + randemX, transform.position.y, guardPos.z + randemZ);
        NavMeshHit hit;

        //判断是否是可以行动的位置
        //如果不是就重新计算
        wayPoint = NavMesh.SamplePosition(ramdomPoint, out hit, patrolRange, 1) ? ramdomPoint : transform.position;
    }

    /// <summary>
    /// Animation Event
    /// 动作帧执行时调用
    /// </summary>
    void Hit()
    {
        //拓展方法 IsFacingTarget
        if (attackTarget != null && transform.IsFacingTarget(attackTarget.transform))
        {
            var targetStats = attackTarget.GetComponent<CharacterStats>();

            targetStats.TakeDamage(characterStats, targetStats);
        }
    }

    #endregion
}