using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace ResetCore.Asset
{
    public class LocalResourcesLoader : IResourcesLoad
    {

        private ResourcesLoaderHelper LoadHelper;
        public ResourcesLoaderHelper loadHelper { get { return LoadHelper; } }

        public static Dictionary<string, LocalResInfo> assetInfoList { get; private set; }

        public LocalResourcesLoader(ResourcesLoaderHelper helper)
        {
            assetInfoList = new Dictionary<string, LocalResInfo>();
            LoadHelper = helper;
        }

        public Object LoadResource(string objectName, System.Action<Object> afterLoadAct = null)
        {
            LocalResInfo resInfo = LoadFromFileOrCache(objectName);
            Object obj = resInfo.localRes;

            if (afterLoadAct != null)
                afterLoadAct(obj);

            return obj;
        }

        private LocalResInfo LoadFromFileOrCache(string prefabName)
        {
            if (assetInfoList.ContainsKey(prefabName))
            {
                return assetInfoList[prefabName];
            }
            else
            {
                LocalResInfo newInfo = new LocalResInfo(prefabName);
                assetInfoList.Add(prefabName, newInfo);
                UnloadAsset();
                return newInfo;
            }
        }
        //卸载资源
        private void UnloadAsset()
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
                    if (assetInfoList[item.Key].localResName.EndsWith(".prefab"))
                    {
                        assetInfoList[item.Key].Unload();
                        assetInfoList.Remove(item.Key);
                    }
                }
            }
        }
        //卸载全部资源
        private void UnloadAll()
        {
            foreach (KeyValuePair<string, LocalResInfo> infoPair in assetInfoList)
            {
                infoPair.Value.Unload();
                assetInfoList.Remove(infoPair.Key);
            }
        }
    }

}
