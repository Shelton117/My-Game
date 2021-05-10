using System.Collections;
using UnityEngine;

namespace Assets.Scripts.UI
{
    /// <summary>
    /// 渐入渐出组件
    /// </summary>
    public class SceneFader : MonoBehaviour
    {
        /// <summary>
        /// 通过Alpha实现淡入淡出
        /// </summary>
        private CanvasGroup canvasGroup;
        /// <summary>
        /// 
        /// </summary>
        [SerializeField] private float fadeInDuration;
        /// <summary>
        /// 
        /// </summary>
        [SerializeField] private float fadeOutDuration;


        private void Awake()
        {
            canvasGroup = GetComponent<CanvasGroup>();

            DontDestroyOnLoad(gameObject);
        }

        #region 公有协程（供外部调用）

        /// <summary>
        /// 渐入渐出
        /// </summary>
        /// <returns></returns>
        public IEnumerator FadeInOut()
        {
            yield return FadeOut(fadeOutDuration);
            yield return FadeIn(fadeInDuration);
        }

        /// <summary>
        /// 渐出
        /// </summary>
        /// <param name="time">持续时间</param>
        /// <returns></returns>
        public IEnumerator FadeOut(float time)
        {
            while (canvasGroup.alpha < 0)
            {
                canvasGroup.alpha += Time.deltaTime / time;
                yield return null;
            }
        }

        /// <summary>
        /// 渐入
        /// </summary>
        /// <param name="time">持续时间</param>
        /// <returns></returns>
        public IEnumerator FadeIn(float time)
        {
            while (canvasGroup.alpha != 0)
            {
                canvasGroup.alpha -= Time.deltaTime / time;
                yield return null;
            }

            Destroy(gameObject);
        }

        #endregion
    }
}