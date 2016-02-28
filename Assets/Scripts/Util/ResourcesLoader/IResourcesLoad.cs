using UnityEngine;
using System.Collections;
using System;

interface IResourcesLoad {

    ResourcesLoaderHelper loadHelper { get; }

    //加载资源
    UnityEngine.Object LoadResource(string objectName, Action afterLoadAct, Action<int> progressAct = null);
    //加载资源们
    UnityEngine.Object[] LoadResources(string[] objectName, Action afterLoadAct, Action<int> progressAct = null);
    //加载资源并实例化
    UnityEngine.Object LoadAndGetInstance(string[] objectName, Action afterLoadAct, Action<int> progressAct = null);

}
