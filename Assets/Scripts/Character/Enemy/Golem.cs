using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Golem : EnemyController
{
    /// <summary>
    /// 击飞的力
    /// </summary>
    [Header("Skill")] public float kickForce = 25f;
    /// <summary>
    /// 石头
    /// </summary>
    public GameObject rockObject;
    /// <summary>
    /// 手的位置
    /// </summary>
    public Transform handPos;

    #region Animation Event

    /// <summary>
    /// 技能效果
    /// 击退并使其眩晕
    /// </summary>
    public void KickOff()
    {
        //拓展方法 IsFacingTarget
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
    /// 扔石头
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
