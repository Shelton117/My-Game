using UnityEngine;

/// <summary>
/// ��Ž�ɫ��������
/// </summary>
[CreateAssetMenu(fileName = "New AttackData", menuName = "Character Stats/Attack Data")]
public class AttackData_SO : ScriptableObject
{
    [Header("Stats Info")]
    /// <summary>
    /// ������Χ
    /// </summary>
    public float attackRange;
    /// <summary>
    /// ���ܷ�Χ
    /// </summary>
    public float skillRange;

    /// <summary>
    /// ��ȴʱ��
    /// </summary>
    public float coolDown;

    /// <summary>
    /// ��С�˺�
    /// </summary>
    public int minDamage;
    /// <summary>
    /// ����˺�
    /// </summary>
    public int maxDamage;
    
    /// <summary>
    /// �����˺��ӳ�
    /// </summary>
    public float criticalMultiplier;
    /// <summary>
    /// ������
    /// </summary>
    public float criticalChance;
}
