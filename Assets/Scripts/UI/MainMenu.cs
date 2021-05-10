using Assets.Scripts.Transition;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.UI;

namespace Assets.Scripts.UI
{
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
        /// <summary>
        /// 
        /// </summary>
        private PlayableDirector playableDirector;

        void Awake()
        {
            newGameBtn = transform.GetChild(1).GetComponent<Button>();
            continueBtn = transform.GetChild(2).GetComponent<Button>();
            quitBtn = transform.GetChild(3).GetComponent<Button>();

            newGameBtn.onClick.AddListener(PlayTimeline);
            continueBtn.onClick.AddListener(continueGame);
            quitBtn.onClick.AddListener(quitGame);

            playableDirector = FindObjectOfType<PlayableDirector>();
            playableDirector.stopped += newGame;
        }

        #region ��ť�¼�

        private void PlayTimeline()
        {
            playableDirector.Play();
        }

        void newGame(PlayableDirector playableDirector)
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

#if UNITY_EDITOR
            Debug.Log("�˳���Ϸ");
#endif
        }

        #endregion
    }
}