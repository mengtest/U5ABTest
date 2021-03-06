﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using System.Text.RegularExpressions;
using System.Text;
using System.Reflection;


namespace ResetCore.Util
{
    public static class StringEx
    {

        public static string GetDirectoryName(string fileName)
        {
            return fileName.Substring(0, fileName.LastIndexOf('/'));
        }

        public static string GetFileName(string path, char separator = '/')
        {
            return path.Substring(path.LastIndexOf(separator) + 1);
        }

        public static string GetFileNameWithoutExtention(string fileName, char separator = '/')
        {
            return GetFilePathWithoutExtention(GetFileName(fileName, separator));
        }

        public static string GetFilePathWithoutExtention(string fileName)
        {
            if (fileName.Contains("."))
                return fileName.Substring(0, fileName.LastIndexOf('.'));
            return fileName;
        }

        public static string GetStreamPath(string fileName)
        {
            string str = Application.streamingAssetsPath + "/" + fileName;
            if (Application.platform != RuntimePlatform.Android)
            {
                str = "file://" + str;
            }
            return str;
        }

        public static char Spriter1 = ',';
        public static char Spriter2 = ':';

        public static char FBracket1 = '(';
        public static char BBracket1 = ')';


        public static T GetValue<T>(this string value)
        {
            return (T)GetValue(value, typeof(T));
        }

        public static object GetValue(this string value, System.Type type)
        {
            //Debug.logger.Log("GetValue " + Spriter1 + "  " + Spriter2);
            if (type != null)
            {
                object obj2;
                object obj3;
                if (type == typeof(string))
                {
                    return value;
                }
                if (type == typeof(int))
                {
                    return Convert.ToInt32(Convert.ToDouble(value));
                }
                if (type == typeof(float))
                {
                    return float.Parse(value);
                }
                if (type == typeof(byte))
                {
                    return Convert.ToByte(Convert.ToDouble(value));
                }
                if (type == typeof(sbyte))
                {
                    return Convert.ToSByte(Convert.ToDouble(value));
                }
                if (type == typeof(uint))
                {
                    return Convert.ToUInt32(Convert.ToDouble(value));
                }
                if (type == typeof(short))
                {
                    return Convert.ToInt16(Convert.ToDouble(value));
                }
                if (type == typeof(long))
                {
                    return Convert.ToInt64(Convert.ToDouble(value));
                }
                if (type == typeof(ushort))
                {
                    return Convert.ToUInt16(Convert.ToDouble(value));
                }
                if (type == typeof(ulong))
                {
                    return Convert.ToUInt64(Convert.ToDouble(value));
                }
                if (type == typeof(double))
                {
                    return double.Parse(value);
                }
                if (type == typeof(bool))
                {
                    return bool.Parse(value);
                }
                if (type.BaseType == typeof(Enum))
                {
                    return GetValue(value, Enum.GetUnderlyingType(type));
                }
                if (type == typeof(Vector2))
                {
                    Vector2 vector;
                    ParseVector2(value, out vector);
                    return vector;
                }
                if (type == typeof(Vector3))
                {
                    Vector3 vector;
                    ParseVector3(value, out vector);
                    //Debug.LogError(vector.ToString());
                    return vector;
                }
                if (type == typeof(Vector4))
                {
                    Vector4 vector;
                    ParseVector4(value, out vector);
                    return vector;
                }
                if (type == typeof(Quaternion))
                {
                    Quaternion quaternion;
                    ParseQuaternion(value, out quaternion);
                    return quaternion;
                }
                if (type == typeof(Color))
                {
                    Color color;
                    ParseColor(value, out color);
                    return color;
                }
                if (type.IsGenericType && (type.GetGenericTypeDefinition() == typeof(Dictionary<,>)))
                {
                    System.Type[] genericArguments = type.GetGenericArguments();
                    Dictionary<string, string> dictionary = ParseMap(value, Spriter2 , Spriter1);
                    obj2 = type.GetConstructor(System.Type.EmptyTypes).Invoke(null);
                    foreach (KeyValuePair<string, string> pair in dictionary)
                    {
                        object obj4 = GetValue(pair.Key, genericArguments[0]);
                        obj3 = GetValue(pair.Value, genericArguments[1]);
                        type.GetMethod("Add").Invoke(obj2, new object[] { obj4, obj3 });
                    }
                    return obj2;
                }
                if (type.IsGenericType && (type.GetGenericTypeDefinition() == typeof(List<>)))
                {
                    System.Type type2 = type.GetGenericArguments()[0];
                    List<string> list = ParseList(value, Spriter1);
                    obj2 = type.GetConstructor(System.Type.EmptyTypes).Invoke(null);
                    foreach (string str in list)
                    {
                        obj3 = GetValue(str, type2);
                        type.GetMethod("Add").Invoke(obj2, new object[] { obj3 });
                    }
                    return obj2;
                }
                if (type.IsArray)
                {
                    Type elementType = Type.GetType(
                     type.FullName.Replace("[]", string.Empty));
                    string[] elStr = value.Split(Spriter1);
                    Array array = Array.CreateInstance(elementType, elStr.Length);

                    for (int i = 0; i < elStr.Length; i++)
                    {
                        array.SetValue(elStr[i].GetValue(elementType), i);
                    }
                    return array;
                }
            }
            Debug.logger.LogWarning("字符转换", "没有适合的转换类型，返回默认值");
            return null;
        }

