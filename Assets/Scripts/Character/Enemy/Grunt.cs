using UnityEngine;
using UnityEngine.AI;

/// <summary>
/// 兽人脚本
/// </summary>
public class Grunt : EnemyController
{
    /// <summary>
    /// 击飞的力
    /// </summary>
    [Header("Skill")] public float kickForce = 10f;

    /// <summary>
    /// 技能效果
    /// 击退并使其眩晕
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
