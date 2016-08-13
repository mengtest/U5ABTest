using UnityEngine;
using System.Collections;
using System;
using System.IO;

namespace ResetCore.Asset
{
    public class ABResInfo
    {

        public string url { get; private set; }
        //被调用的时间
        public DateTime getTimeLastTime { get; private set; }
        public string assetBundleName { get; private set; }

        private AssetBundle Bundle;
        public AssetBundle assetbundle
        {
            get
            {
                getTimeLastTime = DateTime.Now;
                return Bundle;
            }
            private set
            {
                Bundle = value;
            }
        }
        public string[] dependenciesNames { get; private set; }


        public ABResInfo(string bundleName)
        {
            url = ResourcesLoaderHelper.GetBundlePathByBundleName(bundleName);
            getTimeLastTime = DateTime.Now;
            assetBundleName = bundleName;
            dependenciesNames = ResourcesLoaderHelper.Instance.manifest.GetAllDependencies(bundleName);
            string path = ResourcesLoaderHelper.GetBundlePathByBundleName(bundleName);

            if (File.Exists(path))
            {
                assetbundle = AssetBundle.LoadFromFile(path);
            }
            else
            {
                Debug.logger.LogError("LocalResInfo", path + "中的文件不存在！");
            }
                
        }

        public void Unload()
        {
            assetbundle.Unload(false);
        }
    }

}

