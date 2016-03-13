using UnityEngine;
using System.Collections;
using System.Collections.Generic;

using System;
using System.Text;
using System.Linq;
using System.IO;
using System.Security;

using UnityEditor;
using System.Xml;
using System.Xml.Linq;
using System.Reflection;
using UnityEngine.SceneManagement;
using UnityEditor.SceneManagement;
public class SceneXmlBuilder : MonoBehaviour
{
    //最深搜索Prefab子物体深度
    private const int prefabChildDeepestNum = 3;

    static string scenePath;
    static string sceneName;

    private static readonly string dynamicSceneEx = "Dynamic";
    private static readonly string staticSceneEx = "Static";

    static string dataRoot
    {
        get { return PathConfig.resourcePath + PathConfig.sceneXmlRootPath; }
    }
    //Meta表记录了所有xml表的地址，这个为其路径
    static string dataPathMeta = dataRoot + "DataMeta.xml";

    static SceneXmlBuilder instance = null;

    //记录名字/Resource路径键值对
    private static Dictionary<String, String> filesDic = new Dictionary<string, string>();

    private SceneXmlBuilder() { }


    [MenuItem("Tools/XmlHelper/场景Xml/导出场景Xml")]
    public static void ExportScene()
    {
        //加载路径
        //LoadPathFile();
        WriteXml();
        AssetDatabase.Refresh();
    }

    [MenuItem("Tools/XmlHelper/场景Xml/导出所有选中场景的Xml")]
    public static void ExportAllSelectedScene()
    {
        var selection = Selection.GetFiltered(typeof(UnityEngine.Object), SelectionMode.DeepAssets);
        var paths = (from s in selection
                     let path = AssetDatabase.GetAssetPath(s)
                     where File.Exists(path) && path.EndsWith(".unity")
                     select path).ToArray();

        int num = 0;
        foreach (string item in paths)
        {
            EditorUtility.DisplayCancelableProgressBar("导出中", "这会要一些时间，请耐心等待 " + num + "/" + paths.Length, (float)num / (float)paths.Length);
            if (!SceneManager.GetSceneByPath(item).isLoaded)
                SceneManager.LoadScene(item);
            WriteXml();
            num++;
            Debug.Log("导出场景为" + item);
        }
        EditorUtility.ClearProgressBar();
    }

    private static void WriteXml()
    {
        //获取当前场景完整路径
        scenePath = EditorApplication.currentScene;
        //Debug.logger.Log(scenePath);
        //获取当前场景名称
        sceneName = scenePath.Substring(scenePath.LastIndexOf("/") + 1, scenePath.Length - scenePath.LastIndexOf("/") - 1);
        sceneName = sceneName.Substring(0, sceneName.LastIndexOf("."));

        string dynamicDataPath = dataRoot + dynamicSceneEx + sceneName + ".xml";
        string staticDataPath = dataRoot + staticSceneEx + sceneName + ".xml";
        Debug.logger.Log("导出路径为" + dynamicDataPath + "和" + staticDataPath);




        if (instance == null) instance = new SceneXmlBuilder();

        instance.WriteDynamicData(dynamicDataPath);
        instance.WriteStaticData(staticDataPath);
        //WriteDataMeta(dataPathMeta, dynamicDataPath, staticDataPath);
    }

    //public static void WriteDataMeta(string path, string dynamicDataName, string staticDataName)
    //{
    //    XDocument xmlDoc;
    //    if (!File.Exists(path))
    //    {
    //        xmlDoc = new XDocument();
    //        xmlDoc.Add(new XElement("Root"));
    //        xmlDoc.Save(path);
    //    }
    //    xmlDoc = XDocument.Load(path);

    //    //加载其他关卡
    //    foreach (MapStageData item in MapStageData.dataMap.Values)
    //    {
    //        XElement ele = xmlDoc.Root.Element("el" + item.id);
    //        if (ele != null)
    //        {
    //            ele.Element("DynamicData").Attribute("Name").SetValue(item.sceneName + " DynamicData");
    //            ele.Element("StaticData").Attribute("Name").SetValue(item.sceneName + " StaticData");
    //        }
    //        else
    //        {
    //            ele = new XElement("el" + item.id);
    //            XElement dynamicEle = new XElement("DynamicData");
    //            XElement staticEle = new XElement("StaticData");

