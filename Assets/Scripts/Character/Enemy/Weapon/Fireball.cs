using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Fireball : MonoBehaviour
{
    /// <summary>
    /// 自身刚体
    /// </summary>
    private Rigidbody rb;
    /// <summary>
    /// 火球造成的伤害
    /// </summary>
    public int damage;
    /// <summary>
    /// 给火球施加的力
    /// </summary>
    [Header("Basic Settings")] public float force;
    /// <summary>
    /// 攻击目标
    /// </summary>
    [HideInInspector] public GameObject target;
    /// <summary>
    /// 方向
    /// </summary>
    private Vector3 direction;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    void Start()
    {
        FlyToTarget();
    }

    void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            other.gameObject.GetComponent<NavMeshAgent>().isStopped = true;
            other.gameObject.GetComponent<NavMeshAgent>().velocity = direction * force;
            //other.gameObject.GetComponent<Animator>().SetTrigger("Dizzy");
            other.gameObject.GetComponent<CharacterStats>().TakeDamage(damage, other.gameObject.GetComponent<CharacterStats>());
            //Instantiate(breakEffect, transform.position, Quaternion.identity);
        }

        Destroy(gameObject);
    }

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
}
