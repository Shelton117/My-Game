using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

/// <summary>
/// ���ع�����
/// demo
/// </summary>
public class LoadManager : Singleton<LoadManager>
{
    /// <summary>
    /// loadingͼ
    /// </summary>
    public GameObject loadScreen;
    /// <summary>
    /// ������
    /// </summary>
    public Slider slider;
    /// <summary>
    /// ����
    /// </summary>
    public Text text;

    protected override void Awake()
    {
        base.Awake();
        DontDestroyOnLoad(this);
    }

    #region ���к����ӿ�

    /// <summary>
    /// ��ת����һ������
    /// </summary>
    /// <param name="sceneName">��ת������</param>
    public void LoadNextLevel(string sceneName)
    {
        StartCoroutine(LoadLevel(sceneName));
    }

    #endregion
    

    /// <summary>
    /// ͨ��Э�̵ķ�ʽ�첽���س���
    /// </summary>
    /// <param name="sceneName">������</param>
    /// <returns></returns>
    IEnumerator LoadLevel(string sceneName)
    {
        #region �ٷ�ʾ��

        //yield return null;

        ////Begin to load the Scene you specify
        //AsyncOperation asyncOperation = SceneManager.LoadSceneAsync("Gamestart");
        ////Don't let the Scene activate until you allow it to
        //asyncOperation.allowSceneActivation = false;
        //Debug.Log("Pro :" + asyncOperation.progress);
        ////When the load is still in progress, output the Text and progress bar
        //while (!asyncOperation.isDone)
        //{
        //    //Output the current progress
        //    text.text = "Loading progress: " + (asyncOperation.progress * 100) + "%";

        //    // Check if the load has finished
        //    if (asyncOperation.progress >= 0.9f)
        //    {
        //        //Change the Text to show the Scene is ready
        //        text.text = "Press the space bar to continue";
        //        //Wait to you press the space key to activate the Scene
        //        if (Input.GetKeyDown(KeyCode.Space))
        //            //Activate the Scene
        //            asyncOperation.allowSceneActivation = true;
        //    }

        //    yield return null;
        //}

        #endregion
        
        //��ʾloadingͼ
        loadScreen.SetActive(true);

        //�첽���س���
        AsyncOperation asyncOperation = SceneManager.LoadSceneAsync(sceneName);
        asyncOperation.allowSceneActivation = false;

        //����loading״̬
        while (!asyncOperation.isDone)
        {
            slider.value = asyncOperation.progress;
            text.text = "Loading progress: " + (asyncOperation.progress * 100) + "%";

            if (asyncOperation.progress >= 0.9f)
            {
                //Change the Text to show the Scene is ready
                text.text = "Press Anykey to continue";
                //Wait to you press the space key to activate the Scene
                if (Input.anyKeyDown)
                    //Activate the Scene
                    asyncOperation.allowSceneActivation = true;
            }

            yield return null;
        }
    }
}
