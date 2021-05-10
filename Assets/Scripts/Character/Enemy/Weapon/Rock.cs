using UnityEngine;
using UnityEngine.AI;

namespace Assets.Scripts.Character.Enemy.Weapon
{
    /// <summary>
    /// ʯͷ��״̬
    /// </summary>
    public enum RockStates
    {
        HitPlayer,
        HitEnemy,
        HitNothing
    }

    /// <summary>
    /// ʯͷ���
    /// </summary>
    public class Rock : MonoBehaviour
    {
        /// <summary>
        /// �������
        /// </summary>
        private Rigidbody rb;
        /// <summary>
        /// ʯͷ״̬
        /// </summary>
        public RockStates rockStates;
        /// <summary>
        /// ʯͷ��ɵ��˺�
        /// </summary>
        public int damage;
        /// <summary>
        /// ��ʯͷʩ�ӵ���
        /// </summary>
        [Header("Basic Settings")] public float force;
        /// <summary>
        /// ����Ŀ��
        /// </summary>
        [HideInInspector] public GameObject target;
        /// <summary>
        /// ʯͷ����Ķ���
        /// </summary>
        public GameObject breakEffect;
        /// <summary>
        /// ����
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
        /// ��⵽��ײ
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

        #region ���з����ӿ�

        /// <summary>
        /// ���򹥻�Ŀ��
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