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

using ResetCore.Util;


namespace ResetCore.Asset
{
    public class SceneXmlBuilder
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
                if (!EditorSceneManager.GetSceneByPath(item).isLoaded)
                {
                    EditorSceneManager.OpenScene(item);
                }

                WriteXml();
                num++;
                Debug.Log("导出场景为" + item);
            }
            EditorUtility.ClearProgressBar();
        }

        private static void WriteXml()
        {
            //获取当前场景完整路径
            scenePath = SceneManager.GetAllScenes()[0].path;
            //Debug.logger.Log(scenePath);
            //获取当前场景名称
            sceneName = scenePath.Substring(scenePath.LastIndexOf("/") + 1, scenePath.Length - scenePath.LastIndexOf("/") - 1);
            sceneName = sceneName.Substring(0, sceneName.LastIndexOf("."));

            if (!Directory.Exists(dataRoot))
            {
                Directory.CreateDirectory(dataRoot);
            }

            string dynamicDataPath = dataRoot + dynamicSceneEx + sceneName + ".xml";
            string staticDataPath = dataRoot + staticSceneEx + sceneName + ".xml";
            Debug.logger.Log("导出路径为" + dynamicDataPath + "和" + staticDataPath);

            WriteDynamicData(dynamicDataPath);
            WriteStaticData(staticDataPath);
        }

        private static Stack<string> currentTree = new Stack<string>();
        //写入动态物体
        private static void WriteDynamicData(string path)
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
            xmlDoc.Save(path);
        }
        //写入静态物体
        private static void WriteStaticData(string path)
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

        private static void WritePrefabObject(XmlDocument xmlDoc, XmlElement parent, GameObject obj)
        {
            if (obj.activeSelf == false) return;
            currentTree.Push(obj.name);
            PrefabType prefabType = PrefabUtility.GetPrefabType(obj);

            XmlElement gameObjectRoot = xmlDoc.CreateElement("GameObject");
            gameObjectRoot.SetAttribute("PrefabType", prefabType.ToString());
            XmlElement fieldRoot = xmlDoc.CreateElement("Field");
            XmlElement propRoot = xmlDoc.CreateElement("Prop");


            //当为没有Prefab的时候
            if (prefabType == PrefabType.None)
            {
                gameObjectRoot.SetAttribute("Name", obj.name);
                WriteTransform(xmlDoc, fieldRoot, obj);
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
                    WriteTransform(xmlDoc, fieldRoot, obj);
                }

                WriteChangedValue(xmlDoc, propRoot, fieldRoot, obj);
                //对Prefab中的子物体进行遍历

            }
            else
            {
                string prefabPath = AssetDatabase.GetAssetPath(PrefabUtility.GetPrefabParent(obj));
                if (string.IsNullOrEmpty(prefabPath))
                {
                    Debug.LogError(SceneManager.GetAllScenes()[0].path + "中的物体" + obj.name + "丢失预设！！！！");
                    gameObjectRoot.SetAttribute("Name", obj.name + "'s Prefab has Error");
                }
                else
                {
                    gameObjectRoot.SetAttribute("Name", prefabPath);
                }
                Debug.LogError(SceneManager.GetAllScenes()[0].path + "中的物体" + obj.name + "的预设类型为" + prefabType.ToString());
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



            if (fieldRoot.HasChildNodes)
            {
                gameObjectRoot.AppendChild(fieldRoot);
            }
            if (propRoot.HasChildNodes)
            {
                gameObjectRoot.AppendChild(propRoot);
            }

            if (gameObjectRoot.HasChildNodes)
            {
                parent.AppendChild(gameObjectRoot);
            }

            currentTree.Pop();
        }
        //写入位置
        private static void WriteTransform(XmlDocument xmlDoc, XmlElement propEle, GameObject obj)
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

        private static void WriteChangedValue(XmlDocument xmlDoc, XmlElement propEle, XmlElement fieldEle, GameObject obj)
        {
            string prefabPath = AssetDatabase.GetAssetPath(PrefabUtility.GetPrefabParent(obj));
            GameObject prefabGo = AssetDatabase.LoadAssetAtPath(prefabPath, typeof(GameObject)) as GameObject;

            //当不为Prefab的根物体就要寻找该物体在Prefab中的位置
            //（在这里不允许出现子物体重名！并且要求场景中子物体名与Prefab中的相同！）
            bool hasTruePrefabExist = GetPrefabInAsset(obj, ref prefabGo);
            if (!hasTruePrefabExist) return;


            Component[] prefabComps = prefabGo.GetComponents<Component>();
            Component[] objComps = obj.GetComponents<Component>();

            if (!HasSameComponent(obj, prefabComps, objComps)) return;

            //开始检查每个组件中的域
            for (int compNum = 0; compNum < prefabComps.Length; compNum++)
            {
                Component prefabComp = prefabComps[compNum];
                Component objComp = objComps[compNum];

                AddFieldInfos(xmlDoc, fieldEle, prefabComp, objComp);
                AddPropInfos(xmlDoc, propEle, prefabComp, objComp);
            }
        }

        //加上相应的域信息
        private static void AddFieldInfos(XmlDocument xmlDoc, XmlElement fieldEle, Component prefabComp, Component objComp)
        {
            Type compType = prefabComp.GetType();

            XmlElement compEle = xmlDoc.CreateElement(compType.Name);

            FieldInfo[] fieldInfos = compType.GetFields();
            for (int infoNum = 0; infoNum < fieldInfos.Length; infoNum++)
            {
                FieldInfo fieldInfo = fieldInfos[infoNum];

                if (StringEx.IsConvertableType(fieldInfo.FieldType) && fieldInfo.IsPublic == true)//为公共类型并且与Prefab不一样了
                {
                    object prefabValue = fieldInfo.GetValue(prefabComp);
                    object objValue = fieldInfo.GetValue(objComp);

                    if (!prefabValue.Equals(objValue))
                    {
                        Debug.Log(compType.Name);
                        Debug.Log(fieldInfos[infoNum].Name);
                        Debug.Log(prefabValue + "  " + objValue);
                        compEle.SetAttribute(fieldInfo.Name, StringEx.ConverToString(objValue));
                    }

                }
            }
            if (compEle.HasAttributes)
            {
                fieldEle.AppendChild(compEle);
            }
        }

        private static void AddPropInfos(XmlDocument xmlDoc, XmlElement propEle, Component prefabComp, Component objComp)
        {
            Type compType = prefabComp.GetType();

            XmlElement compEle = xmlDoc.CreateElement(compType.Name);

            PropertyInfo[] propInfos = compType.GetProperties();
            for (int infoNum = 0; infoNum < propInfos.Length; infoNum++)
            {
                PropertyInfo propInfo = propInfos[infoNum];

                if (StringEx.IsConvertableType(propInfo.PropertyType) &&
                    propInfo.GetGetMethod() != null && propInfo.GetGetMethod().IsPublic == true &&
                    propInfo.GetSetMethod() != null && propInfo.GetSetMethod().IsPublic == true)
                {
                    object prefabValue = propInfo.GetValue(prefabComp, null);
                    object objValue = propInfo.GetValue(objComp, null);

                    if (prefabValue == null || objValue == null) continue;

                    if (!prefabValue.Equals(objValue))
                    {
                        Debug.Log(compType.Name);
                        Debug.Log(propInfos[infoNum].Name);
                        Debug.Log(prefabValue + "!!!!!!!!!!!!!!!!!" + objValue);

                        compEle.SetAttribute(propInfo.Name, StringEx.ConverToString(objValue));
                    }

                }
            }
            if (compEle.HasAttributes)
            {
                propEle.AppendChild(compEle);
            }
        }



        //获取从AssetDatabase中加载出来的Prefab
        private static bool GetPrefabInAsset(GameObject obj, ref GameObject prefabGo)
        {
            bool flag = true;
            if (PrefabUtility.FindPrefabRoot(obj) != obj)
            {
                GameObject tempObj = obj;
                List<string> objList = new List<string>();
                //寻找场景中物体与Prefab根的相对路径
                while (PrefabUtility.FindPrefabRoot(tempObj) != tempObj)
                {
                    objList.Add(tempObj.name);
                    tempObj = tempObj.transform.parent.gameObject;
                }
                //寻找从AssetDatabase中加载出来的物体的相对路径相同的物体
                for (int i = objList.Count - 1; i >= 0; i--)
                {
                    string childName = objList[i];
                    Transform finedChild = prefabGo.transform.FindChild(childName);
                    if (finedChild != null)
                    {
                        prefabGo = finedChild.gameObject;
                    }
                    else
                    {
                        Debug.LogError("在" + prefabGo.name + "中找不到子物体" + childName);
                        flag = false;
                    }

                }
            }
            return flag;
        }

        //场景中的物体与数据库中相应物体是否有相同的组件
        private static bool HasSameComponent(GameObject obj, Component[] prefabComps, Component[] objComps)
        {
            bool hasSameComponent = true;
            //检查组件是否相同
            if (prefabComps.Length != objComps.Length)
            {
                Debug.LogError(SceneManager.GetAllScenes()[0].path + "中的物体" + obj.name + "的Component与其Prefab上的组件不同！");
                hasSameComponent = false;
            }
            for (int i = 0; i < prefabComps.Length; i++)
            {
                if (prefabComps.GetType() != objComps.GetType())
                {
                    Debug.LogError(SceneManager.GetAllScenes()[0].path + "中的物体" + obj.name + "的Component与其Prefab上的组件不同！");
                    hasSameComponent = false;
                }
            }
            return hasSameComponent;
        }
    }

}
