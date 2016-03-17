using UnityEngine;
using System.Collections;
using System.Xml.Linq;
using System.Collections.Generic;
using System.Text;
using System;
using System.IO;
using System.Reflection;
using ResetCore.Util;
using ResetCore.Asset;
using UnityEngine.SceneManagement;

public class XmlSceneBuilder : MonoSingleton<XmlSceneBuilder>
{


    private static Action<Boolean> loadedAction;
    private static Action<int> processAction;
    private static bool isDone = true;

    private static readonly string dynamicSceneEx = "Dynamic";
    private static readonly string staticSceneEx = "Static";

    public void SceneBuilder(string stageName, Action<Boolean> loaded, Action<int> process = null)
    {
        //InitLoading.stageId

        loadedAction = loaded;
        processAction = process;

        Debug.Log("stageName:" + stageName);
        //string data = Resources.Load(dataMetaPathInResource).ToString();

        string dynamicScenePath = dynamicSceneEx + stageName + ".xml";
        string staticScenePath = staticSceneEx + stageName + ".xml";
        StartCoroutine(CreateScene(dynamicScenePath, staticScenePath));
    }


    private IEnumerator CreateScene(string dynamicSceneXmlName, string staticSceneXmlName)
    {
        yield return null;
        string[] nameList = { dynamicSceneXmlName, staticSceneXmlName };

        yield return StartCoroutine(CreateObject(nameList));
        yield return new WaitForSeconds(0.1f);
        loadedAction(true);
    }

    //创建物体
    private Stack<string> currentTree = new Stack<string>();
    private int objectNum = 0;
    private int createdObjectNum = 0;
    private IEnumerator CreateObject(string[] pathList)
    {
        int num = 0;
        objectNum = 0;
        createdObjectNum = 0;

        //计数
        foreach (string path in pathList)
        {
            string data = ResourcesLoaderHelper.Instance.LoadTextAsset(path).text;
            XDocument xDoc = XDocument.Parse(data);
            XElement root = xDoc.Root;

            foreach (XElement objEle in root.Elements())
            {
                objectNum += GetObjectNum(objEle);
                yield return null;
            }
            yield return null;
        }
        yield return null;
        
        //加载对象
        foreach (string path in pathList)
        {
            string data = ResourcesLoaderHelper.Instance.LoadTextAsset(path).text;
            XDocument xDoc = XDocument.Parse(data);
            XElement root = xDoc.Root;

            currentTree.Push(root.Name.ToString());

            foreach (XElement objEle in root.Elements())
            {
                while (!isDone)
                {
                    yield return null;
                }
                yield return StartCoroutine(CreateObject(objEle, null));
            }

            num++;
            currentTree.Pop();
        }
    }

    private int GetObjectNum(XElement ele)
    {
        int num = 0;
        if (ele.Attribute("PrefabType").Value == "None" && ele.Element("Children") != null)
        {
            foreach (XElement childEle in ele.Element("Children").Elements())
            {
                num += GetObjectNum(childEle);
            }
        }
        else
        {
            num++;
        }
        return num;
    }

    private void UpdateProcess()
    {
        createdObjectNum++;
        if (processAction != null)
        {
            //Debug.LogError("进度" + ((int)(60 * (float)createdObjectNum / (float)objectNum) + 20));
            processAction((int)(60 * (float)createdObjectNum / (float)objectNum) + 20);
        }
    }

