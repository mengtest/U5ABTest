using UnityEngine;
using System.Collections;
using UnityEditor;
using System.IO;
using System.Linq;
using System.Collections.Generic;

namespace ResetCore.Asset
{

    public class AssetBundleGen
    {

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


        public static void ExportBundle(string[] objPaths, string targetPath, bool withMeta)
        {
            AssetBundleBuild[] buildMap = new AssetBundleBuild[objPaths.Length];
            int i = 0;
            foreach (string path in objPaths)
            {
                buildMap[i].assetBundleName = path + ResourcesLoaderHelper.ExName;
                string[] buildAssetNames = new string[] { path };
                //buildAssetNames = objPath;
                buildMap[i].assetNames = buildAssetNames;
                Debug.logger.Log("AssetBundleGen", "Exporting：" + path);
                BuildPipeline.BuildAssetBundles(targetPath, buildMap, options, buildTarget);
                Debug.logger.Log("AssetBundleGen", "Export Success:" + path);
                i++;
            }

            //AssetBundleManifest manifest;
        }

        public static void CreateTargetFolder()
        {
            if (!Directory.Exists(PathConfig.bundleRootPath))
            {
                Directory.CreateDirectory(PathConfig.bundleRootPath);
            }
        }

    }

}