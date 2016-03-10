using UnityEngine;
using System.Collections;
using System.IO;

public class PathConfig {

    public static readonly string resourceListDocPath = Application.dataPath + "/Resources/Data/Common/ResourcesList.xml";
    public static readonly string resourcePath = Application.dataPath + "/Resources";

    public static readonly string exportBundleFolderName = "ExportedAssetBundle";
    public static string exportBundlePath
    {
        get
        {
            DirectoryInfo root = new DirectoryInfo(Application.dataPath);
            return Path.Combine(root.Parent.FullName, exportBundleFolderName).Replace("\\", "/");
        }
    }

    public static readonly string bundleFolderName = "AssetBundle";
    public static string bundlePath
    {
        get
        {
            DirectoryInfo root = new DirectoryInfo(Application.dataPath);
            string rootPath = Path.Combine(root.Parent.FullName, bundleFolderName);
            //rootPath = Path.Combine(rootPath, @"assets\resources").Replace("\\", "/");
            return rootPath.Replace("\\", "/");
        }
    }
    
	
}
