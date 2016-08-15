using UnityEngine;
using System.Collections;


namespace ResetCore.Asset
{
    public interface IResourcesLoad
    {

        ResourcesLoaderHelper loadHelper { get; }

        //加载资源
        Object LoadResource(string objectName, System.Action<Object> afterLoadAct = null);

    }

}
