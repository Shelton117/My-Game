using System.Collections.Generic;
using Assets.Scripts.Tools;
using Assets.Scripts.Transition;
using UnityEngine;
using Cinemachine;
using RPG.Tools;

namespace Assets.Scripts.Manager
{
    /// <summary>
    /// 游戏管理类
    /// 观察者实现载体
    /// </summary>
    public class GameManager : Singleton<GameManager>
    {
        /// <summary>
        /// 玩家属性
        /// </summary>
        [HideInInspector]
        public CharacterStats.MonoBehavior.CharacterStats PlayerStats;
        /// <summary>
        /// 跟随相机
        /// </summary>
        private CinemachineFreeLook followCamera;
        /// <summary>
        /// 被观察者
        /// </summary>
        List<IEndGameObserver> endGameObservers = new List<IEndGameObserver>();

        protected override void Awake()
        {
            base.Awake();
            DontDestroyOnLoad(this);
        }

        #region 共有方法接口

        /// <summary>
        /// 注册玩家（信息）
        /// </summary>
        /// <param name="Player"></param>
        public void RigisterPlayer(CharacterStats.MonoBehavior.CharacterStats Player)
        {
            PlayerStats = Player;

            followCamera = FindObjectOfType<CinemachineFreeLook>();
            if (followCamera != null)
            {
                followCamera.Follow = PlayerStats.transform.GetChild(2);
                followCamera.LookAt = PlayerStats.transform.GetChild(2);
            }
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

        /// <summary>
        /// 获取enter标签点
        /// </summary>
        /// <returns></returns>
        public Transform GetEntrance()
        {
            foreach (var item in FindObjectsOfType<TransitionDestination>())
            {
                if (item.destinationTag == DestinationTag.Enter)
                {
                    return item.transform;
                }
            }

            return null;
        }

        #endregion
    }
}