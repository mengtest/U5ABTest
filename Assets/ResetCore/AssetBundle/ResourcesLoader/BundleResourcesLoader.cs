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
        public Dictionary<string, ABResInfo> assetInfoList { get; private set; }

        public BundleResourcesLoader(ResourcesLoaderHelper helper)
        {
            LoadHelper = helper;
            assetInfoList = new Dictionary<string, ABResInfo>();
        }

        public Object LoadResource(string objectName, System.Action<Object> afterLoadAct = null)
        {

            string bundleName = ResourcesLoaderHelper.GetResourcesBundleNameByObjectName(objectName);
            ABResInfo assetInfo = LoadFromFileOrCache(bundleName);
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
            else
            {
                Debug.logger.LogError("加载Bundle", "Bundle为空");
            }

            if (afterLoadAct != null)
                afterLoadAct(obj);

            if (obj == null) Debug.logger.LogError("加载错误", "加载失败！");

            return obj;


        }

        private ABResInfo LoadFromFileOrCache(string bundleName)
        {
            if (assetInfoList.ContainsKey(bundleName))
            {
                return assetInfoList[bundleName];
            }
            else
            {
                ABResInfo newInfo = new ABResInfo(bundleName);
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
                    assetInfoList[item.Key].Unload();
                    assetInfoList.Remove(item.Key);
                }
            }
        }

        //释放所有Bundle
        private void UnloadAll()
        {
            foreach (KeyValuePair<string, ABResInfo> assetPair in assetInfoList)
            {
                assetPair.Value.Unload();
                assetInfoList.Remove(assetPair.Key);
            }
        }
    }

}
