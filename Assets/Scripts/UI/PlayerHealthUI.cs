using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 生命值&经验值UI组件
/// </summary>
public class PlayerHealthUI : MonoBehaviour
{
    /// <summary>
    /// 等级文本
    /// </summary>
    private Text levelText;
    /// <summary>
    /// 生命条
    /// </summary>
    private Image healthSlider;
    /// <summary>
    /// 经验条
    /// </summary>
    private Image expSlider;

    void Awake()
    {
        levelText = transform.GetChild(2).GetComponent<Text>();
        healthSlider = transform.GetChild(0).GetChild(0).GetComponent<Image>();
        expSlider = transform.GetChild(1).GetChild(0).GetComponent<Image>();
    }

    void Update()
    {
        UpdateHealth();
        UpdateExp();
    }

    #region 私有方法

    /// <summary>
    /// 刷新生命值
    /// </summary>
    private void UpdateHealth()
    {
        float healthSliderPercent = (float) GameManager.Instance.PlayerStats.CurrentHealth /
                                    GameManager.Instance.PlayerStats.MaxHealth;
        healthSlider.fillAmount = healthSliderPercent;
    }

    /// <summary>
    /// 刷新经验
    /// </summary>
    private void UpdateExp()
    {
        float expSliderPercent = (float)GameManager.Instance.PlayerStats.CurrentExp /
                                 GameManager.Instance.PlayerStats.BaseExp;
        expSlider.fillAmount = expSliderPercent;

        levelText.text = "LEVEL." + GameManager.Instance.PlayerStats.CurrentLevel.ToString("00");
    }

    #endregion
}
