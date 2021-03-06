using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.UI
{
    /// <summary>
    /// 其他血条栏
    /// </summary>
    public class HealthBarUI : MonoBehaviour
    {
        /// <summary>
        /// 血条的预制体
        /// </summary>
        [SerializeField] [Tooltip("血条的预制体")] private GameObject HealthUIBar;
        /// <summary>
        /// 血条的位置
        /// </summary>
        [SerializeField] [Tooltip("血条的位置")] private Transform barPoint;
        /// <summary>
        /// 血条的图片
        /// 可滑动的部分
        /// </summary>
        private Image heathSlider;
        /// <summary>
        /// 血条的位置
        /// </summary>
        private Transform UIbar;
        /// <summary>
        /// 相机的位置
        /// 让血条永远面向相机
        /// </summary>
        private Transform cam;
        /// <summary>
        /// 状态属性
        /// </summary>
        private CharacterStats.MonoBehavior.CharacterStats currentStats;
        /// <summary>
        /// 是否显示血条
        /// </summary>
        [SerializeField]
        [Tooltip("是否显示血条")] private bool isShowUpBar;
        /// <summary>
        /// 计算时间
        /// </summary>
        private float timeLeft;
        /// <summary>
        /// 显示时间
        /// </summary>
        [SerializeField]
        [Tooltip("显示时间")] private float visibleTime;

        void Awake()
        {
            currentStats = GetComponent<CharacterStats.MonoBehavior.CharacterStats>();
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
                //通过renderMode查找canvas
                //可能会出现多个renderMode == RenderMode.WorldSpace的情况
                if (canvas.renderMode == RenderMode.WorldSpace)
                {
                    UIbar = Instantiate(HealthUIBar, canvas.transform).transform;
                    heathSlider = UIbar.GetChild(0).GetComponent<Image>();
                    UIbar.gameObject.SetActive(isShowUpBar);
                }
            }
        }

        #region 私有方法

        private void UpdateHealthBar(int current, int max)
        {
            if (current <= 0) Destroy(UIbar.gameObject);

            UIbar.gameObject.SetActive(true);
            timeLeft = visibleTime;

            float sliderPercent = (float)current / max;
            heathSlider.fillAmount = sliderPercent;
        }

        #endregion
    }
}