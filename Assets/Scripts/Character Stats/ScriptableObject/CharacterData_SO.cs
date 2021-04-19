using UnityEngine;

/// <summary>
/// 存放角色基础属性
/// </summary>
[CreateAssetMenu(fileName = "New Data", menuName = "Character Stats/Data")]
public class CharacterData_SO : ScriptableObject
{
    [Header("Stats Info")]
    /// <summary>
    /// 最大生命值
    /// </summary>
    public int maxHealth;
    /// <summary>
    /// 当前血量
    /// </summary>
    public int currentHealth;
    /// <summary>
    /// 基础防御
    /// </summary>
    public int baseDefence;
    /// <summary>
    /// 当前防御
    /// </summary>
    public int currentDefence;

    [Header("Killing Data")]
    public int KillPont;

    [Header("Level")]
    public int currentLevel;
    public int MaxLevel;
    public int baseExp;
    public int currentExp;
    public float LevelBuff;
    
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
        baseExp += (int)(baseExp * LevelBuff);
        //生命值处理
        maxHealth += (int)(maxHealth * LevelBuff);
        currentHealth = maxHealth;
        //其他属性...
    }
}
