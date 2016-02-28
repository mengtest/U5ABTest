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

    Object IResourcesLoad.LoadResource(string objectName, System.Action afterLoadAct = null, System.Action<float> progressAct = null)
    {
        string url = Path.Combine(PathConfig.bundlePath, ResourcesLoaderHelper.instance.resourcesList[objectName].Replace("\\", "/"));
        WWW www = new WWW(url);
        
        throw new System.NotImplementedException();
    }

    Object[] IResourcesLoad.LoadResources(string[] objectsName, System.Action afterLoadAct = null, System.Action<float> progressAct = null)
    {
        throw new System.NotImplementedException();
    }

    GameObject IResourcesLoad.LoadAndGetInstance(string objectName, System.Action afterLoadAct = null, System.Action<float> progressAct = null)
    {
        throw new System.NotImplementedException();
    }
}
