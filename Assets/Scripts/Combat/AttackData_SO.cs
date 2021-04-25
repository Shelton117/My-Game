using UnityEngine;

/// <summary>
/// ��Ž�ɫ��������
/// </summary>
[CreateAssetMenu(fileName = "New AttackData", menuName = "Character Stats/Attack Data")]
public class AttackData_SO : ScriptableObject
{
    /// <summary>
    /// ������Χ
    /// </summary>
    [Header("Stats Info")]
    [Tooltip("������Χ")] public float attackRange;
    /// <summary>
    /// ���ܷ�Χ
    /// </summary>
    [Tooltip("���ܷ�Χ")] public float skillRange;

    /// <summary>
    /// ��ȴʱ��
    /// </summary>
    [Tooltip("��ȴʱ��")] public float coolDown;

    /// <summary>
    /// ��С�˺�
    /// </summary>
    [Tooltip("��С�˺�")] public int minDamage;
    /// <summary>
    /// ����˺�
    /// </summary>
    [Tooltip("����˺�")] public int maxDamage;

    /// <summary>
    /// �����˺��ӳ�
    /// </summary>
    [Tooltip("�����˺��ӳ�")] public float criticalMultiplier;
    /// <summary>
    /// ������
    /// </summary>
    [Tooltip("������")] public float criticalChance;
}
