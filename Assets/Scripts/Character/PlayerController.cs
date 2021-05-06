using System.Collections;
using UnityEngine;
using UnityEngine.AI;

/// <summary>
/// ��ҿ�����
/// </summary>
public class PlayerController : MonoBehaviour
{
    /// <summary>
    /// AI ����
    /// </summary>
    private NavMeshAgent agent;
    /// <summary>
    /// �����༭��
    /// </summary>
    private Animator anim;
    /// <summary>
    /// ����Ŀ��
    /// </summary>
    private GameObject attackTarget;
    /// <summary>
    /// ������ȴ
    /// </summary>
    private float lastAttackTime;
    /// <summary>
    /// ��ɫ����
    /// </summary>
    private CharacterStats characterStats;
    /// <summary>
    /// �Ƿ�����
    /// </summary>
    private bool isDead;
    /// <summary>
    /// ֹͣλ��
    /// </summary>
    private float stopDistance;

    void Awake()
    {
        //��ʼ�����
        agent = GetComponent<NavMeshAgent>();
        anim = GetComponent<Animator>();
        characterStats = GetComponent<CharacterStats>();

        stopDistance = agent.stoppingDistance;
    }

    void Start()
    {
        //��ȡ����
        SaveManager.Instance.LoadPlayerData();
    }

    void OnEnable()
    {
        //ע������¼�
        MouseManager.Instance.OnMouseClicked += MoveToTarget;
        MouseManager.Instance.OnEnemyClicked += EventAttack;
        //ע��Ϊ���۲���
        //��Ϸ�п�ֱ�Ӹ���GameManager��ȡ�����Ϣ
        GameManager.Instance.RigisterPlayer(characterStats);
    }

    void OnDisable()
    {
        //ע������¼�
        MouseManager.Instance.OnMouseClicked -= MoveToTarget;
        MouseManager.Instance.OnEnemyClicked -= EventAttack;
    }

    void Update()
    {
        SwichAnimation();

        //���ж��Ƿ�����
        isDead = characterStats.CurrentHealth == 0;
        
        //���������㲥�����ˣ��۲��ߣ�
        if (isDead)
        {
            GameManager.Instance.NotifyObserver();
        }

        //����ʱ����
        lastAttackTime -= Time.deltaTime;
    }

    #region ������¼����ƶ���������

    /// <summary>
    /// �ƶ���Ŀ�괦
    /// </summary>
    /// <param name="target"></param>
    public void MoveToTarget(Vector3 target)
    {
        //�ر�Я��
        StopAllCoroutines();

        if (isDead) return;

        agent.stoppingDistance = stopDistance;
        agent.isStopped = false;
        //����Ŀ�ĵ�
        agent.destination = target;
    }

    /// <summary>
    /// �����¼�
    /// </summary>
    /// <param name="target"></param>
    private void EventAttack(GameObject target)
    {
        if (isDead) return;

        if (target != null)
        {
            attackTarget = target;

            //�жϱ�����
            characterStats.isCritical = UnityEngine.Random.value < characterStats.attackData.criticalChance;

            //��ʼЭ��
            StartCoroutine(MoveToAttackTarget());
        }
    }

    #endregion

    #region ˽�з���

    /// <summary>
    /// ���ö���
    /// </summary>
    private void SwichAnimation()
    {
        anim.SetFloat("Speed", agent.velocity.sqrMagnitude);
        anim.SetBool("Die", isDead);
    }

    /// <summary>
    /// Э��
    /// �ƶ�&����ָ��
    /// </summary>
    /// <returns></returns>
    IEnumerator MoveToAttackTarget()
    {
        agent.isStopped = false;
        agent.stoppingDistance = characterStats.attackData.attackRange;

        transform.LookAt(attackTarget.transform);

        //���ù�����Χ
        while (Vector3.Distance(attackTarget.transform.position, transform.position) > characterStats.attackData.attackRange)
        {
            //�ƶ�������Ŀ�����ڵ�λ��
            agent.destination = attackTarget.transform.position;
            yield return null;
        }

        agent.isStopped = true;

        //Attack
        //��ȴ������ִ�й�������
        if (lastAttackTime < 0)
        {
            anim.SetTrigger("Attack");
            anim.SetBool("Critical", characterStats.isCritical);

            //������ȴʱ��
            lastAttackTime = characterStats.attackData.coolDown;
        }
    }

    /// <summary>
    /// Animation Event
    /// ����ִ֡��ʱ����
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
