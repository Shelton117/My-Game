using UnityEngine;

/// <summary>
/// 存放角色基础属性
/// </summary>
[CreateAssetMenu(fileName = "New Data", menuName = "Character Stats/Data")]
public class CharacterData_SO : ScriptableObject
{
    [Header("Stats Info")]
    [Tooltip("最大生命值")] public int maxHealth;
    [Tooltip("当前血量")] public int currentHealth;
    [Tooltip("基础防御")] public int baseDefence;
    [Tooltip("当前防御")] public int currentDefence;

    [Header("Killing Data")]
    [Tooltip("击杀获得经验")] public int KillPont;

    [Header("Level")]
    [Tooltip("当前等级")] public int currentLevel;
    [Tooltip("最大等级")] public int MaxLevel;
    [Tooltip("基础经验")] public int baseExp;
    [Tooltip("当前经验值")] public int currentExp;
    [Tooltip("升级提升")] public float LevelBuff;
    
    private float LevelMultiplier
    {
        get { return 1 + (currentLevel - 1) * LevelBuff; }
    }

    /// <summary>
    /// 升级的逻辑
    /// </summary>
    /// <param name="point"></param>
    public void UpdateExp(int point)
    {
        currentExp += point;

        if (currentExp >= baseExp)
        {
            LevelUp();
        }
    }

    /// <summary>
    /// 处理升级后数据的变化
    /// </summary>
    private void LevelUp()
    {
        //等级处理
        currentLevel = Mathf.Clamp(currentExp + 1, 0, MaxLevel);
        //经验
        baseExp = (int)(baseExp * LevelMultiplier);
        //生命值处理
        maxHealth += (int)(maxHealth * LevelBuff);
        currentHealth = maxHealth;
        //其他属性...
    }
}
