using UnityEngine;
using System.Collections;
using UnityEditor;
using System.Collections.Generic;
using ResetCore.Asset;
using System.Xml.Linq;
using System.Reflection;

public class HandleCompOnAllPrefab {

    private static Dictionary<string, string> ResourcesList;

    [MenuItem("Tools/生成预置组件信息")]
    public static void GenCompInfoXml()
    {
        ResourcesList = ResourcesLoaderHelper.LoadResourcesListFile();
        int num = 0;
        foreach (KeyValuePair<string, string> pair in ResourcesList)
        {
            EditorUtility.DisplayProgressBar("导出信息中", "请耐心等候", (float)num / (float)ResourcesList.Count);
            if(!pair.Key.EndsWith(".prefab")) continue;
            string path = pair.Value;
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

        XDocument xDoc = new XDocument();
        XElement root = new XElement("Root");
        xDoc.Add(root);

        root.SetAttributeValue("Name", go.name + ".prefab");
        foreach (Component comp in comps)
        {
            System.Type compType = comp.GetType();
            XElement compEl = new XElement(compType.Name);
            FieldInfo[] fieldInfos = compType.GetFields();
            PropertyInfo[] propInfos = compType.GetProperties();
            Debug.logger.Log(compType.Name + " " + propInfos.Length);
            foreach (PropertyInfo propInfo in propInfos)
            {
                if ((propInfo.PropertyType.IsPublic == true && propInfo.CanWrite && propInfo.CanRead))
                {
                    XElement fieldEl = new XElement(propInfo.Name);
                    object value = propInfo.GetValue(comp, null);
                    if(value != null){
                        fieldEl.SetValue(value);
                    }
                    
                    compEl.Add(fieldEl);
                }
            }
            root.Add(compEl);
        }
        Debug.logger.Log(PathConfig.resourcePath + PathConfig.compInfoXmlRootPath + go.name + ".xml");
        xDoc.Save(PathConfig.resourcePath + PathConfig.compInfoXmlRootPath + go.name + ".xml");
        AssetDatabase.Refresh();
    }
}
