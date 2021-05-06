using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

/// <summary>
/// 加载管理类
/// demo
/// </summary>
public class LoadManager : Singleton<LoadManager>
{
    /// <summary>
    /// loading图
    /// </summary>
    public GameObject loadScreen;
    /// <summary>
    /// 进度条
    /// </summary>
    public Slider slider;
    /// <summary>
    /// 进度
    /// </summary>
    public Text text;

    protected override void Awake()
    {
        base.Awake();
        DontDestroyOnLoad(this);
    }

    #region 公有函数接口

    /// <summary>
    /// 跳转到下一个场景
    /// </summary>
    /// <param name="sceneName">跳转场景名</param>
    public void LoadNextLevel(string sceneName)
    {
        StartCoroutine(LoadLevel(sceneName));
    }

    #endregion
    

    /// <summary>
    /// 通过协程的方式异步加载场景
    /// </summary>
    /// <param name="sceneName">场景名</param>
    /// <returns></returns>
    IEnumerator LoadLevel(string sceneName)
    {
        #region 官方示例

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
        
        //显示loading图
        loadScreen.SetActive(true);

        //异步加载场景
        AsyncOperation asyncOperation = SceneManager.LoadSceneAsync(sceneName);
        asyncOperation.allowSceneActivation = false;

        //更新loading状态
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
