using UnityEngine;
using System.Collections;
using System;

public interface IResourcesLoad {

    ResourcesLoaderHelper loadHelper { get; }

    //加载资源
    UnityEngine.Object LoadResource(string objectName, Action afterLoadAct = null, Action<float> progressAct = null);
    //加载资源们
    UnityEngine.Object[] LoadResources(string[] objectName, Action afterLoadAct = null, Action<float> progressAct = null);
    //加载资源并实例化
    UnityEngine.GameObject LoadAndGetInstance(string objectName, Action afterLoadAct = null, Action<float> progressAct = null);

}
