using UnityEngine;
using UnityEngine.AI;

/// <summary>
/// ��ʦ�ű�
/// </summary>
public class Lich : EnemyController
{
    /// <summary>
    /// ���ɵ���
    /// </summary>
    [Header("Skill")] public float kickForce = 25f;
    /// <summary>
    /// ����
    /// </summary>
    public GameObject FireballObject;
    /// <summary>
    /// �ֵ�λ��
    /// </summary>
    public Transform handPos;

    /// <summary>
    /// ����Ч��
    /// ���˲�ʹ��ѣ��
    /// </summary>
    public void KickOff()
    {
        //��չ���� IsFacingTarget
        if (attackTarget != null && transform.IsFacingTarget(attackTarget.transform))
        {
            var targetStats = attackTarget.GetComponent<CharacterStats>();

            Vector3 direction = (attackTarget.transform.position - transform.position).normalized;
            //direction.Normalize();
            attackTarget.GetComponent<NavMeshAgent>().isStopped = true;
            attackTarget.GetComponent<NavMeshAgent>().velocity = direction * kickForce;
            attackTarget.GetComponent<Animator>().SetTrigger("Dizzy");

            targetStats.TakeDamage(characterStats, targetStats);
        }
    }

    /// <summary>
    /// �ӻ���
    /// </summary>
    public void ThrowFireball()
    {
        if (attackTarget != null)
        {
            Instantiate(FireballObject, handPos.position, Quaternion.identity);

            FireballObject.GetComponent<Fireball>().target = attackTarget;
        }
    }
}