        public static float GetRandom(this string str)
        {
            string[] strs = str.Split('~');
            float num1 = strs[0].GetValue<float>();
            float num2 = strs[1].GetValue<float>();
            if (str.Length == 1)
            {
                return num1;
            }
            else
            {
                return UnityEngine.Random.Range(Mathf.Min(num1, num2), Mathf.Max(num1, num2));
            }
        }

        public static string ConverToString(this object value)
        {
            //Debug.logger.Log("ConverToString " + Spriter1 + "  "+ Spriter2);
            System.Type type = value.GetType();
            if (type == null)
            {
                return "";
            }
            if (type == typeof(Vector3))
            {
                return FBracket1.ToString() + ((Vector3)value).x + Spriter1.ToString() + ((Vector3)value).y + Spriter1.ToString() + ((Vector3)value).z + BBracket1.ToString();
            }
            if (type == typeof(Vector2))
            {
                return FBracket1.ToString() + ((Vector2)value).x + Spriter1.ToString() + ((Vector2)value).y + BBracket1.ToString();
            }
            if (type == typeof(Vector4))
            {
                return FBracket1.ToString() + ((Vector4)value).x + Spriter1.ToString() + ((Vector4)value).y + Spriter1.ToString() + ((Vector4)value).z + Spriter1.ToString() + ((Vector4)value).w + BBracket1.ToString();
            }
            if (type == typeof(Quaternion))
            {
                return FBracket1.ToString() + ((Quaternion)value).x + Spriter1.ToString() + ((Quaternion)value).y + Spriter1.ToString() + ((Quaternion)value).z + Spriter1.ToString() + ((Quaternion)value).w + BBracket1.ToString();
            }
            if (type == typeof(Color))
            {
                return FBracket1.ToString() + ((Color)value).r + Spriter1.ToString() + ((Color)value).g + Spriter1.ToString() + ((Color)value).b + BBracket1.ToString();
            }
            if (type.BaseType == typeof(Enum))
            {
                return Enum.GetName(type, value);
            }
            if (type.IsGenericType && (type.GetGenericTypeDefinition() == typeof(Dictionary<,>)))
            {
                int Count = (int)type.GetProperty("Count").GetValue(value, null);
                if (Count == 0) return String.Empty;
                MethodInfo getIe = type.GetMethod("GetEnumerator");
                object enumerator = getIe.Invoke(value, new object[0]);
                System.Type enumeratorType = enumerator.GetType();
                MethodInfo moveToNextMethod = enumeratorType.GetMethod("MoveNext");
                PropertyInfo current = enumeratorType.GetProperty("Current");

                StringBuilder stringBuilder = new StringBuilder();

                while (enumerator != null && (bool)moveToNextMethod.Invoke(enumerator, new object[0]))
                {
                    stringBuilder.Append(Spriter1.ToString() + ConverToString(current.GetValue(enumerator, null)));
                }

                return stringBuilder.ToString().ReplaceFirst(Spriter1.ToString(), "");

            }
            if (type.IsGenericType && (type.GetGenericTypeDefinition() == typeof(KeyValuePair<,>)))
            {
                object pairKey = type.GetProperty("Key").GetValue(value, null);
                object pairValue = type.GetProperty("Value").GetValue(value, null);

                string keyStr = ConverToString(pairKey);
                string valueStr = ConverToString(pairValue);
                return keyStr + Spriter2.ToString() + valueStr;

            }
            if (type.IsGenericType && (type.GetGenericTypeDefinition() == typeof(List<>)))
            {
                 int Count = (int)type.GetProperty("Count").GetValue(value, null);
                if (Count == 0) return String.Empty;
                MethodInfo mget = type.GetMethod("get_Item", BindingFlags.Instance | BindingFlags.Public);

                StringBuilder stringBuilder = new StringBuilder();

                object item;
                string itemStr;

                for (int i = 0; i < Count - 1; i++)
                {
                    item = mget.Invoke(value, new object[] { i });
                   itemStr = StringEx.ConverToString(item);
                    stringBuilder.Append(itemStr + Spriter1.ToString());
                }
                item = mget.Invoke(value, new object[] { Count - 1 });
                itemStr = StringEx.ConverToString(item);
                stringBuilder.Append(itemStr);

                return stringBuilder.ToString();
            }
            if (type.IsArray)
            {
                StringBuilder stringBuilder = new StringBuilder();
                var array = value as Array;
                if (array.Length > 0)
                {
                   stringBuilder.Append(ConverToString(array.GetValue(0)));
                    for (int i = 1; i < array.Length; i++)
                    {
                        stringBuilder.Append(Spriter1.ToString());
                        stringBuilder.Append(ConverToString(array.GetValue(i)));
                    }
                    return stringBuilder.ToString();
                }
                else
                {
                    return string.Empty;
                }
            }
            //Debug.logger.LogWarning("字符转换", type.Name + "没有适合的转换类型，返回默认值");
            return value.ToString();
        }

