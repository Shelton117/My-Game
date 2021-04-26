using System.Collections;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;

/// <summary>
/// 场景跳转控制器
/// 跳转逻辑
/// </summary>
public class SceneController : Singleton<SceneController>
{
    /// <summary>
    /// 玩家的预制体
    /// </summary>
    public GameObject playerPrefab;
    /// <summary>
    /// 玩家游戏对象
    /// </summary>
    private GameObject player;
    /// <summary>
    /// 玩家的AI代理
    /// </summary>
    private NavMeshAgent playerAgent;

    protected override void Awake()
    {
        base.Awake();
        DontDestroyOnLoad(this);
    }

    /// <summary>
    /// 传送到目标位置
    /// </summary>
    /// <param name="transitionPoint">目标点</param>
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

    /// <summary>
    /// 协程异步加载新场景
    /// </summary>
    /// <param name="sceneName">场景名</param>
    /// <param name="destinationTag"><目标点标签/param>
    /// <returns></returns>
    IEnumerator Transition(string sceneName, DestinationTag destinationTag)
    {
        //保存数据
        SaveManager.Instance.SavePlayerData();

        if (SceneManager.GetActiveScene().name != sceneName)
        {
            //跳转场景
            yield return SceneManager.LoadSceneAsync(sceneName);
            //LoadManager.Instance.LoadNextLevel(sceneName);
            //生成玩家
            yield return Instantiate(playerPrefab, GetDestination(destinationTag).transform.position,
                GetDestination(destinationTag).transform.rotation);
            //加载（恢复）数据
            SaveManager.Instance.LoadPlayerData();
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

    /// <summary>
    /// 获取目标点信息
    /// </summary>
    /// <param name="destinationTag"></param>
    /// <returns></returns>
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
