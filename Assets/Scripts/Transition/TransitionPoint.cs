using UnityEngine;

namespace Assets.Scripts.Transition
{
    /// <summary>
    /// 传送类型
    /// </summary>
    public enum TransitionType
    {
        SameScene,
        DifferentScene
    }

    /// <summary>
    /// 传送点
    /// </summary>
    public class TransitionPoint : MonoBehaviour
    {
        [Header("Transition Imfo")]
        /// <summary>
        /// 场景名
        /// </summary>
        public string sceneName;
        /// <summary>
        /// 跳转类型
        /// </summary>
        public TransitionType transitionType;
        /// <summary>
        /// 跳转点
        /// </summary>
        public DestinationTag destinationTag;
        /// <summary>
        /// 是否可以传送
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