        /// <summary>
        /// 将列表转换至字符串
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string ListConvertToString<T>(this List<T> value, char split1 = ',')
        {
            Type type = value.GetType();
            int Count = (int)type.GetProperty("Count").GetValue(value, null);
            if (Count == 0) return String.Empty;
            MethodInfo mget = type.GetMethod("get_Item", BindingFlags.Instance | BindingFlags.Public);

            StringBuilder stringBuilder = new StringBuilder();

            object item;
            string itemStr;

            for (int i = 0; i < Count - 1; i++)
            {
                item = mget.Invoke(value, new object[] { i });
                itemStr = StringEx.ConverToString(item);
                stringBuilder.Append(itemStr + split1.ToString());
            }
            item = mget.Invoke(value, new object[] { Count - 1 });
            itemStr = StringEx.ConverToString(item);
            stringBuilder.Append(itemStr);

            return stringBuilder.ToString();
        }

        /// <summary>
        /// 数组转换至字符串
        /// </summary>
        /// <param name="value"></param>
        /// <param name="split1"></param>
        /// <returns></returns>
        public static string ArrConvertToString(this Array value, char split1 = ',')
        {
            StringBuilder stringBuilder = new StringBuilder();
            var array = value as Array;
            if (array.Length > 0)
            {
                stringBuilder.Append(ConverToString(array.GetValue(0)));
                for (int i = 1; i < array.Length; i++)
                {
                    stringBuilder.Append(split1.ToString());
                    stringBuilder.Append(ConverToString(array.GetValue(i)));
                }
                return stringBuilder.ToString();
            }
            else
            {
                return string.Empty;
            }
        }

        /// <summary>
        /// 将键值对转换至字符串
        /// </summary>
        /// <param name="value"></param>
        /// <param name="split1"></param>
        /// <returns></returns>
        public static string KVPConvertToString<K, V>(this KeyValuePair<K, V> value, char split1 = ':')
        {
            Type type = value.GetType();
            object pairKey = type.GetProperty("Key").GetValue(value, null);
            object pairValue = type.GetProperty("Value").GetValue(value, null);

            string keyStr = ConverToString(pairKey);
            string valueStr = ConverToString(pairValue);
            return keyStr + Spriter2.ToString() + valueStr;
        }