    //            dynamicEle.SetAttributeValue("Name", item.sceneName + " DynamicData");
    //            staticEle.SetAttributeValue("Name", item.sceneName + " StaticData");

    //            ele.Add(dynamicEle);
    //            ele.Add(staticEle);

    //            xmlDoc.Root.Add(ele);
    //        }
    //    }
    //    xmlDoc.Save(path);
    //}

    private static Stack<string> currentTree = new Stack<string>();
    //写入动态物体
    public void WriteDynamicData(string path)
    {
        GameObject dynamicDataRoot;
        dynamicDataRoot = GameObject.Find("DynamicData");
        if (dynamicDataRoot == null)
        {
            dynamicDataRoot = new GameObject("DynamicData");
            dynamicDataRoot.transform.position = Vector3.zero;
        }

        XmlDocument xmlDoc = new XmlDocument();
        XmlElement root = xmlDoc.CreateElement("DynamicData");
        root.SetAttribute("Name", sceneName);
        root.SetAttribute("Asset", scenePath);

        currentTree = new Stack<string>();
        WritePrefabObject(xmlDoc, root, dynamicDataRoot);

        //遍历写入各种类的动态物体
        xmlDoc.AppendChild(root);
        //Debug.Log(xmlDoc.ToString());
        xmlDoc.Save(path);
    }
    //写入静态物体
    public void WriteStaticData(string path)
    {
        GameObject staticDataRoot;
        staticDataRoot = GameObject.Find("StaticData");
        if (staticDataRoot == null)
        {
            staticDataRoot = new GameObject("StaticData");
            staticDataRoot.transform.position = Vector3.zero;
        }
            

        XmlDocument xmlDoc = new XmlDocument();
        XmlElement root = xmlDoc.CreateElement("StaticData");
        root.SetAttribute("Name", sceneName);
        root.SetAttribute("Asset", scenePath);

        currentTree = new Stack<string>();
        WritePrefabObject(xmlDoc, root, staticDataRoot);

        xmlDoc.AppendChild(root);
        xmlDoc.Save(path);
    }

