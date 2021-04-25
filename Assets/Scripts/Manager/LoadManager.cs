using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LoadManager : Singleton<LoadManager>
{
    /// <summary>
    /// 
    /// </summary>
    public GameObject loadScreen;
    /// <summary>
    /// 
    /// </summary>
    public Slider slider;
    /// <summary>
    /// 
    /// </summary>
    public Text text;

    protected override void Awake()
    {
        base.Awake();
        DontDestroyOnLoad(this);
    }

    public void LoadNextLevel(string sceneName)
    {
        StartCoroutine(LoadLevel(sceneName));
    }

    IEnumerator LoadLevel(string sceneName)
    {
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
        loadScreen.SetActive(true);

        AsyncOperation asyncOperation = SceneManager.LoadSceneAsync(sceneName);
        asyncOperation.allowSceneActivation = false;
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
