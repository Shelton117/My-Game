using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterStats : MonoBehaviour
{
    /// <summary>
    /// 
    /// </summary>
    public event Action<int, int> UpdateHealthBarOnAttack;
    /// <summary>
    /// ��ɫ��������
    /// </summary>
    public CharacterData_SO templateData;
    /// <summary>
    /// ��ɫ��������
    /// </summary>
    public AttackData_SO templateattackData;
    /// <summary>
    /// ��ɫ�������ݣ����ƣ�
    /// </summary>
    [HideInInspector] public CharacterData_SO characterData;
    /// <summary>
    /// ��ɫ�������ݣ����ƣ�
    /// </summary>
    [HideInInspector] public AttackData_SO attackData;
    /// <summary>
    /// �Ƿ񱩻�
    /// </summary>
    [HideInInspector]
    public bool isCritical;

    void Awake()
    {
        if (templateData != null)
        {
            characterData = Instantiate(templateData);
        }

        if (templateattackData != null)
        {
            attackData = Instantiate(templateattackData);
        }
    }

    #region read from Data_SO

    public int MaxHealth
    {
        get { if (characterData != null) return characterData.maxHealth; else return 0; }

        set { characterData.maxHealth = value; }
    }

    public int CurrentHealth
    {
        get { if (characterData != null) return characterData.currentHealth; else return 0; }

        set { characterData.currentHealth = value; }
    }

    public int BaseDefence
    {
        get { if (characterData != null) return characterData.baseDefence; else return 0; }

        set { characterData.baseDefence = value; }
    }

    public int CurrentDefence
    {
        get { if (characterData != null) return characterData.currentDefence; else return 0; }

        set { characterData.currentDefence = value; }
    }

    public int BaseExp
    {
        get { if (characterData != null) return characterData.baseExp; else return 0; }

        set { characterData.baseExp = value; }
    }

    public int CurrentExp
    {
        get { if (characterData != null) return characterData.currentExp; else return 0; }

        set { characterData.currentExp = value; }
    }

    public int CurrentLevel
    {
        get { if (characterData != null) return characterData.currentLevel; else return 0; }

        set { characterData.currentLevel = value; }
    }

    #endregion

    #region read from AttackData_SO

    public float AttackRange
    {
        get { if (attackData != null) return attackData.attackRange; else return 0; }
        set { attackData.attackRange = value; }
    }

    public float SkillRange
    {
        get { if (attackData != null) return attackData.skillRange; else return 0; }
        set { attackData.skillRange = value; }
    }

    public float CoolDown
    {
        get { if (attackData != null) return attackData.coolDown; else return 0; }
        set { attackData.coolDown = value; }
    }

    public int MinDamage
    {
        get { if (attackData != null) return attackData.minDamage; else return 0; }
        set { attackData.minDamage = value; }
    }

    public int MaxDamage
    {
        get { if (attackData != null) return attackData.maxDamage; else return 0; }
        set { attackData.maxDamage = value; }
    }

    public float CriticalMultiplier
    {
        get { if (attackData != null) return attackData.criticalMultiplier; else return 0; }
        set { attackData.criticalMultiplier = value; }
    }

    public float CriticalChance
    {
        get { if (attackData != null) return attackData.criticalChance; else return 0; }
        set { attackData.criticalChance = value; }
    }

    #endregion

    #region Character Combat

    /// <summary>
    /// �������߼�
    /// </summary>
    /// <param name="attacker">������</param>
    /// <param name="defener">������</param>
    public void TakeDamage(CharacterStats attacker, CharacterStats defener)
    {
        int damage = Mathf.Max(attacker.CurrentDamge() - defener.CurrentDefence, 0);

        CurrentHealth = Mathf.Max(CurrentHealth - damage, 0);

        if (attacker.isCritical)
        {
            defener.GetComponent<Animator>().SetTrigger("Hit");
        }
        //UI����
        UpdateHealthBarOnAttack?.Invoke(CurrentHealth, MaxHealth);
        //����
        if (CurrentHealth <= 0)
        {
            GameManager.Instance.PlayerStats.characterData.UpdateExp(characterData.KillPont);
        }
    }

    /// <summary>
    /// �������߼�(ʯͷ)
    /// </summary>
    /// <param name="attacker"></param>
    /// <param name="defener"></param>
    public void TakeDamage(int Damage, CharacterStats defener)
    {
        int damage = Mathf.Max(Damage - defener.CurrentDefence, 0);

        CurrentHealth = Mathf.Max(CurrentHealth - damage, 0);

        defener.GetComponent<Animator>().SetTrigger("Hit");
        //UI����
        UpdateHealthBarOnAttack?.Invoke(CurrentHealth, MaxHealth);
    }

    /// <summary>
    /// ���㱩������ʵ�˺���
    /// </summary>
    /// <returns></returns>
    private int CurrentDamge()
    {
        float coreDamage = UnityEngine.Random.Range(attackData.maxDamage, attackData.minDamage);
        if (isCritical)
        {
            coreDamage *= attackData.criticalMultiplier;
        }
        return (int) coreDamage;
    }

    #endregion
}
