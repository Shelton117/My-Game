using UnityEngine;

namespace Assets.Scripts.Transition
{
    /// <summary>
    /// Ŀ�ĵر�ǩ
    /// </summary>
    public enum DestinationTag
    {
        Enter,
        A,
        B,
        C
    }

    /// <summary>
    /// ����Ŀ�ĵ�
    /// </summary>
    public class TransitionDestination : MonoBehaviour
    {
        public DestinationTag destinationTag;
    }
}