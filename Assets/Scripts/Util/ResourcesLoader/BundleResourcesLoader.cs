using UnityEngine;
using System.Collections;
using System.IO;
using System.Collections.Generic;

public class BundleResourcesLoader : IResourcesLoad {

    private ResourcesLoaderHelper LoadHelper;
    public ResourcesLoaderHelper loadHelper { get { return LoadHelper; } }

    //键为Bundle名，值为Bundle
    public Dictionary<string, AssetInfo> assetInfoList { get; private set; }

    public BundleResourcesLoader(ResourcesLoaderHelper helper)
    {
        LoadHelper = helper;
        assetInfoList = new Dictionary<string, AssetInfo>();
    }

    Object IResourcesLoad.LoadResource(string objectName, System.Action<Object> afterLoadAct = null)
    {

        string bundleName = ResourcesLoaderHelper.GetResourcesBundleNameByObjectName(objectName);
        AssetInfo assetInfo = LoadFromFileOrCache(bundleName);
        AssetBundle assetBundle = assetInfo.assetbundle;

        Object obj = null;
        if (assetBundle != null)
        {

            string[] dependenciesNames = loadHelper.manifest.GetAllDependencies(bundleName);

            foreach (string depBundleName in dependenciesNames)
            {
                Debug.logger.Log("depName" + depBundleName);
                LoadFromFileOrCache(depBundleName);
            }

            obj = assetBundle.LoadAsset(objectName);
        }

        if (afterLoadAct != null)
            afterLoadAct(obj);

        if (obj == null) Debug.logger.LogError("加载错误", "加载失败！");

        return obj;

        
    }

    Object[] IResourcesLoad.LoadResources(string[] objectsNames, System.Action<Object[]> afterLoadAct = null)
    {
        Object[] objs = new Object[objectsNames.Length];
        for (int i = 0; i < objs.Length; i++)
        {
            objs[i] = ResourcesLoaderHelper.Instance.loader.LoadResource(objectsNames[i]);
        }

        if (afterLoadAct != null)
            afterLoadAct(objs);

        return objs;
    }

    GameObject IResourcesLoad.LoadAndGetInstance(string objectName, System.Action<GameObject> afterLoadAct = null)
    {
        Object obj = ResourcesLoaderHelper.Instance.loader.LoadResource(objectName);
        GameObject go = GameObject.Instantiate(obj) as GameObject;

        if (afterLoadAct != null)
            afterLoadAct(go);

        return go;
    }

    private AssetInfo LoadFromFileOrCache(string bundleName)
    {
        if (assetInfoList.ContainsKey(bundleName))
        {
            return assetInfoList[bundleName];
        }
        else
        {
            AssetInfo newInfo = new AssetInfo(bundleName);
            assetInfoList.Add(bundleName, newInfo);
            return newInfo;
        }
    }
}
