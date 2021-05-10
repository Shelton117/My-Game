using UnityEngine;

namespace Assets.Scripts.Transition
{
    /// <summary>
    /// ��������
    /// </summary>
    public enum TransitionType
    {
        SameScene,
        DifferentScene
    }

    /// <summary>
    /// ���͵�
    /// </summary>
    public class TransitionPoint : MonoBehaviour
    {
        [Header("Transition Imfo")]
        /// <summary>
        /// ������
        /// </summary>
        public string sceneName;
        /// <summary>
        /// ��ת����
        /// </summary>
        public TransitionType transitionType;
        /// <summary>
        /// ��ת��
        /// </summary>
        public DestinationTag destinationTag;
        /// <summary>
        /// �Ƿ���Դ���
        /// </summary>
        public bool canTrans;

        void Update()
        {
            if (Input.GetKeyDown(KeyCode.E) && canTrans)
            {
                SceneController.Instance.TransitionToDestination(this);
            }
        }

        void OnTriggerStay(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                canTrans = true;
            }
        }

        void OnTriggerExit(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                canTrans = false;
            }
        }
    }
}