    //递归创建物体
    private IEnumerator CreateObject(XElement ele, GameObject parentObj)
    {
        XElement fieldEl = ele.Element("Field");
        XElement propEl = ele.Element("Prop");

        isDone = false;
        currentTree.Push(ele.Name.ToString());
        GameObject newObj = null;
        if (ele.Attribute("PrefabType").Value == "None")
        {
            newObj = new GameObject(ele.Attribute("Name").Value);
            //给予其父对象
            if (newObj != null && parentObj != null)
            {
                newObj.transform.parent = parentObj.transform;
                SetObjectTransform(ele, newObj);
            }
            yield return StartCoroutine(CreateChildrenObject(ele, newObj));

            UpdateProcess();
        }
        else
        {
            string path = ele.Attribute("Name").Value;
            string resName = path.Substring(path.LastIndexOf('/') + 1);
            if (!string.IsNullOrEmpty(path))
            {
                if (ele.Attribute("PrefabType").Value == "PrefabInstance")
                {
                    ResourcesLoaderHelper.Instance.LoadResource(resName, (go) =>
                    {

                        newObj = GameObject.Instantiate(go) as GameObject;
                        //给予其父对象
                        if (newObj != null && parentObj != null)
                        {
                            newObj.transform.parent = parentObj.transform;
                            StartCoroutine(CreateChildrenObject(ele, newObj));
                            SetObjectTransform(ele, newObj);
                            SetChangedField(fieldEl, newObj);
                            SetChangedProp(propEl, newObj);
                            //Debug.Log("创建" + ele.Attribute("Name").Value + "对象成功！");
                        }
                        else
                        {
                            if (!string.IsNullOrEmpty(resName))
                            {
                                Debug.LogError("创建" + resName + "对象失败！");
                            }
                            else
                            {
                                Debug.LogError("名称为空，请检查Xml");
                            }

                        }
                        UpdateProcess();

                    });
                }
                else if (ele.Attribute("PrefabType").Value == "PrefabChild")
                {
                    //Debug.LogError("设置中！！");
                    newObj = parentObj.transform.FindChild(ele.Attribute("Name").Value).gameObject;
                    SetChangedField(fieldEl, newObj);
                    SetChangedProp(propEl, newObj);
                    yield return StartCoroutine(CreateChildrenObject(ele, newObj));
                    UpdateProcess();
                }
                else
                {
                    Debug.LogError("Prefab类型为不支持的类型" + ele.Attribute("PrefabType").Value);
                }

                yield return null;
            }
            else
            {
                Debug.LogError("名称为空，请检查Xml");
            }

        }

        currentTree.Pop();
    }

    private IEnumerator CreateChildrenObject(XElement parentEle, GameObject parentObj)
    {
        //Debug.LogError(parentEle.Attribute("Name") + "的Child节点为" + parentEle.Element("Children"));
        if (parentEle.Element("Children") != null)
        {
            foreach (XElement childEle in parentEle.Element("Children").Elements())
            {
                yield return StartCoroutine(CreateObject(childEle, parentObj));
                yield return null;
            }
            yield return null;
        }
        isDone = true;
    }

    private void SetObjectTransform(XElement objEle, GameObject obj)
    {
        obj.transform.localPosition = GetXYZ(objEle.Element("Field").Element("Transform").Element("Position"));
        obj.transform.localEulerAngles = GetXYZ(objEle.Element("Field").Element("Transform").Element("Rotation"));
        obj.transform.localScale = GetXYZ(objEle.Element("Field").Element("Transform").Element("Scale"));
    }

    //获取XYZ属性
    private Vector3 GetXYZ(XElement ele)
    {
        Vector3 xyz = new Vector3(float.Parse(ele.Attribute("X").Value), float.Parse(ele.Attribute("Y").Value), float.Parse(ele.Attribute("Z").Value));
        return xyz;
    }

    private void SetChangedField(XElement fieldEle, GameObject obj)
    {
        if (fieldEle == null || obj == null || !fieldEle.HasElements)
            return;
        foreach (XElement ele in fieldEle.Elements())
        {
            if (ele.Name.Equals("Transform"))
            {
                continue;
            }
            Component comp = obj.GetComponent(ele.Name.LocalName);
            Type compType = comp.GetType();
            foreach (XAttribute attr in ele.Attributes())
            {
                FieldInfo fieldInfo = compType.GetField(attr.Name.LocalName);
                Debug.Log("变量名为 " + attr.Name.LocalName + " 变量类型为 " + fieldInfo.FieldType.Name);
                fieldInfo.SetValue(comp, StringEx.GetValue(attr.Value, fieldInfo.FieldType));
            }
        }
    }

    private void SetChangedProp(XElement propEle, GameObject obj)
    {
        if (propEle == null || obj == null || !propEle.HasElements)
            return;
        foreach (XElement ele in propEle.Elements())
        {
            if (ele.Name.Equals("Transform"))
            {
                continue;
            }
            Component comp = obj.GetComponent(ele.Name.LocalName);
            Type compType = comp.GetType();
            foreach (XAttribute attr in ele.Attributes())
            {
                PropertyInfo propInfo = compType.GetProperty(attr.Name.LocalName);
                //Debug.Log("变量名为 " + attr.Name.LocalName + " 变量类型为 " + propInfo.PropertyType.Name + " 值为 " + attr.Value);
                propInfo.SetValue(comp, StringEx.GetValue(attr.Value, propInfo.PropertyType), null);
            }
        }
    }

}
