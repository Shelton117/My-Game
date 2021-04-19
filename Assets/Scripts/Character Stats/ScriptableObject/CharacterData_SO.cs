using UnityEngine;

/// <summary>
/// ��Ž�ɫ��������
/// </summary>
[CreateAssetMenu(fileName = "New Data", menuName = "Character Stats/Data")]
public class CharacterData_SO : ScriptableObject
{
    [Header("Stats Info")]
    /// <summary>
    /// �������ֵ
    /// </summary>
    public int maxHealth;
    /// <summary>
    /// ��ǰѪ��
    /// </summary>
    public int currentHealth;
    /// <summary>
    /// ��������
    /// </summary>
    public int baseDefence;
    /// <summary>
    /// ��ǰ����
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
    /// �������߼�
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
    /// �������������ݵı仯
    /// </summary>
    private void LevelUp()
    {
        //�ȼ�����
        currentLevel = Mathf.Clamp(currentExp + 1, 0, MaxLevel);
        //����
        baseExp += (int)(baseExp * LevelBuff);
        //����ֵ����
        maxHealth += (int)(maxHealth * LevelBuff);
        currentHealth = maxHealth;
        //��������...
    }
}
