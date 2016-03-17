using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using System.Reflection;
using System.Xml.Linq;
using ResetCore.Util;


namespace ResetCore.Data
{
    public abstract class GameData
    {
        public int id
        {
            get;
            protected set;
        }

        protected static Dictionary<int, T> GetDataMap<T>()
        {
            Type type = typeof(T);
            FieldInfo field = type.GetField("fileName");
            Dictionary<int, T> dictionary;
            if (field != null)
            {
                string fileName = field.GetValue(null) as string;
                dictionary = (GameDataControler.Instance.FormatData(fileName, typeof(Dictionary<int, T>), type) as Dictionary<int, T>);
            }
            else
            {
                dictionary = new Dictionary<int, T>();
            }
            return dictionary;
        }
    }

    public abstract class GameData<T> : GameData
    {
        private static Dictionary<int, T> m_dataMap;

        public static Dictionary<int, T> dataMap
        {
            get
            {
                if (GameData<T>.m_dataMap == null)
                {
                    GameData<T>.m_dataMap = GameData.GetDataMap<T>();
                }
                return GameData<T>.m_dataMap;
            }
            set
            {
                GameData<T>.m_dataMap = value;
            }
        }
    }


    public abstract class DataLoader
    {
        protected readonly string m_resourcePath;

        protected readonly string m_fileExtention = ".xml";


        protected DataLoader()
        {
            m_resourcePath = PathConfig.localGameDataXmlPath;
        }
    }

    public class GameDataControler : DataLoader
    {
        private static GameDataControler m_instance;

        public static GameDataControler Instance
        {
            get
            {
                if (GameDataControler.m_instance == null)
                {
                    GameDataControler.m_instance = new GameDataControler();
                }
                return GameDataControler.m_instance;
            }
        }

        public object FormatData(string fileName, Type dicType, Type type)
        {
            return this.FormatXMLData(fileName + this.m_fileExtention, dicType, type);
        }

        private object FormatXMLData(string fileName, Type dicType, Type type)
        {
            object dataDic = null;
            object result;
            try
            {
                Dictionary<int, Dictionary<string, string>> dictionary = new Dictionary<int, Dictionary<string, string>>();
                dataDic = dicType.GetConstructor(Type.EmptyTypes).Invoke(null);
                if (!MyXMLParser.LoadIntMap(fileName, out dictionary))
                {
                    //加载失败
                    Debug.logger.LogError("GameData", "数据加载失败！");
                    result = dataDic;
                    return result;
                }
                Debug.logger.Log("dictionary.count" + dictionary.Count);
                PropertyInfo[] properties = type.GetProperties();
                foreach (KeyValuePair<int, Dictionary<string, string>> pair in dictionary)
                {
                    object propInstance = type.GetConstructor(Type.EmptyTypes).Invoke(null);
                    PropertyInfo[] array = properties;
                    for (int i = 0; i < array.Length; i++)
                    {
                        PropertyInfo propInfo = array[i];
                        if (propInfo.Name == "id")
                        {
                            //Key值为序号
                            propInfo.SetValue(propInstance, pair.Key, null);
                        }
                        else if (pair.Value.ContainsKey(propInfo.Name))
                        {
                            object propValue = StringEx.GetValue(pair.Value[propInfo.Name], propInfo.PropertyType);
                            propInfo.SetValue(propInstance, propValue, null);
                        }
                    }
                    dicType.GetMethod("Add").Invoke(dataDic, new object[]
					{
						pair.Key,
						propInstance
					});
                }
            }
            catch (Exception exception)
            {
                Debug.logger.LogError("GameData", "FormatData Error: " + fileName + "  " + exception.Message + " " + exception.StackTrace);
            }
            result = dataDic;
            return result;
        }

    }
}
