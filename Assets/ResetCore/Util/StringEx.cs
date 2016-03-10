using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using System.Text.RegularExpressions;

public static class StringEx {

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
        if(fileName.Contains("."))
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
            if (type == typeof(Vector3))
            {
                Vector3 vector;
                ParseVector3(value, out vector);
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
        return null;
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

    public static bool ParseQuaternion(string _inputString, out Quaternion result)
    {
        string str = _inputString.Trim();
        result = new Quaternion();
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
            result.x = float.Parse(strArray[0]);
            result.y = float.Parse(strArray[1]);
            result.z = float.Parse(strArray[2]);
            result.w = float.Parse(strArray[3]);
            return true;
        }
        catch (Exception)
        {
            return false;
        }
    }

    public static bool ParseVector3(string _inputString, out Vector3 result)
    {
        string str = _inputString.Trim();
        result = new Vector3();
        if (str.Length < 7)
        {
            return false;
        }
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
        catch (Exception)
        {
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
