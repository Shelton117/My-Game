using UnityEngine;

/// <summary>
/// 存放角色攻击属性
/// </summary>
[CreateAssetMenu(fileName = "New AttackData", menuName = "Character Stats/Attack Data")]
public class AttackData_SO : ScriptableObject
{
    /// <summary>
    /// 攻击范围
    /// </summary>
    [Header("Stats Info")]
    [Tooltip("攻击范围")] public float attackRange;
    /// <summary>
    /// 技能范围
    /// </summary>
    [Tooltip("技能范围")] public float skillRange;

    /// <summary>
    /// 冷却时间
    /// </summary>
    [Tooltip("冷却时间")] public float coolDown;

    /// <summary>
    /// 最小伤害
    /// </summary>
    [Tooltip("最小伤害")] public int minDamage;
    /// <summary>
    /// 最大伤害
    /// </summary>
    [Tooltip("最大伤害")] public int maxDamage;

    /// <summary>
    /// 暴击伤害加成
    /// </summary>
    [Tooltip("暴击伤害加成")] public float criticalMultiplier;
    /// <summary>
    /// 暴击率
    /// </summary>
    [Tooltip("暴击率")] public float criticalChance;
}