    private void WritePrefabObject(XmlDocument xmlDoc, XmlElement parent, GameObject obj)
    {
        if (obj.activeSelf == false) return;
        currentTree.Push(obj.name);
        PrefabType prefabType = PrefabUtility.GetPrefabType(obj);

        XmlElement gameObjectRoot = xmlDoc.CreateElement("GameObject");
        gameObjectRoot.SetAttribute("PrefabType", prefabType.ToString());
        XmlElement propRoot = xmlDoc.CreateElement("Property");



        //当为没有Prefab的时候
        if (prefabType == PrefabType.None)
        {
            gameObjectRoot.SetAttribute("Name", obj.name);
            WriteTransform(xmlDoc, propRoot, obj);
        }
        //有Prefab的时候
        else if (prefabType == PrefabType.PrefabInstance)
        {
            string prefabPath = AssetDatabase.GetAssetPath(PrefabUtility.GetPrefabParent(obj));

            //对Prefab子物体进行特殊处理
            if (PrefabUtility.FindPrefabRoot(obj) != obj)
            {
                gameObjectRoot.SetAttribute("PrefabType", "PrefabChild");
                gameObjectRoot.SetAttribute("Name", obj.name);
            }
            else
            {
                //写入路径
                if (string.IsNullOrEmpty(prefabPath))
                {
                    //Debug.LogError(EditorApplication.currentScene + "中的物体" + obj.name + "丢失预设！！！！");
                    gameObjectRoot.SetAttribute("Name", obj.name + "'s Prefab has Error");
                }
                else
                {
                    gameObjectRoot.SetAttribute("Name", obj.name + ".prefab");
                }
                //写入Transform
                WriteTransform(xmlDoc, propRoot, obj);
            }

            WriteChangedValue(xmlDoc, propRoot, obj);
            //对Prefab中的子物体进行遍历

        }
        else
        {
            string prefabPath = AssetDatabase.GetAssetPath(PrefabUtility.GetPrefabParent(obj));
            if (string.IsNullOrEmpty(prefabPath))
            {
                Debug.LogError(EditorApplication.currentScene + "中的物体" + obj.name + "丢失预设！！！！");
                gameObjectRoot.SetAttribute("Name", obj.name + "'s Prefab has Error");
            }
            else
            {
                gameObjectRoot.SetAttribute("Name", prefabPath);
            }
            Debug.LogError(EditorApplication.currentScene + "中的物体" + obj.name + "的预设类型为" + prefabType.ToString());
        }

        bool willCheckChild = true;
        //继续遍历子物体
        if (gameObjectRoot.GetAttribute("PrefabType") == "PrefabChild")
        {
            GameObject tempObj = obj;
            int deepNum = 0;
            willCheckChild = true;
            while (PrefabUtility.FindPrefabRoot(tempObj) == tempObj)
            {
                tempObj = tempObj.transform.parent.gameObject;
                deepNum++;
                if (deepNum > prefabChildDeepestNum)
                {
                    willCheckChild = false;
                }
            }
        }
        else
        {
            willCheckChild = true;
        }

        if (willCheckChild)
        {
            XmlElement childRoot = xmlDoc.CreateElement("Children");
            for (int i = 0; i < obj.transform.childCount; i++)
            {
                GameObject childObj = obj.transform.GetChild(i).gameObject;
                WritePrefabObject(xmlDoc, childRoot, childObj);
            }
            if (childRoot.HasChildNodes)
                gameObjectRoot.AppendChild(childRoot);
        }



        if (propRoot.HasChildNodes)
            gameObjectRoot.AppendChild(propRoot);
        if (gameObjectRoot.HasChildNodes)
            parent.AppendChild(gameObjectRoot);
        currentTree.Pop();
    }
    //写入位置
    private void WriteTransform(XmlDocument xmlDoc, XmlElement propEle, GameObject obj)
    {
        XmlElement transformRoot = xmlDoc.CreateElement("Transform");

        XmlElement positionRoot = xmlDoc.CreateElement("Position");
        positionRoot.SetAttribute("X", obj.transform.localPosition.x.ToString());
        positionRoot.SetAttribute("Y", obj.transform.localPosition.y.ToString());
        positionRoot.SetAttribute("Z", obj.transform.localPosition.z.ToString());
        transformRoot.AppendChild(positionRoot);

        XmlElement rotationRoot = xmlDoc.CreateElement("Rotation");
        rotationRoot.SetAttribute("X", obj.transform.localEulerAngles.x.ToString());
        rotationRoot.SetAttribute("Y", obj.transform.localEulerAngles.y.ToString());
        rotationRoot.SetAttribute("Z", obj.transform.localEulerAngles.z.ToString());
        transformRoot.AppendChild(rotationRoot);

        XmlElement scaleRoot = xmlDoc.CreateElement("Scale");
        scaleRoot.SetAttribute("X", obj.transform.localScale.x.ToString());
        scaleRoot.SetAttribute("Y", obj.transform.localScale.y.ToString());
        scaleRoot.SetAttribute("Z", obj.transform.localScale.z.ToString());
        transformRoot.AppendChild(scaleRoot);

        propEle.AppendChild(transformRoot);
    }

