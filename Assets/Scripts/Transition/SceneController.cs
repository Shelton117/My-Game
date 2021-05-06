using System.Collections;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;

/// <summary>
/// ������ת������
/// ��ת�߼�
/// </summary>
public class SceneController : Singleton<SceneController>, IEndGameObserver
{
    /// <summary>
    /// ��ҵ�Ԥ����
    /// </summary>
    public GameObject playerPrefab;
    /// <summary>
    /// ת��Ч����Ԥ����
    /// </summary>
    public SceneFader sceneFaderPrefab;
    /// <summary>
    /// �Ƿ�ת������
    /// </summary>
    private bool faderFinished;
    /// <summary>
    /// �����Ϸ����
    /// </summary>
    private GameObject player;
    /// <summary>
    /// ��ҵ�AI����
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

    #region ���к����ӿ�

    /// <summary>
    /// ���͵�Ŀ��λ��
    /// </summary>
    /// <param name="transitionPoint">Ŀ���</param>
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
    /// ��ʼ��һ��
    /// </summary>
    public void TransitionToFirshLevel()
    {
        StartCoroutine(LoadLevel("Level0"));
    }
    
    /// <summary>
    /// ������Ϸ
    /// ������ǰ������
    /// </summary>
    public void TransitionToLoadGame()
    {
        StartCoroutine(LoadLevel(SaveManager.Instance.SceneName));
    }

    #endregion

    #region ��ת������Э��

    /// <summary>
    /// Э���첽�����³���
    /// </summary>
    /// <param name="sceneName">������</param>
    /// <param name="destinationTag"><Ŀ����ǩ/param>
    /// <returns></returns>
    IEnumerator Transition(string sceneName, DestinationTag destinationTag)
    {
        //��������
        SaveManager.Instance.SavePlayerData();

        if (SceneManager.GetActiveScene().name != sceneName)
        {
            //��ת����
            yield return SceneManager.LoadSceneAsync(sceneName);
            //LoadManager.Instance.LoadNextLevel(sceneName);
            //�������
            yield return Instantiate(playerPrefab, GetDestination(destinationTag).transform.position,
                GetDestination(destinationTag).transform.rotation);
            //���أ��ָ�������
            SaveManager.Instance.LoadPlayerData();
            //�ж�Э��
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
    /// ���س���
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

            //������Ϸ
            SaveManager.Instance.SavePlayerData();
            yield return StartCoroutine(fade.FadeIn(2.5f));
            yield break;
        }
    }

    /// <summary>
    /// ���ؿ�ʼ����
    /// �ص���ʼ����
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

    #region ˽�з���

    /// <summary>
    /// ��ȡĿ�����Ϣ
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

    #region ʵ�ֽӿ�

    /// <summary>
    /// ʵ�ֹ۲����¼�
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
