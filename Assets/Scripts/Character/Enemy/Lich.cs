using UnityEngine;
using UnityEngine.AI;

/// <summary>
/// 巫师脚本
/// </summary>
public class Lich : EnemyController
{
    /// <summary>
    /// 击飞的力
    /// </summary>
    [Header("Skill")] public float kickForce = 25f;
    /// <summary>
    /// 火球
    /// </summary>
    public GameObject FireballObject;
    /// <summary>
    /// 手的位置
    /// </summary>
    public Transform handPos;

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

            Vector3 direction = (attackTarget.transform.position - transform.position).normalized;
            //direction.Normalize();
            attackTarget.GetComponent<NavMeshAgent>().isStopped = true;
            attackTarget.GetComponent<NavMeshAgent>().velocity = direction * kickForce;
            attackTarget.GetComponent<Animator>().SetTrigger("Dizzy");

            targetStats.TakeDamage(characterStats, targetStats);
        }
    }

    /// <summary>
    /// 扔火球
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
