using UnityEngine;
using System.Collections;

public interface IResourcesLoad {

    ResourcesLoaderHelper loadHelper { get; }

    //加载资源
    void LoadResource(string objectName, System.Action<Object> afterLoadAct = null, System.Action<float> progressAct = null);
    //加载资源们
    void LoadResources(string[] objectName, System.Action<Object[]> afterLoadAct = null, System.Action<float> progressAct = null);
    //加载资源并实例化
    void LoadAndGetInstance(string objectName, System.Action<GameObject> afterLoadAct = null, System.Action<float> progressAct = null);

}
