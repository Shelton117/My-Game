using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBarUI : MonoBehaviour
{
    /// <summary>
    /// Ѫ����Ԥ����
    /// </summary>
    [SerializeField] private GameObject HealthUIBar;
    /// <summary>
    /// Ѫ����λ��
    /// </summary>
    [SerializeField] private Transform barPoint;
    /// <summary>
    /// Ѫ����ͼƬ
    /// �ɻ����Ĳ���
    /// </summary>
    private Image heathSlider;
    /// <summary>
    /// Ѫ����λ��
    /// </summary>
    private Transform UIbar;
    /// <summary>
    /// �����λ��
    /// ��Ѫ����Զ�������
    /// </summary>
    private Transform cam;
    /// <summary>
    /// ״̬����
    /// </summary>
    private CharacterStats currentStats;
    /// <summary>
    /// 
    /// </summary>
    [SerializeField]
    private bool isShowUpBar;
    /// <summary>
    /// ����ʱ��
    /// </summary>
    private float timeLeft;
    /// <summary>
    /// ��ʾʱ��
    /// </summary>
    [SerializeField]
    private float visibleTime;

    void Awake()
    {
        currentStats = GetComponent<CharacterStats>();
        currentStats.UpdateHealthBarOnAttack += UpdateHealthBar;
    }

    void LateUpdate()
    {
        if (UIbar != null)
        {
            UIbar.position = barPoint.position;
            UIbar.forward = -cam.forward;

            
            if (timeLeft <= 0 && !isShowUpBar)
            {
                UIbar.gameObject.SetActive(false);
            }
            else
            {
                timeLeft -= Time.deltaTime;
            }
        }
    }

    void OnEnable()
    {
        cam = Camera.main.transform;

        foreach (Canvas canvas in FindObjectsOfType<Canvas>())
        {
            //ͨ��renderMode����canvas
            //���ܻ���ֶ��renderMode == RenderMode.WorldSpace�����
            if (canvas.renderMode == RenderMode.WorldSpace)
            {
                UIbar = Instantiate(HealthUIBar, canvas.transform).transform;
                heathSlider = UIbar.GetChild(0).GetComponent<Image>();
                UIbar.gameObject.SetActive(isShowUpBar);
            }
        }
    }

    private void UpdateHealthBar(int current, int max)
    {
        if (current <= 0) Destroy(UIbar.gameObject);

        UIbar.gameObject.SetActive(true);
        timeLeft = visibleTime;

        float sliderPercent = (float)current / max;
        heathSlider.fillAmount = sliderPercent;
    }
}
