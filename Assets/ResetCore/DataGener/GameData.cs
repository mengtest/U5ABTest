using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using System.Reflection;

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
        m_resourcePath = PathConfig.localGameDataPath;
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
        return this.FormatXMLData(this.m_resourcePath + fileName + this.m_fileExtention, dicType, type);
    }

    private object FormatXMLData(string fileName, Type dicType, Type type)
    {
        object obj2 = null;
        object result;
        try
        {
            Dictionary<int, Dictionary<string, string>> dictionary = new Dictionary<int, Dictionary<string, string>>();
            obj2 = dicType.GetConstructor(Type.EmptyTypes).Invoke(null);
            //if (!XMLParser.LoadIntMap(fileName, false, out dictionary))
            //{
            //    result = obj2;
            //    return result;
            //}
            PropertyInfo[] properties = type.GetProperties();
            foreach (KeyValuePair<int, Dictionary<string, string>> pair in dictionary)
            {
                object obj3 = type.GetConstructor(Type.EmptyTypes).Invoke(null);
                PropertyInfo[] array = properties;
                for (int i = 0; i < array.Length; i++)
                {
                    PropertyInfo info = array[i];
                    if (info.Name == "id")
                    {
                        info.SetValue(obj3, pair.Key, null);
                    }
                    else if (pair.Value.ContainsKey(info.Name))
                    {
                        object obj4 = StringEx.GetValue(pair.Value[info.Name], info.PropertyType);
                        info.SetValue(obj3, obj4, null);
                    }
                }
                dicType.GetMethod("Add").Invoke(obj2, new object[]
					{
						pair.Key,
						obj3
					});
            }
        }
        catch (Exception exception)
        {
            Debug.logger.LogError("GameData", "FormatData Error: " + fileName + "  " + exception.Message);
        }
        result = obj2;
        return result;
    }
}