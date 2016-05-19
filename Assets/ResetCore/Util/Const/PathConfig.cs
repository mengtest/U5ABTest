using UnityEngine;
using System.Collections;
using System.IO;

public class PathConfig
{

    #region 全局
    public static readonly string projectPath = Application.dataPath.ToLower().Replace("asset", "");
    public static readonly string assetResourcePath = "Assets/Resources/";

    #endregion

    #region AssetBundle 相关
    public static readonly string resourcePath = Application.dataPath + "/Resources/";
    //Bundle根目录
    public static readonly string resourceBundlePath = "Data/BundleData/";
    //资源列表目录
    public static readonly string resourceListDocPath = "Data/BundleData/ResourcesList";
    //场景记录文件储存目录
    public static readonly string sceneXmlRootPath = "Data/BundleData/SceneData/";
    //预置组件信息文件储存目录
    public static readonly string compInfoObjRootPath = "Data/BundleData/PrefabCompData/";
    
    //AssetBundle导出文件夹
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
    public static readonly string localGameDataExcelPath = Application.dataPath + "/Excel/";
    //存放Xml的地址
    public static readonly string localGameDataXmlPath = resourcePath + "Data/GameData/";
    //存放GameData类的地址
    public static readonly string localGameDataClassPath = Application.dataPath + "/ResetCore/DataGener/GameDatas/DataClasses/";

    #endregion

    #region NetPost
    public static readonly string NetPostURL = "127.0.0.1:8000";
    #endregion

    #region Lua
    public static readonly string localLuaDataXmlPath = resourcePath + "Data/Lua/";
    public static readonly string localModLuaFilePath = Application.persistentDataPath + "/Mod/Lua/";
    #endregion

    #region 工具
    public static readonly string csToolPath = Application.dataPath + "/ResetCore/CSTool/Editor/ExcelDataManager.exe";
    public static readonly string csTool_GameDataViaExcel = "GameDataGen";
    #endregion
}
