using UnityEngine;
using System.Collections;

public class BundleResourcesLoader : IResourcesLoad {


    Object IResourcesLoad.LoadResource(string objectName, System.Action afterLoadAct, System.Action<int> progressAct)
    {
        throw new System.NotImplementedException();
    }

    Object[] IResourcesLoad.LoadResources(string[] objectName, System.Action afterLoadAct, System.Action<int> progressAct)
    {
        throw new System.NotImplementedException();
    }

    Object IResourcesLoad.LoadAndGetInstance(string[] objectName, System.Action afterLoadAct, System.Action<int> progressAct)
    {
        throw new System.NotImplementedException();
    }
}
