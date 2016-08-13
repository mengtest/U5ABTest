using UnityEngine;
using System.Collections;
using System.Xml.Linq;
using ResetCore.Util;
using System.Collections.Generic;

namespace ResetCore.Xml
{
    public static class XElementEx
    {

        /// <summary>
        /// 获取当前节点下的子节点数量
        /// </summary>
        /// <param name="_el"></param>
        /// <returns></returns>
        public static int GetElementNum(this XElement _el)
        {
            if (!_el.HasElements) return 0;
            int num = 0;
            IEnumerator e = _el.Elements().GetEnumerator();
            while (e.MoveNext())
            {
                ++num;
            }
            return num;
        }

        /// <summary>
        /// 从节点中读取值
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="_el"></param>
        /// <param name="_defValue"></param>
        /// <returns></returns>
        public static T ReadValueFromElement<T>(this XElement _el, T _defValue = default(T))
        {
            return (T)StringEx.GetValue(_el.Value, typeof(T));
        }

        /// <summary>
        /// 从节点中读取Dictionary
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="_root"></param>
        /// <returns></returns>
        public static Dictionary<string, T> ReadDictionaryFromElement<T>(this XElement _root)
        {
            Dictionary<string, T> _dictionary = new Dictionary<string, T>();
            foreach (XElement el in _root.Elements())
            {
                if (_dictionary.ContainsKey(el.Name.ToString()))
                    Debug.Log("同一元素在XML中重复定义");
                _dictionary.Add(el.Name.ToString(), (T)StringEx.GetValue(el.Value, typeof(T)));
            }
            return _dictionary;
        }


    }

}

