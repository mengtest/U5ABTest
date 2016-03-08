using UnityEngine;
using System.Collections;

public class AssetInfo {

    public string url { get; private set; }
    public string assetBundleName { get; private set; }
    public AssetBundle assetbundle { get; private set; }

    public AssetInfo(string bundleName)
    {
        url = ResourcesLoaderHelper.GetBundlePathByBundleName(bundleName);
        assetBundleName = bundleName;
        assetbundle = AssetBundle.LoadFromFile(ResourcesLoaderHelper.GetBundlePathByBundleName(bundleName));
    }
}
