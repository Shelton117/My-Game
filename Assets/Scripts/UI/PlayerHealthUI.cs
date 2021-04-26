using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// ����ֵ&����ֵUI���
/// </summary>
public class PlayerHealthUI : MonoBehaviour
{
    /// <summary>
    /// �ȼ��ı�
    /// </summary>
    private Text levelText;
    /// <summary>
    /// ������
    /// </summary>
    private Image healthSlider;
    /// <summary>
    /// ������
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

    #region ˽�з���

    /// <summary>
    /// ˢ������ֵ
    /// </summary>
    private void UpdateHealth()
    {
        float healthSliderPercent = (float) GameManager.Instance.PlayerStats.CurrentHealth /
                                    GameManager.Instance.PlayerStats.MaxHealth;
        healthSlider.fillAmount = healthSliderPercent;
    }

    /// <summary>
    /// ˢ�¾���
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
