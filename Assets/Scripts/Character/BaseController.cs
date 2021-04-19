using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

/// <summary>
/// ��ɫ״̬
/// </summary>
public enum EnemyStates
{
    /// <summary>
    /// ����
    /// </summary>
    GUARD,
    /// <summary>
    /// Ѳ��
    /// </summary>
    PATROL,
    /// <summary>
    /// ׷��
    /// </summary>
    CHASE,
    /// <summary>
    /// ����
    /// </summary>
    DEAD
}

/// <summary>
/// ��ɫ����
/// </summary>
public enum EnemyOrFriendlyForces
{
    Player,
    Enemy
}

/// <summary>
/// Ԥ������
/// ͨ�ý�ɫ������
/// �����޸ĳɳ�����
/// </summary>
[RequireComponent(typeof(NavMeshAgent))]
[RequireComponent(typeof(CharacterStats))]
public class BaseController : MonoBehaviour
{
    /// <summary>
    /// ���˵�״̬
    /// </summary>
    [SerializeField]
    private EnemyStates enemyStates;
    /// <summary>
    /// AI ����
    /// </summary>
    private NavMeshAgent agent;
    /// <summary>
    /// �����༭��
    /// </summary>
    protected Animator anim;
    /// <summary>
    /// ��ײ��
    /// </summary>
    private Collider coll;
    /// <summary>
    /// ��ɫ����
    /// </summary>
    protected CharacterStats characterStats;

    /// <summary>
    /// �����Ӿ���Χ
    /// </summary>
    [Header("Basic Settings")]
    [SerializeField]
    private float sightRadius;
    /// <summary>
    /// �����ƶ����ٶ�
    /// </summary>
    private float speed;
    /// <summary>
    /// ���˵�״̬
    /// </summary>
    [SerializeField]
    private bool isGruard;
    /// <summary>
    /// ����Ŀ��
    /// </summary>
    protected GameObject attackTarget;
    /// <summary>
    /// ���ʱ��
    /// </summary>
    [SerializeField]
    private float lookAtTime;
    /// <summary>
    /// �������ʱ��
    /// </summary>
    private float remainLookAtTime;
    /// <summary>
    /// ������ȴʱ��
    /// </summary>
    private float lastAttackTime;

    /// <summary>
    /// Ѳ�߷�Χ
    /// </summary>
    [Header("Patrol Settings")]
    [SerializeField] private float patrolRange;
    /// <summary>
    /// Ѳ�ߵ�
    /// </summary>
    private Vector3 wayPoint;
    /// <summary>
    /// ��ʼλ��
    /// </summary>
    private Vector3 guardPos;
    /// <summary>
    /// ��ʼ����
    /// </summary>
    private Quaternion guardRot;
    /// <summary>
    /// �Ѿ����߹���
    /// </summary>
    [SerializeField]
    private EnemyOrFriendlyForces enemyOrFriendlyForces;

    [Header(" ")]
    //���ƶ���״̬
    protected bool isWalk;
    protected bool isChase;
    private bool isFollow;
    private bool isDead;
    protected bool isPlayerDead;

    void Awake()
    {
        //��ʼ�����
        agent = GetComponent<NavMeshAgent>();
        anim = GetComponent<Animator>();
        characterStats = GetComponent<CharacterStats>();
        coll = GetComponent<BoxCollider>();
        //��ʼ������
        speed = agent.speed;
        guardPos = transform.position;
        guardRot = transform.rotation;
        remainLookAtTime = lookAtTime;
    }

    protected virtual void Start()
    {
        //����״̬
        if (isGruard)
        {
            enemyStates = EnemyStates.GUARD;
        }
        else
        {
            enemyStates = EnemyStates.PATROL;
            //�����Ѳ�ߵĵ�����Ҫ�����ø�Ѳ�ߵ�
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
    /// ���ӻ�
    /// </summary>
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, patrolRange);
    }

    /// <summary>
    /// ˢ�¶���
    /// </summary>
    private void SwichAnimation()
    {
        anim.SetBool("Walk", isWalk);
        anim.SetBool("Chase", isChase);
        anim.SetBool("Follow", isFollow);
        anim.SetBool("Die", isDead);

        //anim.SetFloat("Speed", agent.velocity.sqrMagnitude);
    }

    #region ˽�з���

    /// <summary>
    /// ˢ��״̬
    /// </summary>
    private void SwichStates()
    {
        if (isDead)
        {
            enemyStates = EnemyStates.DEAD;
        }
        else if (IsFoundPlayer())
        {
            //������Ҿ�׷��
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

                //�Ƿ������
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

                        //�жϱ�����
                        characterStats.isCritical = Random.value < characterStats.attackData.criticalChance;

                        //��������
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
    /// �Ƿ��ڹ�����Χ��
    /// ��ͨ����
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
    /// �Ƿ��ڹ�����Χ��
    /// ���⹥��������̫����ִ�н�ս
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
    /// ���ù�������
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
    /// �ж��Ƿ��ҵ����
    /// </summary>
    /// <returns></returns>
    private bool IsFoundPlayer()
    {
        var colliders = Physics.OverlapSphere(transform.position, sightRadius);

        foreach (var target in colliders)
        {
            if (target.CompareTag(enemyOrFriendlyForces.ToString()))
            {
                //������Ŀ�꣨Player������
                attackTarget = target.gameObject;
                return true;
            }
        }

        //Ŀ�궪ʧ���������
        attackTarget = null;
        return false;
    }

    /// <summary>
    /// ������һ��Ѳ�ߵ�
    /// </summary>
    private void GetNewWayPoint()
    {
        remainLookAtTime = lookAtTime;

        float randemX = Random.Range(-patrolRange, patrolRange);
        float randemZ = Random.Range(-patrolRange, patrolRange);

        Vector3 ramdomPoint = new Vector3(guardPos.x + randemX, transform.position.y, guardPos.z + randemZ);
        NavMeshHit hit;

        //�ж��Ƿ��ǿ����ж���λ��
        //������Ǿ����¼���
        wayPoint = NavMesh.SamplePosition(ramdomPoint, out hit, patrolRange, 1) ? ramdomPoint : transform.position;
    }

    /// <summary>
    /// Animation Event
    /// ����ִ֡��ʱ����
    /// </summary>
    void Hit()
    {
        //��չ���� IsFacingTarget
        if (attackTarget != null && transform.IsFacingTarget(attackTarget.transform))
        {
            var targetStats = attackTarget.GetComponent<CharacterStats>();

            targetStats.TakeDamage(characterStats, targetStats);
        }
    }

    #endregion
}