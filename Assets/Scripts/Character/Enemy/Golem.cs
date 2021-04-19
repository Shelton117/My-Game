using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Golem : EnemyController
{
    /// <summary>
    /// ���ɵ���
    /// </summary>
    [Header("Skill")] public float kickForce = 25f;
    /// <summary>
    /// ʯͷ
    /// </summary>
    public GameObject rockObject;
    /// <summary>
    /// �ֵ�λ��
    /// </summary>
    public Transform handPos;

    #region Animation Event

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
            if (characterStats.isCritical)
            {
                Vector3 direction = (attackTarget.transform.position - transform.position).normalized;
                //direction.Normalize();
                attackTarget.GetComponent<NavMeshAgent>().isStopped = true;
                attackTarget.GetComponent<NavMeshAgent>().velocity = direction * kickForce;
                attackTarget.GetComponent<Animator>().SetTrigger("Dizzy");
            }

            targetStats.TakeDamage(characterStats, targetStats);
        }
    }

    /// <summary>
    /// ��ʯͷ
    /// </summary>
    public void ThrowRock()
    {
        if (attackTarget != null)
        {
            Instantiate(rockObject, handPos.position, Quaternion.identity);

            rockObject.GetComponent<Rock>().target = attackTarget;
        }
    }

    #endregion
}
