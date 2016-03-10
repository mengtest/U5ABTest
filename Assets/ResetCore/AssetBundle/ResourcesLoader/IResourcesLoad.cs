using UnityEngine;
using System.Collections;


namespace ResetCore.Asset
{
    public interface IResourcesLoad
    {

        ResourcesLoaderHelper loadHelper { get; }

        //加载资源
        Object LoadResource(string objectName, System.Action<Object> afterLoadAct = null);
        //加载资源们
        Object[] LoadResources(string[] objectName, System.Action<Object[]> afterLoadAct = null);
        //加载资源并实例化
        GameObject LoadAndGetInstance(string objectName, System.Action<GameObject> afterLoadAct = null);

    }

}
