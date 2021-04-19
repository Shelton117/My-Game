using UnityEngine;

/// <summary>
/// 存放角色攻击属性
/// </summary>
[CreateAssetMenu(fileName = "New AttackData", menuName = "Character Stats/Attack Data")]
public class AttackData_SO : ScriptableObject
{
    [Header("Stats Info")]
    /// <summary>
    /// 攻击范围
    /// </summary>
    public float attackRange;
    /// <summary>
    /// 技能范围
    /// </summary>
    public float skillRange;

    /// <summary>
    /// 冷却时间
    /// </summary>
    public float coolDown;

    /// <summary>
    /// 最小伤害
    /// </summary>
    public int minDamage;
    /// <summary>
    /// 最大伤害
    /// </summary>
    public int maxDamage;
    
    /// <summary>
    /// 暴击伤害加成
    /// </summary>
    public float criticalMultiplier;
    /// <summary>
    /// 暴击率
    /// </summary>
    public float criticalChance;
}
