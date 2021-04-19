using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum TransitionType
{
    SameScene,
    DifferentScene
}

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
