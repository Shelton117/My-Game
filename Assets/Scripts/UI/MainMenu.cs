using UnityEngine;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    /// <summary>
    /// 
    /// </summary>
    private Button newGameBtn;
    /// <summary>
    /// 
    /// </summary>
    private Button continueBtn;
    /// <summary>
    /// 
    /// </summary>
    private Button quitBtn;

    void Awake()
    {
        newGameBtn = transform.GetChild(1).GetComponent<Button>();
        continueBtn = transform.GetChild(2).GetComponent<Button>();
        quitBtn = transform.GetChild(3).GetComponent<Button>();

        newGameBtn.onClick.AddListener(newGame);
        continueBtn.onClick.AddListener(continueGame);
        quitBtn.onClick.AddListener(quitGame);
    }

    void newGame()
    {
        //�������
        PlayerPrefs.DeleteAll();
        //ת������
        SceneController.Instance.TransitionToFirshLevel();
    }

    void continueGame()
    {
        //ת������
        SceneController.Instance.TransitionToLoadGame();
    }

    void quitGame()
    {
        Application.Quit();
        Debug.Log("�˳���Ϸ");
    }
}
