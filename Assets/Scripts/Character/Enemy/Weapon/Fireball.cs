using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Fireball : MonoBehaviour
{
    /// <summary>
    /// �������
    /// </summary>
    private Rigidbody rb;
    /// <summary>
    /// ������ɵ��˺�
    /// </summary>
    public int damage;
    /// <summary>
    /// ������ʩ�ӵ���
    /// </summary>
    [Header("Basic Settings")] public float force;
    /// <summary>
    /// ����Ŀ��
    /// </summary>
    [HideInInspector] public GameObject target;
    /// <summary>
    /// ����
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
}
