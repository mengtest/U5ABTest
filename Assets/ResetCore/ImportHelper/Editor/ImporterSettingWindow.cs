using UnityEngine;
using System.Collections;
using UnityEditor;

namespace ResetCore.Editor.ImportHelper
{
    public class ImporterSettingWindow : EditorWindow
    {
        [MenuItem("Tools/资源导入规范化预置")]
        static void ShowMainWindow()
        {
            Rect wr = new Rect(0, 0, 800, 800);
            ImporterSettingWindow window = 
                EditorWindow.GetWindowWithRect(typeof(ImporterSettingWindow), wr, true, "资源导入规范化设置") as ImporterSettingWindow;
            window.Show();
        }

        void OnGUI()
        {

        }
    }

}
