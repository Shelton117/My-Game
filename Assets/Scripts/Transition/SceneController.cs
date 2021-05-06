using System.Collections;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;

/// <summary>
/// 场景跳转控制器
/// 跳转逻辑
/// </summary>
public class SceneController : Singleton<SceneController>, IEndGameObserver
{
    /// <summary>
    /// 玩家的预制体
    /// </summary>
    public GameObject playerPrefab;
    /// <summary>
    /// 转场效果的预制体
    /// </summary>
    public SceneFader sceneFaderPrefab;
    /// <summary>
    /// 是否转过场了
    /// </summary>
    private bool faderFinished;
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

    void Start()
    {
        GameManager.Instance.AddObserver(this);
        faderFinished = true;
    }

    void OnDisable()
    {
        if (!GameManager.F_isInitialized) return;
        GameManager.Instance.RemoveObserver(this);
    }

    #region 公有函数接口

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
    /// 开始第一关
    /// </summary>
    public void TransitionToFirshLevel()
    {
        StartCoroutine(LoadLevel("Level0"));
    }
    
    /// <summary>
    /// 继续游戏
    /// 加载先前的数据
    /// </summary>
    public void TransitionToLoadGame()
    {
        StartCoroutine(LoadLevel(SaveManager.Instance.SceneName));
    }

    #endregion

    #region 跳转场景的协程

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
    /// 加载场景
    /// </summary>
    /// <param name="scene"></param>
    /// <returns></returns>
    IEnumerator LoadLevel(string scene)
    {
        SceneFader fade = Instantiate(sceneFaderPrefab);

        if (scene != "")
        {
            yield return StartCoroutine(fade.FadeOut(2.5f));
            yield return SceneManager.LoadSceneAsync(scene);
            yield return player = Instantiate(playerPrefab, GameManager.Instance.GetEntrance().position,
                GameManager.Instance.GetEntrance().rotation);

            //保存游戏
            SaveManager.Instance.SavePlayerData();
            yield return StartCoroutine(fade.FadeIn(2.5f));
            yield break;
        }
    }

    /// <summary>
    /// 加载开始场景
    /// 回到开始场景
    /// </summary>
    /// <returns></returns>
    IEnumerator LoadStart()
    {
        SceneFader fade = Instantiate(sceneFaderPrefab);
        yield return StartCoroutine(fade.FadeOut(2.5f));
        yield return SceneManager.LoadSceneAsync("GameStart");
        yield return StartCoroutine(fade.FadeIn(2.5f));
        yield break;
    }

    #endregion

    #region 私有方法

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

    #endregion

    #region 实现接口

    /// <summary>
    /// 实现观察者事件
    /// </summary>
    public void EndNotify()
    {
        if (faderFinished)
        {
            faderFinished = false;
        }
        StartCoroutine(LoadStart());
    }

    #endregion
}
