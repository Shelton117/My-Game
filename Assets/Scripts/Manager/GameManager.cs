using System.Collections.Generic;
using Assets.Scripts.Tools;
using Assets.Scripts.Transition;
using UnityEngine;
using Cinemachine;
using RPG.Tools;

namespace Assets.Scripts.Manager
{
    /// <summary>
    /// ��Ϸ������
    /// �۲���ʵ������
    /// </summary>
    public class GameManager : Singleton<GameManager>
    {
        /// <summary>
        /// �������
        /// </summary>
        [HideInInspector]
        public CharacterStats.MonoBehavior.CharacterStats PlayerStats;
        /// <summary>
        /// �������
        /// </summary>
        private CinemachineFreeLook followCamera;
        /// <summary>
        /// ���۲���
        /// </summary>
        List<IEndGameObserver> endGameObservers = new List<IEndGameObserver>();

        protected override void Awake()
        {
            base.Awake();
            DontDestroyOnLoad(this);
        }

        #region ���з����ӿ�

        /// <summary>
        /// ע����ң���Ϣ��
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

        /// <summary>
        /// ��ȡenter��ǩ��
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