using UnityEngine;
using System.Collections;
using UnityEditor;
using System.Collections.Generic;
using System.Linq;
using System.IO;

public class BundleBuildWindow : EditorWindow
{

    //显示窗口的函数
    [MenuItem("Assets/打开AssetBundle压缩包生成器")]
    static void ShowMainWindow()
    {
        Rect wr = new Rect(0, 0, 800, 800);
        BundleBuildWindow window = 
            EditorWindow.GetWindowWithRect(typeof(BundleBuildWindow), wr, true, "AssetBundle压缩包生成器") as BundleBuildWindow;
        window.Show();
    }

    List<string> pathList;
    void Awake()
    {
        pathList = new List<string>();
    }

    void OnGUI()
    {
        ShowPublicTools();
    }

    private void ShowPublicTools()
    {
        GUILayout.TextField("当前文件数量为" + pathList.Count);
        EditorGUILayout.BeginHorizontal();
        if (GUILayout.Button("添加文件", GUILayout.Width(200)))
        {
            var selection = Selection.GetFiltered(typeof(UnityEngine.Object), SelectionMode.DeepAssets);
            var paths = (from s in selection
                         let path = AssetDatabase.GetAssetPath(s)
                         where File.Exists(path)
                         select path).ToArray();
        }
        if (GUILayout.Button("清空文件", GUILayout.Width(200)))
        {

        }
        if (GUILayout.Button("显示文件目录", GUILayout.Width(200)))
        {
            foreach (string path in pathList)
            {
                Debug.logger.Log("文件目录" + path);
            }
        }
        EditorGUILayout.EndHorizontal();
    }

}
