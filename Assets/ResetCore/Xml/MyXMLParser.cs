using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Xml.Linq;

using ResetCore.Asset;

namespace ResetCore.Util
{
    public class MyXMLParser
    {

        public static bool LoadIntMap(string fileName, out Dictionary<int, Dictionary<string, string>> dicFromXml)
        {
            TextAsset textAsset = ResourcesLoaderHelper.Instance.LoadTextAsset(fileName);
            if (textAsset == null)
            {
                Debug.logger.LogError("XMLParser", fileName + " 文本加载失败");
            }
            XDocument xDoc = XDocument.Parse(textAsset.text);
            XElement root = xDoc.Root;
            dicFromXml = new Dictionary<int, Dictionary<string, string>>();
            if (xDoc == null) return false;
            int id = 1;
            Debug.Log("Elements.Count" + root.Elements());
            foreach (XElement item in root.Elements())
            {
                Dictionary<string, string> propDic = new Dictionary<string, string>();
                foreach (XElement propItem in item.Elements())
                {
                    string key = propItem.Name.LocalName;
                    //忽略后缀
                    if (key.Contains("_"))
                    {
                        key = key.Split('_')[0];
                    }
                    else
                    {
                        Debug.logger.LogError("XMLPraser", "未记录类型信息，将无法自动生成GameData代码" + key);
                    }

                    if (!propDic.ContainsKey(key))
                    {
                        propDic.Add(key, propItem.Value);
                    }
                    else
                    {
                        Debug.logger.LogError("XMLPraser", "已经拥有相同的键值" + key);
                    }

                }
                dicFromXml.Add(id, propDic);
                id++;
            }
            return true;
        }
    }
}

