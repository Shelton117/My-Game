using UnityEngine;

/// <summary>
/// ��Ž�ɫ��������
/// </summary>
[CreateAssetMenu(fileName = "New Data", menuName = "Character Stats/Data")]
public class CharacterData_SO : ScriptableObject
{
    [Header("Stats Info")]
    [Tooltip("�������ֵ")] public int maxHealth;
    [Tooltip("��ǰѪ��")] public int currentHealth;
    [Tooltip("��������")] public int baseDefence;
    [Tooltip("��ǰ����")] public int currentDefence;

    [Header("Killing Data")]
    [Tooltip("��ɱ��þ���")] public int KillPont;

    [Header("Level")]
    [Tooltip("��ǰ�ȼ�")] public int currentLevel;
    [Tooltip("���ȼ�")] public int MaxLevel;
    [Tooltip("��������")] public int baseExp;
    [Tooltip("��ǰ����ֵ")] public int currentExp;
    [Tooltip("��������")] public float LevelBuff;
    
    /// <summary>
    /// ����
    /// </summary>
    private float LevelMultiplier
    {
        get { return 1 + (currentLevel - 1) * LevelBuff; }
    }

    #region ���нӿڷ������������ԣ�

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
    #endregion

    #region ˽�к���

    /// <summary>
    /// �������������ݵı仯
    /// </summary>
    private void LevelUp()
    {
        //�ȼ�����
        currentLevel = Mathf.Clamp(currentExp + 1, 0, MaxLevel);
        //����
        baseExp = (int)(baseExp * LevelMultiplier);
        //����ֵ����
        maxHealth += (int)(maxHealth * LevelBuff);
        currentHealth = maxHealth;
        //TODO����������...
    }

    #endregion
}
