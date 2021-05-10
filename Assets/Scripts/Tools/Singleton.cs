using UnityEngine;

namespace RPG.Tools
{
    /// <summary>
    /// ���͵�������
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class Singleton<T> : MonoBehaviour where T : Singleton<T>
    {
        private static T instance;

        /// <summary>
        /// ʵ��
        /// </summary>
        public static T Instance
        {
            get { return instance; }
        }

        /// <summary>
        /// �Ƿ����ɵ���
        /// </summary>
        public static bool F_isInitialized
        {
            get { return instance != null; }
        }

        protected virtual void Awake()
        {
            if (instance != null)
            {
                Destroy(gameObject);
            }
            else
            {
                instance = (T)this;
            }
        }

        protected virtual void OnDestroy()
        {
            if (instance == this)
            {
                instance = null;
            }
        }
    }

}