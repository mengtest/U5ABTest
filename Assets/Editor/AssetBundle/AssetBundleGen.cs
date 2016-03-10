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

    private static BuildAssetBundleOptions options
    {
        get
        {
            return BuildAssetBundleOptions.None;
        }
    }
   

	[MenuItem("Assets/AssetBundle/生成AssetBundle")]
    public static void GenAssetBundle()
    {
        ResourcesListGen.UpdateResourcesList();
        CreateTargetFolder();

        var selection = Selection.GetFiltered(typeof(UnityEngine.Object), SelectionMode.DeepAssets);
        var paths = (from s in selection
                     let path = AssetDatabase.GetAssetPath(s)
                     where File.Exists(path)
                     select path).ToArray();

        ExportBundle(paths, PathConfig.bundleRootPath, true);

    }


    private static void ExportBundle(string[] objPaths, string targetPath, bool withMeta)
    {
        AssetBundleBuild[] buildMap = new AssetBundleBuild[objPaths.Length];
        int i = 0;
        foreach (string path in objPaths)
        {
            buildMap[i].assetBundleName = path.Substring(0, path.LastIndexOf('.')) + ResourcesLoaderHelper.ExName;
            string[] buildAssetNames = new string[] { path };
            //buildAssetNames = objPath;
            buildMap[i].assetNames = buildAssetNames;
            BuildPipeline.BuildAssetBundles(targetPath, buildMap, options, buildTarget);
            i++;
        }
        
        //AssetBundleManifest manifest;
    }

    private static void CreateTargetFolder()
    {
        if (!Directory.Exists(PathConfig.bundleRootPath))
        {
            Directory.CreateDirectory(PathConfig.bundleRootPath); 
        }
    }

}
