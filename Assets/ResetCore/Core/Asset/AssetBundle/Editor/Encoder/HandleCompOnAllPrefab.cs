using UnityEngine;
using System.Collections;
using UnityEditor;
using System.Collections.Generic;
using ResetCore.Asset;
using System.Xml.Linq;
using System.Reflection;
using System.IO;

public class HandleCompOnAllPrefab {

    private static Dictionary<string, string> ResourcesList;
    private static readonly string ComponentInfoObjectRootPath = PathConfig.assetResourcePath + PathConfig.compInfoObjRootPath;

    [MenuItem("Tools/Assets/Gen Prefab Info")]
    public static void GenCompInfoXml()
    {
        ResourcesList = ResourcesLoaderHelper.LoadResourcesListFile();
        int num = 0;
        foreach (KeyValuePair<string, string> pair in ResourcesList)
        {
            EditorUtility.DisplayProgressBar("导出信息中", "请耐心等候", (float)num / (float)ResourcesList.Count);
            if(!pair.Key.EndsWith(".prefab")) continue;
            GameObject go = AssetDatabase.LoadAssetAtPath(pair.Value + ".prefab", typeof(GameObject)) as GameObject;
            //GameObject go = GameObject.Instantiate(obj) as GameObject;
            ReadPrefab(go);
            //Editor.DestroyImmediate(go);
        }
        EditorUtility.ClearProgressBar();
    }

    private static void ReadPrefab(GameObject go)
    {
        //
        if (go.GetComponent<ComponentLoader>() != null)
        {
            Debug.logger.Log(go.name + "已经是一个处理过的prefab");
            return;
        }

        Component[] comps = go.GetComponents<Component>();
        
        ComponentInfoObject compDataObj = ScriptableObject.CreateInstance<ComponentInfoObject>();
        compDataObj.componentGroup = new List<Component>(comps);

        foreach (Component comp in compDataObj.componentGroup)
        {
            System.Type compType = comp.GetType();
            Debug.LogError(compType.Name);

            foreach (FieldInfo info in compType.GetFields())
            {
                if (info.IsPublic && !info.IsStatic)
                {
                    Debug.Log("Field " + info.Name);
                    ///TODO写入域信息
                }
                
            }

            foreach (PropertyInfo info in compType.GetProperties())
            {
                if (info.GetGetMethod() != null && info.GetSetMethod() != null &&
                    info.GetGetMethod().IsPublic && info.GetSetMethod().IsPublic &&
                    info.CanRead && info.CanWrite)
                {
                    Debug.Log("Property " + info.Name);
                    ///TODO写入属性信息
                }

            }
            
        }

        string path = ComponentInfoObjectRootPath + go.name + ComponentInfoObject.ExName;

        if (!Directory.Exists(ComponentInfoObjectRootPath))
        {
            Directory.CreateDirectory(ComponentInfoObjectRootPath);
        }

        AssetDatabase.CreateAsset(compDataObj, path);
        
    }
}
