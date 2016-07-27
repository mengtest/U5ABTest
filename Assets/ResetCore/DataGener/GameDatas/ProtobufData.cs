using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using System.Reflection;
using System.Linq;
using ResetCore.Util;
using ResetCore.Asset;
using System.IO;

namespace ResetCore.Data.GameDatas.Protobuf
{
    public class ProtobufData : BaseData
    {
        public static readonly string ex = ".bytes";
        public static readonly string nameSpace = "ResetCore.Data.GameDatas.Protobuf";
        protected static Dictionary<int, T> GetDataMap<T>()
        {
            Type type = typeof(T);
            FieldInfo field = type.GetField("fileName");
            Dictionary<int, T> dictionary = null;
            if (field != null)
            {
                string fileName = field.GetValue(null) as string;
                dictionary = (ProtobufDataController.instance.FormatData(fileName, typeof(Dictionary<int, T>), type) as Dictionary<int, T>);
            }
            else
            {
                dictionary = new Dictionary<int, T>();
            }
            return dictionary;
        }
    }

    public abstract class ProtobufData<T> : ProtobufData where T : ProtobufData<T>
    {
        private static Dictionary<int, T> m_dataMap;

        public static Dictionary<int, T> dataMap
        {
            get
            {
                if (ProtobufData<T>.m_dataMap == null)
                {
                    ProtobufData<T>.m_dataMap = ProtobufData.GetDataMap<T>();
                }
                return ProtobufData<T>.m_dataMap;
            }
            set
            {
                ProtobufData<T>.m_dataMap = value;
            }
        }

        public static T Select(Func<T, bool> condition)
        {
            return ProtobufData<T>.dataMap.Values.FirstOrDefault((data) =>
            {
                return condition(data);
            });
        }
    }

    public class ProtobufDataController : Singleton<ProtobufDataController>
    {
        public object FormatData(string fileName, Type dicType, Type type)
        {
            TextAsset asset = ResourcesLoaderHelper.Instance.LoadResource<TextAsset>(fileName + ProtobufData.ex);
            MemoryStream ms = new MemoryStream(asset.bytes);
            BinaryReader br = new BinaryReader(ms);

            object resDict = Activator.CreateInstance(dicType);
            Type listType = Type.GetType("System.Collections.Generic.List`1[[" + type.FullName + ", Assembly-CSharp]]");

            int len = br.ReadInt32();
            byte[] itembuf = br.ReadBytes(len);

            object resList = ProtoBuf.Serializer.NonGeneric.Deserialize(listType, new MemoryStream(itembuf));

            int listCount = (int)listType.GetProperty("Count").GetValue(resList, null);
            MethodInfo addMethod = dicType.GetMethod("Add");
            MethodInfo listGetMethod = listType.GetMethod("get_Item", BindingFlags.Instance | BindingFlags.Public);
            for (int i = 0; i < listCount; i++)
            {
                addMethod.Invoke(resDict, new object[] { i + 1, listGetMethod.Invoke(resList, new object[] { i }) });
            }
            return resDict;
        }
    }
}
