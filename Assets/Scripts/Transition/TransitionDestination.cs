using UnityEngine;

namespace Assets.Scripts.Transition
{
    /// <summary>
    /// 目的地标签
    /// </summary>
    public enum DestinationTag
    {
        Enter,
        A,
        B,
        C
    }

    /// <summary>
    /// 传送目的地
    /// </summary>
    public class TransitionDestination : MonoBehaviour
    {
        public DestinationTag destinationTag;
    }
}