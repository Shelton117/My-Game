using UnityEditor;
using UnityEngine;

namespace Assets.Editor
{
    /// <summary>
    /// 一键打包脚本
    /// TODO：学习并修改细节
    /// </summary>
    public class AutomatedPackaging : UnityEditor.Editor
    {
        private static string[] s;
        static void BuildPackage()
        {
            s = System.Environment.GetCommandLineArgs();
            var target = s[s.Length - 3];
            if (target.Equals("0")) //windows
            {
                BuildForStandaloneWindows();
            }
            else if (target.Equals("1")) //android
            {
                BuildForAndroid();
            }
            else if (target.Equals("2"))  //ios
            {
                BuildForIOS();
            }
            //其他平台自己看着写就行 
            s = null;
        }

        [MenuItem("Tools/ProjectBuild/BuildForAndroid")]
        static void BuildForAndroid()
        {
            string[] levels = { "Assets/Scenes/1.unity" };
            string path = GetExportPath(BuildTarget.Android);
            BuildPipeline.BuildPlayer(levels, path, BuildTarget.Android, BuildOptions.None);
        }

        [MenuItem("Tools/ProjectBuild/BuildForIOS")]
        static void BuildForIOS()
        {
            string[] levels = { "Assets/Scenes/1.unity" };
            string path = GetExportPath(BuildTarget.iOS);
            BuildPipeline.BuildPlayer(levels, path, BuildTarget.iOS, BuildOptions.None);
        }

        [MenuItem("Tools/ProjectBuild/BuildForStandaloneWindows")]
        static void BuildForStandaloneWindows()
        {
            string[] levels = { "Assets/Scenes/GameStart.unity" };
            string path = GetExportPath(BuildTarget.StandaloneWindows);
            PlayerSettings.defaultIsFullScreen = false;
            PlayerSettings.companyName = companyName;
            PlayerSettings.productName = productName;
            PlayerSettings.resizableWindow = false;
            PlayerSettings.displayResolutionDialog = ResolutionDialogSetting.Disabled;
            var tx = Resources.Load<Texture2D>("icon");
            Texture2D[] txs = { tx, tx, tx, tx, tx, tx, tx, };
            var lengt = PlayerSettings.GetIconsForTargetGroup(BuildTargetGroup.Standalone);
            if (txs != null)
                PlayerSettings.SetIconsForTargetGroup(BuildTargetGroup.Standalone, txs);
            var st = BuildPipeline.BuildPlayer(levels, path, BuildTarget.StandaloneWindows, BuildOptions.None);
        }

        private static string GetExportPath(BuildTarget target)
        {
            var s = System.Environment.GetCommandLineArgs();
            string path = s[s.Length - 2];
            string name = string.Empty;
            if (target == BuildTarget.Android)
            {
                name = "/" + s[s.Length - 1] + ".apk";
            }
            else if (target == BuildTarget.iOS)
            {
                name = "/" + s[s.Length - 1];
            }
            else if (target == BuildTarget.StandaloneWindows)
            {
                name = "/" + s[s.Length - 1] + ".exe";
            }

            UnityEngine.Debug.Log(path);
            UnityEngine.Debug.Log(name);
            string exepath = @path + name;
            UnityEngine.Debug.Log(exepath);
            return exepath;
        }

        private static string companyName
        {
            get
            {
                return "Shelton";
            }
        }
        private static string productName
        {
            get
            {
                return "3D RPG";
            }
        }
    }
}