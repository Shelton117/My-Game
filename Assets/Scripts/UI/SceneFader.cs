using System.Collections;
using UnityEngine;

namespace Assets.Scripts.UI
{
    /// <summary>
    /// ���뽥�����
    /// </summary>
    public class SceneFader : MonoBehaviour
    {
        /// <summary>
        /// ͨ��Alphaʵ�ֵ��뵭��
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

        #region ����Э�̣����ⲿ���ã�

        /// <summary>
        /// ���뽥��
        /// </summary>
        /// <returns></returns>
        public IEnumerator FadeInOut()
        {
            yield return FadeOut(fadeOutDuration);
            yield return FadeIn(fadeInDuration);
        }

        /// <summary>
        /// ����
        /// </summary>
        /// <param name="time">����ʱ��</param>
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
        /// ����
        /// </summary>
        /// <param name="time">����ʱ��</param>
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