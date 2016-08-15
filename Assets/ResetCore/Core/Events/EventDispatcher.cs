using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;

namespace ResetCore.Event
{
    public class EventDispatcher
    {
        private static EventController m_eventController = new EventController();
        //全局监听器
        public static Dictionary<string, Delegate> TheRouter
        {
            get
            {
                return m_eventController.TheRouter;
            }
        }

        #region 添加监听器(使用物体
        /// <summary>
        /// 添加监听器
        /// </summary>
        /// <param name="eventType">事件类型</param>
        /// <param name="handler">处理行为</param>
        /// <param name="bindObject">绑定对象</param>
        public static void AddEventListener(string eventType, Action handler, object bindObject = null)
        {
            if (bindObject == null)
            {
                m_eventController.AddEventListener(eventType, handler);
            }
            else
            {
                MonoEventDispatcher.GetMonoController(bindObject).AddEventListener(eventType, handler);
            }

        }
        /// <summary>
        /// 添加监听器
        /// </summary>
        /// <param name="eventType">事件类型</param>
        /// <param name="handler">处理行为</param>
        /// <param name="bindObject">绑定对象</param>
        public static void AddEventListener<T>(string eventType, Action<T> handler, object bindObject = null)
        {
            if (bindObject == null)
            {
                m_eventController.AddEventListener<T>(eventType, handler);
            }
            else
            {
                MonoEventDispatcher.GetMonoController(bindObject).AddEventListener<T>(eventType, handler);
            }
            
        }
        /// <summary>
        /// 添加监听器
        /// </summary>
        /// <param name="eventType">事件类型</param>
        /// <param name="handler">处理行为</param>
        /// <param name="bindObject">绑定对象</param>
        public static void AddEventListener<T, U>(string eventType, Action<T, U> handler, object bindObject = null)
        {
            if (bindObject == null)
            {
                m_eventController.AddEventListener<T, U>(eventType, handler);
            }
            else
            {
                MonoEventDispatcher.GetMonoController(bindObject).AddEventListener<T, U>(eventType, handler);
            }
            
        }
        /// <summary>
        /// 添加监听器
        /// </summary>
        /// <param name="eventType">事件类型</param>
        /// <param name="handler">处理行为</param>
        /// <param name="bindObject">绑定对象</param>
        public static void AddEventListener<T, U, V>(string eventType, Action<T, U, V> handler, object bindObject = null)
        {
            if (bindObject == null)
            {
                m_eventController.AddEventListener<T, U, V>(eventType, handler);
            }
            else
            {
                MonoEventDispatcher.GetMonoController(bindObject).AddEventListener<T, U, V>(eventType, handler);
            }
            
        }
        /// <summary>
        /// 添加监听器
        /// </summary>
        /// <param name="eventType">事件类型</param>
        /// <param name="handler">处理行为</param>
        /// <param name="bindObject">绑定对象</param>
        public static void AddEventListener<T, U, V, W>(string eventType, Action<T, U, V, W> handler, object bindObject = null)
        {
            if (bindObject == null)
            {
                m_eventController.AddEventListener<T, U, V, W>(eventType, handler);
            }
            else
            {
                MonoEventDispatcher.GetMonoController(bindObject).AddEventListener<T, U, V, W>(eventType, handler);
            }
        }
        #endregion //添加监听器

        #region 监听器工具
        /// <summary>
        /// 清理监听器
        /// </summary>
        /// <param name="bindObject">绑定对象</param>
        public static void Cleanup(object bindObject = null)
        {
            if (bindObject == null)
            {
                m_eventController.CleanUp();
            }
            else
            {
                MonoEventDispatcher.GetMonoController(bindObject).CleanUp();
            }
            
        }


        public static void MarkAsPermanent(string eventType, object bindObject = null)
        {
            if (bindObject == null)
            {
                m_eventController.MarkAsPermanent(eventType);
            }
            else
            {
                MonoEventDispatcher.GetMonoController(bindObject).MarkAsPermanent(eventType);
            }
            
        }
        #endregion //监听器工具

