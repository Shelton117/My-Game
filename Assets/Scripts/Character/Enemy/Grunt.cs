using UnityEngine;
using UnityEngine.AI;

/// <summary>
/// ���˽ű�
/// </summary>
public class Grunt : EnemyController
{
    /// <summary>
    /// ���ɵ���
    /// </summary>
    [Header("Skill")] public float kickForce = 10f;

    /// <summary>
    /// ����Ч��
    /// ���˲�ʹ��ѣ��
    /// </summary>
    public void KickOff()
    {
        if (attackTarget != null)
        {
            transform.LookAt(attackTarget.transform);

            Vector3 direction = attackTarget.transform.position - transform.position;
            direction.Normalize();

            attackTarget.GetComponent<NavMeshAgent>().isStopped = true;
            attackTarget.GetComponent<NavMeshAgent>().velocity = direction * kickForce;
            attackTarget.GetComponent<Animator>().SetTrigger("Dizzy");
        }
    }
}
