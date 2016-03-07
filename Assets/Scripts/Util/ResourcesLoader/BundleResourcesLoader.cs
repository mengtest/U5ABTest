using UnityEngine;
using System.Collections;
using System.IO;

public class BundleResourcesLoader : IResourcesLoad {

    private ResourcesLoaderHelper LoadHelper;
    public ResourcesLoaderHelper loadHelper { get { return LoadHelper; } }

    public BundleResourcesLoader(ResourcesLoaderHelper helper)
    {
        LoadHelper = helper;
    }

    void IResourcesLoad.LoadResource(string objectName, System.Action<Object> afterLoadAct = null, System.Action<float> progressAct = null)
    {
        //string url = Path.Combine(PathConfig.bundlePath, ResourcesLoaderHelper.instance.resourcesList[objectName].Replace("\\", "/"));
        //loadHelper.LoadWWWAsset(url, afterLoadAct, progressAct);
        //Driver.instance.StartCoroutine(loadHelper.LoadWWWAsset(url, afterLoadAct, progressAct));
        string filePath = PathConfig.bundlePath + loadHelper.resourcesList[objectName];
        AssetBundle manifestBundle = AssetBundle.LoadFromFile(filePath);
        
        //if (manifestBundle != null)
        //{
        //    AssetBundleManifest manifest = (AssetBundleManifest)manifestBundle.LoadAsset(filePath + ".manifest");
        //    string[] manifest.GetAllDependencies(manifestBundle.name);
        //}
    }

    void IResourcesLoad.LoadResources(string[] objectsNames, System.Action<Object[]> afterLoadAct = null, System.Action<float> progressAct = null)
    {
        throw new System.NotImplementedException();
    }

    void IResourcesLoad.LoadAndGetInstance(string objectName, System.Action<GameObject> afterLoadAct = null, System.Action<float> progressAct = null)
    {
        throw new System.NotImplementedException();
    }
}
