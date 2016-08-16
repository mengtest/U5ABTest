using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using System.Reflection;
using ResetCore.Data;
using ResetCore.Util;
using System.Linq;

namespace ResetCore.Data.GameDatas.Obj
{
    public class ObjData : BaseData
    {
        public static readonly string nameSpace = "ResetCore.Data.GameDatas.Obj";
        protected static Dictionary<int, T> GetDataMap<T>()
        {
            Type type = typeof(T);
            FieldInfo field = type.GetField("fileName");
            Dictionary<int, T> dictionary = new Dictionary<int, T>();
            if (field != null)
            {
                string fileName = field.GetValue(null) as string;
                dictionary = (ObjDataController.instance.FormatObjData(fileName) as Dictionary<int, T>);
            }
            else
            {
                dictionary = new Dictionary<int, T>();
            }
            return dictionary;
        }
    }

    public abstract class ObjData<T> : ObjData where T : ObjData<T>
    {
        private static Dictionary<int, T> m_dataMap;

        public static Dictionary<int, T> dataMap
        {
            get
            {
                if (ObjData<T>.m_dataMap == null)
                {
                    ObjData<T>.m_dataMap = ObjData.GetDataMap<T>();
                }
                return ObjData<T>.m_dataMap;
            }
            set
            {
                ObjData<T>.m_dataMap = value;
            }
        }

        public static T Select(Func<T, bool> condition)
        {
            return ObjData<T>.dataMap.Values.FirstOrDefault((data) =>
            {
                return condition(data);
            });
        }
    }

    public class ObjDataController : Singleton<ObjDataController>
    {

        protected readonly string m_resourcePath = PathConfig.localGameDataObjPath;

        protected readonly string m_fileExtention = ".objdat";

        public object FormatObjData(string fileName)
        {
            object obj = Resources.Load(fileName);
            Type objType = obj.GetType();
            FieldInfo fieldInfo = objType.GetField("DataArray");

            object array = fieldInfo.GetValue(obj);
            Type arrayType = array.GetType();

            Dictionary<int, object> dict = new Dictionary<int, object>();

            int Count = (int)arrayType.GetProperty("Count").GetValue(array, null);
            if (Count == 0) return String.Empty;
            MethodInfo mget = arrayType.GetMethod("get_Item", BindingFlags.Instance | BindingFlags.Public);

            object item;

            for (int i = 0; i < Count; i++)
            {
                item = mget.Invoke(array, new object[] { i });
                dict.Add(i + 1, item);
            }
            return dict;
        }

    }
}
