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
