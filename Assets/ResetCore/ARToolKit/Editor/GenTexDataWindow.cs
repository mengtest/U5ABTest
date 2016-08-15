using UnityEngine;
using System.Collections;
using UnityEditor;
using System.Linq;
using System.IO;
using ResetCore.Util;

namespace ResetCore.AR
{
    public class GenTexDataWindow
    {
        public static readonly string arTexDataBuilderPath = 
            Path.Combine(PathConfig.ExtraToolPath, "ARToolKit/Editor/Tools/genTexData.exe");


        [MenuItem("Assets/ARToolKit/AR Texture Data Builder")]
        public static void ShowMainWindow()
        {
            var selection = Selection.GetFiltered(typeof(UnityEngine.Object), SelectionMode.DeepAssets);
            var paths = (from s in selection
                         let path = AssetDatabase.GetAssetPath(s)
                         where (path.EndsWith(".jpg"))
                         select path).ToArray();

            if(paths.Length > 1 || paths.Length == 0)
            {
                Debug.Log("Select Single File Please！");
                return;
            }

            CmdLuncher.LaunchExe(arTexDataBuilderPath, paths[0]);

        }
    }

}
