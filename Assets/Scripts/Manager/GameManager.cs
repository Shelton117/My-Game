using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 游戏管理类
/// 观察者实现载体
/// </summary>
public class GameManager : Singleton<GameManager>
{
    [HideInInspector]
    public CharacterStats PlayerStats;
    List<IEndGameObserver> endGameObservers = new List<IEndGameObserver>();

    /// <summary>
    /// 注册玩家（信息）
    /// </summary>
    /// <param name="Player"></param>
    public void RigisterPlayer(CharacterStats Player)
    {
        PlayerStats = Player;
    }

    /// <summary>
    /// 注册被观察者
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
    /// 注销被观察者
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
    /// 消息广播
    /// </summary>
    public void NotifyObserver()
    {
        foreach (var observer in endGameObservers)
        {
            observer.EndNotify();
        }
    }
}
