using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ��Ϸ������
/// �۲���ʵ������
/// </summary>
public class GameManager : Singleton<GameManager>
{
    [HideInInspector]
    public CharacterStats PlayerStats;
    List<IEndGameObserver> endGameObservers = new List<IEndGameObserver>();

    /// <summary>
    /// ע����ң���Ϣ��
    /// </summary>
    /// <param name="Player"></param>
    public void RigisterPlayer(CharacterStats Player)
    {
        PlayerStats = Player;
    }

    /// <summary>
    /// ע�ᱻ�۲���
    /// </summary>
    /// <param name="observer"></param>
    public void AddObserver(IEndGameObserver observer)
    {
        if (!endGameObservers.Contains(observer))
        {
            endGameObservers.Add(observer);
        }
    }

    /// <summary>
    /// ע�����۲���
    /// </summary>
    /// <param name="observer"></param>
    public void RemoveObserver(IEndGameObserver observer)
    {
        if (endGameObservers.Contains(observer))
        {
            endGameObservers.Remove(observer);
        }
    }

    /// <summary>
    /// ��Ϣ�㲥
    /// </summary>
    public void NotifyObserver()
    {
        foreach (var observer in endGameObservers)
        {
            observer.EndNotify();
        }
    }
}
