using UnityEngine.UI;
using ResetCore.Event;

namespace ResetCore.Event
{
   
    public static class DataBind
    {
        /// <summary>
        /// 绑定数据到Text
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="V"></typeparam>
        /// <param name="text"></param>
        /// <param name="dataId"></param>
        /// <param name="func"></param>
        public static void BindData<T, V>(this Text text, string dataId, System.Func<T, V> func = null)
        {
            EventDispatcher.AddEventListener<T>(dataId, (data) =>
            {
                if (func == null)
                {
                    text.text = data.ToString();
                }
                else
                {
                    text.text = func(data).ToString();
                }
                
            }, text.gameObject);
        }

        /// <summary>
        /// 绑定数据到任意对象
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj"></param>
        /// <param name="dataId"></param>
        /// <param name="act"></param>
        public static void BindData<T>(this object obj, string dataId, System.Action<T> act)
        {
            EventDispatcher.AddEventListener<T>(dataId, (data) =>
            {
                act(data);
            }, obj);
        }

        /// <summary>
        /// 改变数据
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="dataId"></param>
        /// <param name="value"></param>
        public static void ChangeData<T>(string dataId, T value)
        {
            EventDispatcher.TriggerEvent<T>(dataId, value);
        }

    }

}
