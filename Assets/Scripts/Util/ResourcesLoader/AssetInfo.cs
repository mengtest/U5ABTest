using UnityEngine;
using System.Collections;
using System;

public class AssetInfo {

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


    public AssetInfo(string bundleName)
    {
        url = ResourcesLoaderHelper.GetBundlePathByBundleName(bundleName);
        getTimeLastTime = DateTime.Now;
        assetBundleName = bundleName;
        dependenciesNames = ResourcesLoaderHelper.Instance.manifest.GetAllDependencies(bundleName);
        assetbundle = AssetBundle.LoadFromFile(ResourcesLoaderHelper.GetBundlePathByBundleName(bundleName));
    }
}
