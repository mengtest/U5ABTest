using UnityEngine;
using System.Collections;
using UnityEditor;
using System.IO;
using System.Linq;

public class AssetBundleGen  {

    private static BuildTarget buildTarget
    {
        get
        {
            switch (EditorUserBuildSettings.activeBuildTarget)
            {
                case BuildTarget.Android:
                    return BuildTarget.Android;
                case BuildTarget.StandaloneWindows:
                    return BuildTarget.StandaloneWindows;
                case BuildTarget.StandaloneWindows64:
                    return BuildTarget.StandaloneWindows64;
                case BuildTarget.iOS:
                    return BuildTarget.iOS;
                default:
                    return BuildTarget.StandaloneWindows;
            }
        }
    }
    private static string bundlePath
    {
        get
        {
            return Path.Combine(PathConfig.exportBundlePath, buildTarget.ToString()).Replace("\\", "/");
        }
    }

    private static BuildAssetBundleOptions options
    {
        get
        {
            return BuildAssetBundleOptions.UncompressedAssetBundle;
        }
    }
   

	[MenuItem("Assets/AssetBundle/生成AssetBundle")]
    public static void GenAssetBundle()
    {
        ResourcesListGen.UpdateResourcesList();
        GenTargetFolder();

        var selection = Selection.GetFiltered(typeof(UnityEngine.Object), SelectionMode.DeepAssets);
        var paths = (from s in selection
                     let path = AssetDatabase.GetAssetPath(s)
                     where File.Exists(path)
                     select path).ToArray();
        foreach (string item in paths)
        {
            Debug.Log("ex " + item);
            //ExportBundle(new string[] { item }, targetPath, true);
        }

        Debug.Log(bundlePath);
    }

    private static void ExportBundle(string objName, string targetPath, bool withMeta)
    {
        
    }

    private static void GenTargetFolder()
    {
        if (!Directory.Exists(bundlePath))
        {
            Directory.CreateDirectory(bundlePath); 
        }
    }

}
