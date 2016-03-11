using UnityEngine;
using System.Collections;
using System.IO;
using System.Collections.Generic;
using System.Linq;

namespace ResetCore.Asset
{
    public class BundleResourcesLoader : IResourcesLoad
    {

        private ResourcesLoaderHelper LoadHelper;
        public ResourcesLoaderHelper loadHelper { get { return LoadHelper; } }

        //键为Bundle名，值为Bundle
        public Dictionary<string, AssetInfo> assetInfoList { get; private set; }

        public BundleResourcesLoader(ResourcesLoaderHelper helper)
        {
            LoadHelper = helper;
            assetInfoList = new Dictionary<string, AssetInfo>();
        }

        public Object LoadResource(string objectName, System.Action<Object> afterLoadAct = null)
        {

            string bundleName = ResourcesLoaderHelper.GetResourcesBundleNameByObjectName(objectName);
            AssetInfo assetInfo = LoadFromFileOrCache(bundleName);
            AssetBundle assetBundle = assetInfo.assetbundle;

            Object obj = null;
            if (assetBundle != null)
            {

                foreach (string depBundleName in assetInfo.dependenciesNames)
                {
                    LoadFromFileOrCache(depBundleName);
                }

                obj = assetBundle.LoadAsset(objectName);
            }

            if (afterLoadAct != null)
                afterLoadAct(obj);

            if (obj == null) Debug.logger.LogError("加载错误", "加载失败！");

            return obj;


        }

        public Object[] LoadResources(string[] objectsNames, System.Action<Object[]> afterLoadAct = null)
        {
            Object[] objs = new Object[objectsNames.Length];
            for (int i = 0; i < objs.Length; i++)
            {
                objs[i] = ResourcesLoaderHelper.Instance.LoadResource(objectsNames[i]);
            }

            if (afterLoadAct != null)
                afterLoadAct(objs);

            return objs;
        }

        public GameObject LoadAndGetInstance(string objectName, System.Action<GameObject> afterLoadAct = null)
        {
            Object obj = ResourcesLoaderHelper.Instance.LoadResource(objectName);
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
                UnloadBundles();
                return newInfo;
            }
        }

        //释放Bundle
        private void UnloadBundles()
        {
            if (assetInfoList.Count > 50)
            {
                //上次调用时间超过1分钟的全部卸载
                var unloadResources = from list in assetInfoList
                                      orderby list.Value.getTimeLastTime
                                      where (System.DateTime.Now - list.Value.getTimeLastTime).Minutes > 1
                                      select new { Key = list.Key };
                foreach (var item in unloadResources)
                {
                    assetInfoList[item.Key].assetbundle.Unload(false);
                    assetInfoList.Remove(item.Key);
                }
            }
        }
    }

}
