using UnityEngine;
using System.Collections;

public class LocalResourcesLoader : IResourcesLoad {

    private ResourcesLoaderHelper LoadHelper;
    public ResourcesLoaderHelper loadHelper { get { return LoadHelper; } }

    public LocalResourcesLoader(ResourcesLoaderHelper helper)
    {
        LoadHelper = helper;
    }

    void IResourcesLoad.LoadResource(string objectName, System.Action<Object> afterLoadAct = null, System.Action<float> progressAct = null)
    {
        if (progressAct != null)
            progressAct(0);

        Object obj = Resources.Load(loadHelper.resourcesList[objectName]);

        if(progressAct != null)
            progressAct(1);

        if (afterLoadAct != null)
            afterLoadAct(obj);
        
    }

    void IResourcesLoad.LoadResources(string[] objectsName, System.Action<Object[]> afterLoadAct = null, System.Action<float> progressAct = null)
    {
        if (progressAct != null)
            progressAct(0);

        int count = objectsName.Length;
        Object[] objs = new Object[count];
        for (int i = 0; i < count; i ++)
        {
            objs[i] = Resources.Load(objectsName[i]);

            if (progressAct != null)
                progressAct((float)i/(float)count);

        }
        if (progressAct != null)
            progressAct(1);

        if (afterLoadAct != null)
            afterLoadAct(objs);
    }

    void IResourcesLoad.LoadAndGetInstance(string objectName, System.Action<GameObject> afterLoadAct = null, System.Action<float> progressAct = null)
    {
        if (progressAct != null)
            progressAct(0);

        Object obj = Resources.Load(loadHelper.resourcesList[objectName]);
        GameObject go = GameObject.Instantiate(obj) as GameObject;

        if (progressAct != null)
            progressAct(1);

        if (afterLoadAct != null)
            afterLoadAct(go);
    }

   
}
