using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using ResetCore.Asset;
using System.Reflection;

public class ComponentLoader : MonoBehaviour {

    void Awake()
    {
        string assetName = gameObject.name + ComponentInfoObject.ExName;
        if (ResourcesLoaderHelper.resourcesList.ContainsKey(assetName))
        {
            ComponentInfoObject compInfoObj = ResourcesLoaderHelper.Instance.LoadResource(assetName) as ComponentInfoObject;
            foreach (Component comp in compInfoObj.componentGroup)
            {
                Debug.Log(comp.GetType().Name);
                System.Type compType = comp.GetType();
                Component compOnGo = gameObject.GetComponent(compType.Name);
                if (compOnGo != null)
                {
                    compOnGo = comp;
                }
                else
                {
                    System.Type goType = gameObject.GetType();
                    MethodInfo addCompMethod = goType.GetMethod("AddComponent");
                    addCompMethod.MakeGenericMethod(compType);
                    addCompMethod.Invoke(gameObject, null);
                    //compOnGo = comp;
                }
            }
        }
        
    }

    private void GetComponentType(string typeName)
    {

    }
}
