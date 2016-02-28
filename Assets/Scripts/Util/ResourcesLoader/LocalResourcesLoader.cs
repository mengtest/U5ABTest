using UnityEngine;
using System.Collections;

public class LocalResourcesLoader : IResourcesLoad {

    private ResourcesLoaderHelper LoadHelper;
    private ResourcesLoaderHelper loadHelper { get { return LoadHelper; } }

    public LocalResourcesLoader(ResourcesLoaderHelper helper)
    {
        LoadHelper = helper;
    }

    public Object IResourcesLoad.LoadResource(string objectName, System.Action afterLoadAct, System.Action<int> progressAct = null)
    {
        throw new System.NotImplementedException();
        
    }

    public Object[] IResourcesLoad.LoadResources(string[] objectName, System.Action afterLoadAct, System.Action<int> progressAct = null)
    {
        throw new System.NotImplementedException();
    }

    public Object IResourcesLoad.LoadAndGetInstance(string[] objectName, System.Action afterLoadAct, System.Action<int> progressAct = null)
    {
        throw new System.NotImplementedException();
    }
}
