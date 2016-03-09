using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Xml.Linq;

public class ResourcesLoaderHelper : MonoSingleton<ResourcesLoaderHelper>{

    //键为物体名，值为相对于Resources路径
    public Dictionary<string, string> resourcesList { get; private set; }
    public static readonly string ExName = ".ab"; 

    //根AssetBundle文件
    private AssetBundle MainBundle;
    public AssetBundle mainBundle
    {
        get
        {
            if (MainBundle == null)
            {
                MainBundle = AssetBundle.LoadFromFile(PathConfig.AssetRootBundlePath);
            }
            return MainBundle;
        }
    }
    //AssetBundle主配置文件
    private AssetBundleManifest Manifest;
    public AssetBundleManifest manifest
    {
        get
        {
            if (Manifest == null)
            {
                Manifest = (AssetBundleManifest)mainBundle.LoadAsset("AssetBundleManifest");
            }
            return Manifest;
        }
    }

    private LocalResourcesLoader localLoader;
    private BundleResourcesLoader bundleLoader;

    public override void Init()
    {
        base.Init();
        localLoader = new LocalResourcesLoader(this);
        bundleLoader = new BundleResourcesLoader(this);
        LoadResourcesListFile();
    }

    /// <summary>
    /// 加载资源
    /// </summary>
    /// <param name="objectName"></param>
    /// <param name="afterLoadAct"></param>
    /// <returns></returns>
    public Object LoadResource(string objectName, System.Action<Object> afterLoadAct = null)
    {
        Object obj = bundleLoader.LoadResource(objectName, afterLoadAct);
        if (obj == null)
        {
            obj = localLoader.LoadResource(objectName, afterLoadAct);
        }
            
        return obj;
    }
    /// <summary>
    /// 加载多个资源
    /// </summary>
    /// <param name="objectNames"></param>
    /// <param name="afterLoadAct"></param>
    /// <returns></returns>
    public Object[] LoadResources(string[] objectNames, System.Action<Object[]> afterLoadAct = null)
    {
        Object[] objs = bundleLoader.LoadResources(objectNames, afterLoadAct);
        if (objs == null)
        {
            objs = localLoader.LoadResources(objectNames, afterLoadAct);
        }
        return objs;
    }
    /// <summary>
    /// 加载游戏资源
    /// </summary>
    /// <param name="objectName"></param>
    /// <param name="afterLoadAct"></param>
    /// <returns></returns>
    public GameObject LoadAndGetInstance(string objectName, System.Action<GameObject> afterLoadAct = null)
    {
        GameObject go = bundleLoader.LoadAndGetInstance(objectName, afterLoadAct);
        if (go == null)
        {
            go = localLoader.LoadAndGetInstance(objectName, afterLoadAct);
        }
        return go;
    }


    //加载资源列表
    private void LoadResourcesListFile()
    {
        resourcesList = new Dictionary<string, string>();
        XDocument resourcesListDoc = XDocument.Load(PathConfig.resourceListDocPath);
        int i = 1;
        string name = "";
        string path = "";
        foreach (XElement el in resourcesListDoc.Element("Root").Elements())
        {
            if (i % 2 == 1)
            {
                name = el.Value;
                //Debug.Log("Name:" + name);
            }
            else
            {
                path = el.Value;
                resourcesList.Add(name, path);
                //Debug.Log("Path:" + path);
            }
            i++;
        }
    }

    /// <summary>
    /// 获取Resources文件夹下资源所生成的AssetBundle的路径（即可以直接加载的物体）
    /// </summary>
    /// <param name="objName">物体名称：如“Cube.prefab”</param>
    /// <returns></returns>
    public static string GetResourcesBundlePathByObjectName(string objName)
    {
        return PathConfig.bundleRootPath + "/assets/resources/" + ResourcesLoaderHelper.Instance.resourcesList[objName];
    }

    /// <summary>
    /// 获取Resources文件夹下资源所生成的AssetBundle的名称
    /// </summary>
    /// <param name="objName"></param>
    /// <returns></returns>
    public static string GetResourcesBundleNameByObjectName(string objName)
    {
        return ("assets/resources/" + ResourcesLoaderHelper.Instance.resourcesList[objName]).ToLower() + ExName;
    }

    /// <summary>
    /// 通过bundle名来获取相应的bundle路径
    /// </summary>
    /// <param name="bundleName">bundle名称 如“assets/resources/prefabs/cube”</param>
    /// <returns></returns>
    public static string GetBundlePathByBundleName(string bundleName)
    {
        return PathConfig.bundleRootPath + "/" + bundleName;
    }


}
