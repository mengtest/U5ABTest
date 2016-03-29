using UnityEngine;
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

        public static object GetValue(string value, System.Type type)
        {
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
                    if (value == "0")
                    {
                        return false;
                    }
                    return ((value == "1") ? ((object)1) : ((object)bool.Parse(value)));
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
                    Dictionary<string, string> dictionary = ParseMap(value, ':', ',');
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
                    List<string> list = ParseList(value, ',');
                    obj2 = type.GetConstructor(System.Type.EmptyTypes).Invoke(null);
                    foreach (string str in list)
                    {
                        obj3 = GetValue(str, type2);
                        type.GetMethod("Add").Invoke(obj2, new object[] { obj3 });
                    }
                    return obj2;
                }
            }
            Debug.logger.LogWarning("字符转换", "没有适合的转换类型，返回默认值");
            return null;
        }

        public static string ConverToString(object value)
        {
            System.Type type = value.GetType();
            if (type == null)
            {
                return "";
            }
            if (type == typeof(Vector3))
            {
                return ((Vector3)value).x + "," + ((Vector3)value).y + "," + ((Vector3)value).z;
            }
            if (type == typeof(Vector2))
            {
                return ((Vector2)value).x + "," + ((Vector2)value).y;
            }
            if (type == typeof(Vector4))
            {
                return ((Vector4)value).x + "," + ((Vector4)value).y + "," + ((Vector4)value).z + "," + ((Vector4)value).w;
            }
            if (type == typeof(Quaternion))
            {
                return ((Quaternion)value).x + "," + ((Quaternion)value).y + "," + ((Quaternion)value).z + "," + ((Quaternion)value).w;
            }
            if (type == typeof(Color))
            {
                return ((Color)value).r + "," + ((Color)value).g + "," + ((Color)value).b;
            }
            if (type.BaseType == typeof(Enum))
            {
                return Enum.GetName(type, value);
            }
            if (type.IsGenericType && (type.GetGenericTypeDefinition() == typeof(Dictionary<,>)))
            {

                MethodInfo getIe = type.GetMethod("GetEnumerator");
                object enumerator = getIe.Invoke(value, new object[0]);
                System.Type enumeratorType = enumerator.GetType();
                MethodInfo moveToNextMethod = enumeratorType.GetMethod("MoveNext");
                PropertyInfo current = enumeratorType.GetProperty("Current");

                StringBuilder stringBuilder = new StringBuilder();

                while (enumerator != null && (bool)moveToNextMethod.Invoke(enumerator, new object[0]))
                {
                    stringBuilder.Append("," + ConverToString(current.GetValue(enumerator, null)));
                }

                return stringBuilder.ToString().ReplaceFirst(",", "");

            }
            if (type.IsGenericType && (type.GetGenericTypeDefinition() == typeof(KeyValuePair<,>)))
            {
                Debug.Log("hahaha");

                object pairKey = type.GetProperty("Key").GetValue(value, null);
                object pairValue = type.GetProperty("Value").GetValue(value, null);

                string keyStr = ConverToString(pairKey);
                string valueStr = ConverToString(pairValue);

                return keyStr + ":" + valueStr;

            }
            if (type.IsGenericType && (type.GetGenericTypeDefinition() == typeof(List<>)))
            {
                int Count = (int)type.GetProperty("Count").GetValue(value, null);
                MethodInfo mget = type.GetMethod("get_Item", BindingFlags.Instance | BindingFlags.Public);

                StringBuilder stringBuilder = new StringBuilder();

                object item;
                string itemStr;

                for(int i = 0; i < Count - 1; i++){
                    item = mget.Invoke(value, new object[] { i });
                    itemStr = StringEx.ConverToString(item);
                    stringBuilder.Append(itemStr + ",");
                }
                item = mget.Invoke(value, new object[] { Count-1 });
                itemStr = StringEx.ConverToString(item);
                stringBuilder.Append(itemStr);

                return stringBuilder.ToString();
            }
            //Debug.logger.LogWarning("字符转换", type.Name + "没有适合的转换类型，返回默认值");
            return value.ToString();
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
            typeof(long), 
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
            typeof(Enum)
        };

        public static bool IsConvertableType(Type type)
        {
            return convertableTypes.Contains(type);
        }

        public static bool ParseColor(string _inputString, out Color result)
        {
            string str = _inputString.Trim();
            result = Color.clear;
            if (str.Length < 9)
            {
                return false;
            }
            try
            {
                string[] strArray = str.Split(new char[] { ',' });
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

        public static bool ParseVector4(string _inputString, out Vector4 result)
        {
            string str = _inputString.Trim();
            result = new Vector4();
            try
            {
                string[] strArray = str.Split(new char[] { ',' });
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

        public static bool ParseQuaternion(string _inputString, out Quaternion result)
        {
            Vector4 vec = new Vector4();
            bool flag = ParseVector4(_inputString, out vec);
            result = new Quaternion(vec.x, vec.y, vec.z, vec.w);
            return flag;
        }

        public static bool ParseVector3(string _inputString, out Vector3 result)
        {
            string str = _inputString.Trim();
            result = new Vector3();
            try
            {
                string[] strArray = str.Split(new char[] { ',' });
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

        public static bool ParseVector2(string _inputString, out Vector2 result)
        {
            string str = _inputString.Trim();
            result = new Vector2();
            try
            {
                string[] strArray = str.Split(new char[] { ',' });
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
