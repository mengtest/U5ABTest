using UnityEngine;
using System.Collections;


namespace ResetCore.Asset
{
    public class LocalResourcesLoader : IResourcesLoad
    {

        private ResourcesLoaderHelper LoadHelper;
        public ResourcesLoaderHelper loadHelper { get { return LoadHelper; } }

        public LocalResourcesLoader(ResourcesLoaderHelper helper)
        {
            LoadHelper = helper;
        }

        public Object LoadResource(string objectName, System.Action<Object> afterLoadAct = null)
        {

            Object obj = Resources.Load(ResourcesLoaderHelper.resourcesList[objectName].Replace("Asset/Resources", ""));

            if (afterLoadAct != null)
                afterLoadAct(obj);

            return obj;
        }

        public Object[] LoadResources(string[] objectsName, System.Action<Object[]> afterLoadAct = null)
        {

            int count = objectsName.Length;
            Object[] objs = new Object[count];
            for (int i = 0; i < count; i++)
            {
                objs[i] = Resources.Load(objectsName[i]);


            }

            if (afterLoadAct != null)
                afterLoadAct(objs);
            return objs;
        }

        public GameObject LoadAndGetInstance(string objectName, System.Action<GameObject> afterLoadAct = null)
        {

            Object obj = Resources.Load(ResourcesLoaderHelper.resourcesList[objectName].Replace("Asset/Resources", ""));
            GameObject go = GameObject.Instantiate(obj) as GameObject;


            if (afterLoadAct != null)
                afterLoadAct(go);

            return go;
        }

    }

}
