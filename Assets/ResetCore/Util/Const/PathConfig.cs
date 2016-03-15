using UnityEngine;
using System.Collections;
using System.IO;

public class PathConfig
{

    #region AssetBundle 相关
    public static readonly string resourceListDocPath = Application.dataPath + "/ResetCore/AssetBundle/Data/ResourcesList.xml";
    public static readonly string resourcePath = Application.dataPath + "/Resources";


    
    public static readonly string bundleFolderName = "AssetBundle";
    public static readonly string AssetRootBundlePath = PathConfig.bundleRootPath + "/" + bundleFolderName;
    public static string bundleRootPath
    {
        get
        {
            DirectoryInfo root = new DirectoryInfo(Application.dataPath);
            string rootPath = Path.Combine(root.Parent.FullName, bundleFolderName);
            return rootPath.Replace("\\", "/");
        }
    }

    public static readonly string bundleExportFolderName = "AssetBundleExport";
    public static string bundlePkgExportPath
    {
        get
        {
            DirectoryInfo root = new DirectoryInfo(Application.dataPath);
            string rootPath = Path.Combine(root.Parent.FullName, bundleExportFolderName);
            return rootPath.Replace("\\", "/");
        }
    }
    #endregion

    #region GameData相关
    //存放Xml的地址
    public static readonly string localGameDataXmlPath = Application.dataPath + "/ResetCore/DataGener/GameDatas/xml";
    //存放GameData类的地址
    public static readonly string localGameDataClassPath = Application.dataPath + "/ResetCore/DataGener/GameDatas/DataClasses";

    #endregion

    #region HttpProxy相关
    private const string internalUrl = "";
    private const string outUrl = "";
    #endregion
}
