using UnityEngine;
using System.Collections;
using ResetCore.Event;
using System.Collections.Generic;
using System;
using ResetCore.Util;

namespace ResetCore.Event
{
    public class MonoEventDispatcher
    {

        public static Dictionary<object, EventController> monoEventControllerDict = new Dictionary<object, EventController>();
        public static EventController GetMonoController(object gameObject)
        {
            if (gameObject == null) return null;

            if (!monoEventControllerDict.ContainsKey(gameObject))
            {
                monoEventControllerDict.Add(gameObject, new EventController());
            }
            return monoEventControllerDict[gameObject];
        }
    }

    public static class MonoEventEx
    {
        /// <summary>
        /// 添加监听器
        /// </summary>
        /// <param name="bindObject"></param>
        /// <param name="eventType"></param>
        /// <param name="handler"></param>
        public static void AddEventListener(this GameObject bindObject, string eventType, Action handler)
        {
            MonoEventDispatcher.GetMonoController(bindObject).AddEventListener(eventType, handler);
            bindObject.GetOrCreateComponent<MonoEventCleanUp>();
        }
        /// <summary>
        /// 添加监听器
        /// </summary>
        /// <param name="bindObject"></param>
        /// <param name="eventType"></param>
        /// <param name="handler"></param>
        public static void AddEventListener<T>(this GameObject bindObject, string eventType, Action<T> handler)
        {
            MonoEventDispatcher.GetMonoController(bindObject).AddEventListener<T>(eventType, handler);
            bindObject.GetOrCreateComponent<MonoEventCleanUp>();
        }
        /// <summary>
        /// 添加监听器
        /// </summary>
        /// <param name="bindObject"></param>
        /// <param name="eventType"></param>
        /// <param name="handler"></param>
        public static void AddEventListener<T, U>(this GameObject bindObject, string eventType, Action<T, U> handler)
        {
            MonoEventDispatcher.GetMonoController(bindObject).AddEventListener<T, U>(eventType, handler);
            bindObject.GetOrCreateComponent<MonoEventCleanUp>();
        }
        /// <summary>
        /// 添加监听器
        /// </summary>
        /// <param name="bindObject"></param>
        /// <param name="eventType"></param>
        /// <param name="handler"></param>
        public static void AddEventListener<T, U, V>(this GameObject bindObject, string eventType, Action<T, U, V> handler)
        {
            MonoEventDispatcher.GetMonoController(bindObject).AddEventListener<T, U, V>(eventType, handler);
            bindObject.GetOrCreateComponent<MonoEventCleanUp>();
        }
        /// <summary>
        /// 添加监听器
        /// </summary>
        /// <param name="bindObject"></param>
        /// <param name="eventType"></param>
        /// <param name="handler"></param>
        public static void AddEventListener<T, U, V, W>(this GameObject bindObject, string eventType, Action<T, U, V, W> handler)
        {
            MonoEventDispatcher.GetMonoController(bindObject).AddEventListener<T, U, V, W>(eventType, handler);
            bindObject.GetOrCreateComponent<MonoEventCleanUp>();
        }
    }

    public class UGUIEventsEx
    {
        
    }

}