        /// <summary>
        /// 将Dictionary转换至字符串
        /// </summary>
        /// <typeparam name="K"></typeparam>
        /// <typeparam name="V"></typeparam>
        /// <param name="value"></param>
        /// <param name="split1"></param>
        /// <param name="split2"></param>
        /// <returns></returns>
        public static string DictConvertToString<K, V>(this Dictionary<K, V> value, char split1 = ',', char split2 = ':')
        {
            Type type = value.GetType();
            int Count = (int)type.GetProperty("Count").GetValue(value, null);
            if (Count == 0) return String.Empty;
            MethodInfo getIe = type.GetMethod("GetEnumerator");
            object enumerator = getIe.Invoke(value, new object[0]);
            System.Type enumeratorType = enumerator.GetType();
            MethodInfo moveToNextMethod = enumeratorType.GetMethod("MoveNext");
            PropertyInfo current = enumeratorType.GetProperty("Current");

            StringBuilder stringBuilder = new StringBuilder();

            while (enumerator != null && (bool)moveToNextMethod.Invoke(enumerator, new object[0]))
            {
                stringBuilder.Append(split1.ToString() + ConverToString(current.GetValue(enumerator, null)));
            }

            return stringBuilder.ToString().ReplaceFirst(split1.ToString(), "");     
        }

        //可转换类型列表
        public static readonly List<Type> convertableTypes = new List<Type>
        {
            typeof(int),
            typeof(string),
            typeof(float),
            typeof(double),
            typeof(byte),
            typeof(long),
            typeof(bool),
            typeof(short),
            typeof(uint),
            typeof(ulong),
            typeof(ushort),
            typeof(sbyte),
            typeof(Vector3),
            typeof(Vector2),
            typeof(Vector4),
            typeof(Quaternion),
            typeof(Color),
            typeof(Dictionary<,>),
            typeof(KeyValuePair<,>),
            typeof(List<>),
            typeof(Enum),
            typeof(Array)
        };

        /// <summary>
        /// 通过文本获取类型：
        /// 注意！解析嵌套多泛型类型时会出现问题！
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static Type GetTypeByString(this string str)
        {
            str = str.Trim();
            switch (str)
            {
                case"int":
                    return typeof(int);
                case "float":
                    return typeof(float);
                case "string":
                    return typeof(string);
                case "double":
                    return typeof(double);
                case "byte":
                    return typeof(byte);
                case "bool":
                    return typeof(bool);
                case "short":
                    return typeof(short);
                case "uint":
                    return typeof(uint);
                case "ushort":
                    return typeof(ushort);
                case "sbyte":
                    return typeof(sbyte);
                case "Vector3":
                    return typeof(Vector3);
                case "Vector2":
                    return typeof(Vector2);
                case "Vector4":
                    return typeof(Vector4);
                case "Quaternion":
                    return typeof(Quaternion);
                case "Color":
                    return typeof(Color);
            }

            if (str.StartsWith("List"))
            {
                Type genType = str.Substring(str.IndexOf('<') + 1, str.IndexOf('>') - str.LastIndexOf('<') - 1).GetTypeByString();
                return Type.GetType("System.Collections.Generic.List`1[[" + genType.FullName + ", " + genType.Assembly.FullName + "]], " + typeof(List<>).Assembly.FullName);
            }

            if (str.StartsWith("Dictionary"))
            {
                string[] typeNames = str.Substring(str.IndexOf('<') + 1, str.IndexOf('>') - str.LastIndexOf('<') - 1).Split(',');
                Type type1 = typeNames[0].Trim().GetTypeByString();
                Type type2 = typeNames[1].Trim().GetTypeByString();
                string typeStr = "System.Collections.Generic.Dictionary`2[[" + type1.FullName + ", " + type1.Assembly.FullName + "]"+
                    ",[" + type2.FullName + ", " + type2.Assembly.FullName + "]], " +
                    typeof(Dictionary<,>).Assembly.FullName;
                return Type.GetType(typeStr);
            }
            //仅支持内置类型,支持多维数组
            if (str.Contains("[") && str.Contains("]"))
            {
                string itemTypeStr = str.Substring(0, str.IndexOf('['));
                string bracketStr = str.Substring(str.IndexOf('['), str.LastIndexOf(']') - str.IndexOf('[') + 1);
                Type itemType = itemTypeStr.GetTypeByString();
                string typeStr = itemType.FullName + bracketStr;
                return Type.GetType(typeStr);
            }
            return Type.GetType(str);
        }

        public static bool IsConvertableType(this Type type)
        {
            return convertableTypes.Contains(type);
        }

