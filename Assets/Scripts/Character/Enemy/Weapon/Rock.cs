using UnityEngine;
using UnityEngine.AI;

namespace Assets.Scripts.Character.Enemy.Weapon
{
    /// <summary>
    /// 石头的状态
    /// </summary>
    public enum RockStates
    {
        HitPlayer,
        HitEnemy,
        HitNothing
    }

    /// <summary>
    /// 石头组件
    /// </summary>
    public class Rock : MonoBehaviour
    {
        /// <summary>
        /// 自身刚体
        /// </summary>
        private Rigidbody rb;
        /// <summary>
        /// 石头状态
        /// </summary>
        public RockStates rockStates;
        /// <summary>
        /// 石头造成的伤害
        /// </summary>
        public int damage;
        /// <summary>
        /// 给石头施加的力
        /// </summary>
        [Header("Basic Settings")] public float force;
        /// <summary>
        /// 攻击目标
        /// </summary>
        [HideInInspector] public GameObject target;
        /// <summary>
        /// 石头破碎的动画
        /// </summary>
        public GameObject breakEffect;
        /// <summary>
        /// 方向
        /// </summary>
        private Vector3 direction;

        void Awake()
        {
            rb = GetComponent<Rigidbody>();
            rb.velocity = Vector3.one;
            rockStates = RockStates.HitPlayer;
        }

        void Start()
        {
            FlyToTarget();
        }

        void FixedUpdate()
        {
            if (rb.velocity.sqrMagnitude < 1)
            {
                rockStates = RockStates.HitNothing;
            }
        }

        /// <summary>
        /// 检测到碰撞
        /// </summary>
        /// <param name="other"></param>
        void OnCollisionEnter(Collision other)
        {
            switch (rockStates)
            {
                case RockStates.HitPlayer:
                    if (other.gameObject.CompareTag("Player"))
                    {
                        other.gameObject.GetComponent<NavMeshAgent>().isStopped = true;
                        other.gameObject.GetComponent<NavMeshAgent>().velocity = direction * force;
                        other.gameObject.GetComponent<Animator>().SetTrigger("Dizzy");
                        other.gameObject.GetComponent<CharacterStats.MonoBehavior.CharacterStats>().TakeDamage(damage,
                            other.gameObject.GetComponent<CharacterStats.MonoBehavior.CharacterStats>());
                        Instantiate(breakEffect, transform.position, Quaternion.identity);
                        rockStates = RockStates.HitNothing;
                    }
                    break;
                case RockStates.HitEnemy:
                    if (other.gameObject.GetComponent<Golem>())
                    {
                        var otherStats = other.transform.GetComponent<CharacterStats.MonoBehavior.CharacterStats>();
                        otherStats.TakeDamage(damage, otherStats);

                        Destroy(gameObject);
                    }
                    break;
                case RockStates.HitNothing:
                    Destroy(gameObject);
                    break;
            }
        }

        #region 共有方法接口

        /// <summary>
        /// 飞向攻击目标
        /// </summary>
        public void FlyToTarget()
        {
            if (target == null)
            {
                target = FindObjectOfType<PlayerController>().gameObject;
            }
            direction = (target.transform.position - transform.position + Vector3.up).normalized;
            rb.AddForce(direction * force, ForceMode.Impulse);
        }

        #endregion
    }
}