        #region 移除监听器
        /// <summary>
        /// 移除监听器
        /// </summary>
        /// <param name="eventType">事件类型</param>
        /// <param name="handler">处理行为</param>
        /// <param name="bindObject">绑定对象</param>
        public static void RemoveEventListener(string eventType, Action handler, object bindObject = null)
        {
            if (bindObject == null)
            {
                m_eventController.RemoveEventListener(eventType, handler);
            }
            else
            {
                MonoEventDispatcher.GetMonoController(bindObject).RemoveEventListener(eventType, handler);
            }
        }
        /// <summary>
        /// 移除监听器
        /// </summary>
        /// <param name="eventType">事件类型</param>
        /// <param name="handler">处理行为</param>
        /// <param name="bindObject">绑定对象</param>
        public static void RemoveEventListener<T>(string eventType, Action<T> handler, object bindObject = null)
        {
            if (bindObject == null)
            {
                m_eventController.RemoveEventListener<T>(eventType, handler);
            }
            else
            {
                MonoEventDispatcher.GetMonoController(bindObject).RemoveEventListener<T>(eventType, handler);
            }
                
        }
        /// <summary>
        /// 移除监听器
        /// </summary>
        /// <param name="eventType">事件类型</param>
        /// <param name="handler">处理行为</param>
        /// <param name="bindObject">绑定对象</param>
        public static void RemoveEventListener<T, U>(string eventType, Action<T, U> handler, object bindObject = null)
        {
            if(bindObject == null)
            {
                m_eventController.RemoveEventListener<T, U>(eventType, handler);
            }
            else
            {
                MonoEventDispatcher.GetMonoController(bindObject).RemoveEventListener<T, U>(eventType, handler);
            }
               
        }
        /// <summary>
        /// 移除监听器
        /// </summary>
        /// <param name="eventType">事件类型</param>
        /// <param name="handler">处理行为</param>
        /// <param name="bindObject">绑定对象</param>
        public static void RemoveEventListener<T, U, V>(string eventType, Action<T, U, V> handler, object bindObject = null)
        {
            if (bindObject == null)
            {
                m_eventController.RemoveEventListener<T, U, V>(eventType, handler);
            }
            else
            {
                MonoEventDispatcher.GetMonoController(bindObject).RemoveEventListener<T, U, V>(eventType, handler);
            }
        }
        /// <summary>
        /// 移除监听器
        /// </summary>
        /// <param name="eventType">事件类型</param>
        /// <param name="handler">处理行为</param>
        /// <param name="bindObject">绑定对象</param>
        public static void RemoveEventListener<T, U, V, W>(string eventType, Action<T, U, V, W> handler, object bindObject = null)
        {
            if (bindObject == null)
            {
                m_eventController.RemoveEventListener<T, U, V, W>(eventType, handler);
            }
            else
            {
                MonoEventDispatcher.GetMonoController(bindObject).RemoveEventListener<T, U, V, W>(eventType, handler);
            }
        }
        #endregion //移除监听器、

        #region 触发事件
        /// <summary>
        /// 触发行为
        /// </summary>
        /// <param name="eventType">事件类型</param>
        /// <param name="triggerObject">触发对象</param>
        public static void TriggerEvent(string eventType, object triggerObject = null)
        {
            if (triggerObject == null)
            {
                m_eventController.TriggerEvent(eventType);
                foreach (EventController controller in MonoEventDispatcher.monoEventControllerDict.Values)
                {
                    controller.TriggerEvent(eventType);
                }
            }
            else
            {
                MonoEventDispatcher.GetMonoController(triggerObject).TriggerEvent(eventType);
            }
            
        }
        /// <summary>
        /// 触发行为
        /// </summary>
        /// <param name="eventType">事件类型</param>
        /// <param name="triggerObject">触发对象</param>
        public static void TriggerEvent<T>(string eventType, T arg1, object triggerObject = null)
        {
            if (triggerObject == null)
            {
                m_eventController.TriggerEvent<T>(eventType, arg1);
                foreach (EventController controller in MonoEventDispatcher.monoEventControllerDict.Values)
                {
                    controller.TriggerEvent<T>(eventType, arg1);
                }
            }
            else
            {
                MonoEventDispatcher.GetMonoController(triggerObject).TriggerEvent<T>(eventType, arg1);
            }
            
        }
        /// <summary>
        /// 触发行为
        /// </summary>
        /// <param name="eventType">事件类型</param>
        /// <param name="triggerObject">触发对象</param>
        public static void TriggerEvent<T, U>(string eventType, T arg1, U arg2, object triggerObject = null)
        {
            if (triggerObject == null)
            {
                m_eventController.TriggerEvent<T, U>(eventType, arg1, arg2);
                foreach (EventController controller in MonoEventDispatcher.monoEventControllerDict.Values)
                {
                    controller.TriggerEvent<T, U>(eventType, arg1, arg2);
                }
            }
            else
            {
                MonoEventDispatcher.GetMonoController(triggerObject).TriggerEvent<T, U>(eventType, arg1, arg2);
            }
            
        }
        /// <summary>
        /// 触发行为
        /// </summary>
        /// <param name="eventType">事件类型</param>
        /// <param name="triggerObject">触发对象</param>
        public static void TriggerEvent<T, U, V>(string eventType, T arg1, U arg2, V arg3, object triggerObject = null)
        {
            if (triggerObject == null)
            {
                m_eventController.TriggerEvent<T, U, V>(eventType, arg1, arg2, arg3);
                foreach (EventController controller in MonoEventDispatcher.monoEventControllerDict.Values)
                {
                    controller.TriggerEvent<T, U, V>(eventType, arg1, arg2, arg3);
                }
            }
            else
            {
                MonoEventDispatcher.GetMonoController(triggerObject).TriggerEvent<T, U, V>(eventType, arg1, arg2, arg3);
            }
            
        }
        /// <summary>
        /// 触发行为
        /// </summary>
        /// <param name="eventType">事件类型</param>
        /// <param name="triggerObject">触发对象</param>
        public static void TriggerEvent<T, U, V, W>(string eventType, T arg1, U arg2, V arg3, W arg4, object triggerObject = null)
        {
            if (triggerObject == null)
            {
                m_eventController.TriggerEvent<T, U, V, W>(eventType, arg1, arg2, arg3, arg4);
                foreach (EventController controller in MonoEventDispatcher.monoEventControllerDict.Values)
                {
                    controller.TriggerEvent<T, U, V, W>(eventType, arg1, arg2, arg3, arg4);
                }
            }
            else
            {
                MonoEventDispatcher.GetMonoController(triggerObject).TriggerEvent<T, U, V, W>(eventType, arg1, arg2, arg3, arg4);
            }

        }

        #endregion //触发事件
    }

}

