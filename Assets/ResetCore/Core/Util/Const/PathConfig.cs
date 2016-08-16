using UnityEngine;
using System.Collections;
using System.IO;
using System.Collections.Generic;

public class PathConfig
{

    #region 全局
    //插件统一文件夹名
    public static string PluginsFolderName = "Plugins";

    //ResetCore根目录
    public static string ResetCorePath = Application.dataPath + "/ResetCore/";
    public static string ResetCoreTempPath = Path.Combine(projectPath, "ResetCoreTemp");
    //ResetCore备份根目录
    public static string ResetCoreBackUpPath = Path.Combine(ResetCoreTempPath, "Backup");

    //Extra工具包内目录
    public static string ExtraToolPathInPackage = Path.Combine(ResetCorePath, "ExtraTool.zip");
    //Extra工具根目录
    public static string ExtraToolPath = Path.Combine(ResetCoreTempPath, "ExtraTool");

    //SDK工具包内目录
    public static string SDKPathInPackage = Path.Combine(ResetCorePath, "SDK.zip");
    //SDK工具备份目录
    public static string SDKBackupPath = Path.Combine(ResetCoreTempPath, "SDK");
    //SDK工具安装目录
    public static string SDKPath = Path.Combine(Application.dataPath, "SDK");

    //Plugins目录
    public static readonly string pluginPath = Path.Combine(Application.dataPath, PluginsFolderName); 

    //工程目录
    public static string projectPath
    {
        get
        {
            DirectoryInfo directory = new DirectoryInfo(Application.dataPath);
            return directory.Parent.FullName;
        }
    }
    public static readonly string assetResourcePath = "Assets/Resources/";

    #endregion

    #region AssetBundle 相关
    //AssetBundleManifest的文件名
    public static readonly string assetBundleManifest_FileName = "AssetBundleManifest";
    //AssetBundleManifest的扩展名
    public static readonly string assetBundleManifest_ExName = ".manifest";
    //Resources路径
    public static readonly string resourcePath = Application.dataPath + "/Resources/";
    //Bundle根目录
    public static readonly string resourceBundlePath = "Data/BundleData/";
    //资源列表目录
    public static readonly string resourceListDocPath = "Data/BundleData/ResourcesList";
    //场景记录文件储存目录
    public static readonly string sceneXmlRootPath = "Data/BundleData/SceneData/";
    //预置组件信息文件储存目录
    public static readonly string compInfoObjRootPath = "Data/BundleData/PrefabCompData/";
    //相对于Assets到Resources文件夹的相对路径
    public static readonly string assetsToResources = "Asset/Resources";

    //资源下载根目录
    public static readonly string wwwPath = "http://127.0.0.1/Test";

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

    //游戏数据根目录
    public static readonly string localGameDataSourceRoot = resourcePath + "Data/GameData/";
    //游戏数据类文件根目录
    public static readonly string localGameDataClassRoot = ResetCorePath + "Core/GameDatas/DataClasses/";

    //存放Xml的地址
    public static readonly string localGameDataXmlPath = localGameDataSourceRoot + "Xml/";
    //存放XmlGameData类的地址
    public static readonly string localXmlGameDataClassPath = localGameDataClassRoot + "Xml/";

    //存放Obj的地址
    public static readonly string localGameDataObjPath = localGameDataSourceRoot + "Obj/";
    //存放ObjGameData类的地址
    public static readonly string localObjGameDataClassPath = localGameDataClassRoot + "Obj/";

    //存放Protobuf的地址
    public static readonly string localGameDataProtobufPath = localGameDataSourceRoot + "Protobuf/";
    //存放ProtobufGameData类的地址
    public static readonly string localProtobufGameDataClassPath = localGameDataClassRoot + "Protobuf/";

    //存放PrefData的地址
    public static readonly string localPrefDataPath = localGameDataSourceRoot + "Pref/";
    //存放PrefData GameData类的地址
    public static readonly string localPrefDataClassPath = localGameDataClassRoot + "Pref/";

    //存放核心数据备份的地址
    public static readonly string localCoreDataBackupPath = ResetCorePath + "Core/GameDatas/CoreData/Datas/";
    //存放核心数据的地址
    public static readonly string localCoreDataPath = localGameDataSourceRoot + "Core/";
    //存放核心数据 GameData类的地址
    public static readonly string localCoreDataClassPath = ResetCorePath + "Core/GameDatas/CoreData/Classes/";

    #endregion

    #region NetPost
    //服务器响应url
    public static readonly string NetPostURL = "127.0.0.1:8000";
    #endregion

    #region Lua
    //Lua储存路径
    public static readonly string localLuaDataXmlPath = resourcePath + "Data/Lua/";
    public static readonly string localModLuaFilePath = Application.persistentDataPath + "/Mod/Lua/";
    #endregion

    #region 工具
    //Lua模板资源路径
    public static readonly string luaScriptAssetPath = ResetCorePath + "Lua/Editor/LuaAsset.lua".Replace(projectPath, "");
    //Xml模板资源路径
    public static readonly string xmlScriptAssetPath = ResetCorePath + "Xml/Editor/XmlAsset.xml".Replace(projectPath, "");

    public static readonly string csToolPath = ExtraToolPath + "C#Tools/ExcelDataManager.exe";
    public static readonly string csTool_GameDataViaExcel = "GameDataGen";
    #endregion
}
