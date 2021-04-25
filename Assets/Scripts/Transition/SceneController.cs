using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;

/// <summary>
/// 场景跳转
/// </summary>
public class SceneController : Singleton<SceneController>
{
    /// <summary>
    /// 
    /// </summary>
    public GameObject playerPrefab;
    /// <summary>
    /// 
    /// </summary>
    private GameObject player;
    /// <summary>
    /// 
    /// </summary>
    private NavMeshAgent playerAgent;

    protected override void Awake()
    {
        base.Awake();
        DontDestroyOnLoad(this);
    }

    public void TransitionToDestination(TransitionPoint transitionPoint)
    {
        switch (transitionPoint.transitionType)
        {
            case TransitionType.SameScene:
                StartCoroutine(Transition(SceneManager.GetActiveScene().name, transitionPoint.destinationTag));
                break;
            case TransitionType.DifferentScene:
                StartCoroutine(Transition(transitionPoint.sceneName, transitionPoint.destinationTag));
                break;
        }
    }

    IEnumerator Transition(string sceneName, DestinationTag destinationTag)
    {
        //TODO:保存数据

        if (SceneManager.GetActiveScene().name != sceneName)
        {
            //跳转场景
            yield return SceneManager.LoadSceneAsync(sceneName);
            //LoadManager.Instance.LoadNextLevel(sceneName);
            //生成玩家
            yield return Instantiate(playerPrefab, GetDestination(destinationTag).transform.position,
                GetDestination(destinationTag).transform.rotation);
            //中断协程
            yield break;
        }
        else
        {
            player = GameManager.Instance.PlayerStats.gameObject;
            playerAgent = player.GetComponent<NavMeshAgent>();
            player.transform.SetPositionAndRotation(GetDestination(destinationTag).transform.position,
                GetDestination(destinationTag).transform.rotation);
            playerAgent.isStopped = true;

            yield return null;
        }
        
    }

    private TransitionDestination GetDestination(DestinationTag destinationTag)
    {
        var entrances = FindObjectsOfType<TransitionDestination>();
        foreach (var VARIABLE in entrances)
        {
            if (VARIABLE.destinationTag == destinationTag)
            {
                return VARIABLE;
            }
        }

        return null;
    }
}