        public static bool ParseColor(string _inputString, out Color result, char colorSpriter = ',')
        {
            string str = _inputString.Trim();
            str = str.Replace(FBracket1.ToString(), "");
            str = str.Replace(BBracket1.ToString(), "");
            result = Color.clear;
            if (str.Length < 9)
            {
                return false;
            }
            try
            {
                string[] strArray = str.Split(new char[] { colorSpriter });
                if (strArray.Length != 4)
                {
                    return false;
                }
                result = new Color(float.Parse(strArray[0]) / 255f, float.Parse(strArray[1]) / 255f, float.Parse(strArray[2]) / 255f, float.Parse(strArray[3]) / 255f);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public static List<string> ParseList(this string strList, char listSpriter = ',')
        {
            List<string> list = new List<string>();
            if (!string.IsNullOrEmpty(strList))
            {
                string str = strList.Trim();
                if (string.IsNullOrEmpty(strList))
                {
                    return list;
                }
                string[] strArray = str.Split(new char[] { listSpriter });
                foreach (string str2 in strArray)
                {
                    if (!string.IsNullOrEmpty(str2))
                    {
                        list.Add(str2.Trim());
                    }
                }
            }
            return list;
        }

        public static Dictionary<string, string> ParseMap(this string strMap, char keyValueSpriter = ':', char mapSpriter = ',')
        {
            Dictionary<string, string> dictionary = new Dictionary<string, string>();
            if (!string.IsNullOrEmpty(strMap))
            {
                string[] strArray = strMap.Split(new char[] { mapSpriter });
                for (int i = 0; i < strArray.Length; i++)
                {
                    if (!string.IsNullOrEmpty(strArray[i]))
                    {
                        string[] strArray2 = strArray[i].Split(new char[] { keyValueSpriter });
                        if ((strArray2.Length == 2) && !dictionary.ContainsKey(strArray2[0]))
                        {
                            dictionary.Add(strArray2[0], strArray2[1]);
                        }
                    }
                }
            }
            return dictionary;
        }

        public static bool ParseVector4(string _inputString, out Vector4 result, char vectorSpriter = ',')
        {
            string str = _inputString.Trim();
            str = str.Replace(FBracket1.ToString(), "");
            str = str.Replace(BBracket1.ToString(), "");
            result = new Vector4();
            try
            {
                string[] strArray = str.Split(new char[] { vectorSpriter });
                if (strArray.Length != 4)
                {
                    return false;
                }
                result.x = float.Parse(strArray[0]);
                result.y = float.Parse(strArray[1]);
                result.z = float.Parse(strArray[2]);
                result.w = float.Parse(strArray[3]);
                return true;
            }
            catch (Exception e)
            {
                Debug.logger.LogException(e);
                return false;
            }
        }

        public static bool ParseQuaternion(string _inputString, out Quaternion result, char spriter = ',')
        {
            Vector4 vec = new Vector4();
            bool flag = ParseVector4(_inputString, out vec, spriter);
            result = new Quaternion(vec.x, vec.y, vec.z, vec.w);
            return flag;
        }

        public static bool ParseVector3(string _inputString, out Vector3 result, char spriter = ',')
        {
            string str = _inputString.Trim();
            str = str.Replace(FBracket1.ToString(), "");
            str = str.Replace(BBracket1.ToString(), "");
            result = new Vector3();
            try
            {
                string[] strArray = str.Split(new char[] { spriter });
                if (strArray.Length != 3)
                {
                    return false;
                }
                result.x = float.Parse(strArray[0]);
                result.y = float.Parse(strArray[1]);
                result.z = float.Parse(strArray[2]);
                return true;
            }
            catch (Exception e)
            {
                Debug.logger.LogException(e);
                return false;
            }
        }

        public static bool ParseVector2(string _inputString, out Vector2 result, char spriter = ',')
        {
            string str = _inputString.Trim();
            str = str.Replace(FBracket1.ToString(), "");
            str = str.Replace(BBracket1.ToString(), "");
            result = new Vector2();
            try
            {
                string[] strArray = str.Split(new char[] { spriter });
                if (strArray.Length != 2)
                {
                    return false;
                }
                result.x = float.Parse(strArray[0]);
                result.y = float.Parse(strArray[1]);
                return true;
            }
            catch (Exception e)
            {
                Debug.logger.LogException(e);
                return false;
            }
        }

        public static string ReplaceFirst(this string input, string oldValue, string newValue, int startAt = 0)
        {
            int index = input.IndexOf(oldValue, startAt);
            if (index < 0)
            {
                return input;
            }
            return (input.Substring(0, index) + newValue + input.Substring(index + oldValue.Length));
        }

        public static bool HasChinese(this string input)
        {
            return Regex.IsMatch(input, @"[\u4e00-\u9fa5]");
        }

        public static bool HasSpace(this string input)
        {
            return input.Contains(" ");
        }
    }

}