    private void WriteChangedValue(XmlDocument xmlDoc, XmlElement propEle, GameObject obj)
    {
        string prefabPath = AssetDatabase.GetAssetPath(PrefabUtility.GetPrefabParent(obj));
        GameObject prefabGo = AssetDatabase.LoadAssetAtPath(prefabPath, typeof(GameObject)) as GameObject;

        //当不为Prefab的根物体就要寻找该物体在Prefab中的位置
        //（在这里不允许出现子物体重名！并且要求场景中子物体名与Prefab中的相同！）
        if (PrefabUtility.FindPrefabRoot(obj) != obj)
        {
            //Debug.Log("开始寻找 prefabGo 的相对路径" + obj.name);
            GameObject tempObj = obj;
            List<string> objList = new List<string>();
            while (PrefabUtility.FindPrefabRoot(tempObj) != tempObj)
            {
                objList.Add(tempObj.name);
                //Debug.Log(tempObj.name);
                tempObj = tempObj.transform.parent.gameObject;
            }
            //Debug.Log("prefabGo现在为" + prefabGo.name);
            for (int i = objList.Count - 1; i >= 0; i--)
            {
                string childName = objList[i];
                Transform finedChild = prefabGo.transform.FindChild(childName);
                if (finedChild != null)
                {
                    prefabGo = finedChild.gameObject;
                    //Debug.Log("prefabGo现在为" + prefabGo.name);
                }
                else
                {
                    Debug.LogError("在" + prefabGo.name + "中找不到子物体" + childName);
                    return;
                }

            }
        }


        Component[] prefabComps = prefabGo.GetComponents<Component>();
        Component[] objComps = obj.GetComponents<Component>();

        //检查组件是否相同
        if (prefabComps.Length != objComps.Length)
        {
            Debug.LogError(EditorApplication.currentScene + "中的物体" + obj.name + "的Component与其Prefab上的组件不同！");
            return;
        }
        for (int i = 0; i < prefabComps.Length; i++)
        {
            if (prefabComps.GetType() != objComps.GetType())
            {
                Debug.LogError(EditorApplication.currentScene + "中的物体" + obj.name + "的Component与其Prefab上的组件不同！");
                return;
            }
        }

        //开始检查每个组件中的域
        for (int compNum = 0; compNum < prefabComps.Length; compNum++)
        {
            Component prefabComp = prefabComps[compNum];
            Component objComp = objComps[compNum];

            Type compType = prefabComp.GetType();
            //Debug.Log(obj.name + "组件名为" + compType.Name);

            XmlElement compEle = xmlDoc.CreateElement(compType.Name);

            FieldInfo[] fieldInfos = compType.GetFields();
            for (int infoNum = 0; infoNum < fieldInfos.Length; infoNum++)
            {
                FieldInfo fieldInfo = fieldInfos[infoNum];

                object prefabValue = fieldInfo.GetValue(prefabComp);
                object objValue = fieldInfo.GetValue(objComp);
                //Debug.Log(obj.name + "组件名为 " + compType.Name + " 域名为 " + fieldInfo.Name + " 域类型为 " + fieldInfo.FieldType.Name + " 场景中值为 " + objValue.ToString() + " 预设值为 " + prefabValue.ToString());
                if ((fieldInfo.FieldType.IsPrimitive || fieldInfo.FieldType == typeof(string) || fieldInfo.FieldType == typeof(UnityEngine.Object)) && //为基本类型或者字符类型
                    !fieldInfo.IsStatic && //不能为静态
                    fieldInfo.IsPublic == true &&
                    !prefabValue.Equals(objValue))//为公共类型并且与Prefab不一样了
                {
                    //Debug.Log(obj.name + "组件名为 " + compType.Name + " 域名为 " + fieldInfo.Name +" 域类型为 " + fieldInfo.FieldType.Name + " 场景中值为 " + objValue.ToString() + " 预设值为 " + prefabValue.ToString());
                    compEle.SetAttribute(fieldInfo.Name, objValue.ToString());
                    if (fieldInfo.FieldType == typeof(int))
                    {
                        compEle.SetAttribute(fieldInfo.Name, objValue.ToString());
                    }
                    else if (fieldInfo.FieldType == typeof(float))
                    {
                        compEle.SetAttribute(fieldInfo.Name, objValue.ToString());
                    }
                    else if (fieldInfo.FieldType == typeof(bool))
                    {
                        compEle.SetAttribute(fieldInfo.Name, objValue.ToString());
                    }
                    else if (fieldInfo.FieldType == typeof(double))
                    {
                        compEle.SetAttribute(fieldInfo.Name, objValue.ToString());
                    }
                    else if (fieldInfo.FieldType == typeof(string))
                    {
                        compEle.SetAttribute(fieldInfo.Name, objValue.ToString());
                    }
                    else if (fieldInfo.FieldType == typeof(UnityEngine.Object))
                    {
                        string objPath = AssetDatabase.GetAssetPath((UnityEngine.Object)objValue);
                        objPath = objPath.Substring(objPath.LastIndexOf('/') + 1);
                        compEle.SetAttribute(fieldInfo.Name, objPath);
                        //Debug.LogError("objPath" + objPath);
                    }
                    else
                    {
                        Debug.LogError(EditorApplication.currentScene + "中的" + prefabGo.name + "的组件" + objComp.GetType().Name + "使用了不支持的类型！" + fieldInfo.FieldType.Name);
                    }
                }
            }
            if (compEle.HasAttributes)
            {
                propEle.AppendChild(compEle);
            }
        }
    }


}
