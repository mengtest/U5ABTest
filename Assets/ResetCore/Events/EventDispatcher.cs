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

        public static void AddEventListener(string eventType, Action handler, GameObject bindObject = null)
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

        public static void AddEventListener<T>(string eventType, Action<T> handler, GameObject bindObject = null)
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

        public static void AddEventListener<T, U>(string eventType, Action<T, U> handler, GameObject bindObject = null)
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

        public static void AddEventListener<T, U, V>(string eventType, Action<T, U, V> handler, GameObject bindObject = null)
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

        public static void AddEventListener<T, U, V, W>(string eventType, Action<T, U, V, W> handler, GameObject bindObject = null)
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

        public static void Cleanup(GameObject bindObject = null)
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


        public static void MarkAsPermanent(string eventType, GameObject bindObject = null)
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

        public static void RemoveEventListener(string eventType, Action handler, GameObject bindObject = null)
        {
            if (bindObject == null)
            {
                m_eventController.RemoveEventListener(eventType, handler);
            }
            else
                MonoEventDispatcher.GetMonoController(bindObject).RemoveEventListener(eventType, handler);
        }

        public static void RemoveEventListener<T>(string eventType, Action<T> handler, GameObject bindObject = null)
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

        public static void RemoveEventListener<T, U>(string eventType, Action<T, U> handler, GameObject bindObject = null)
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

        public static void RemoveEventListener<T, U, V>(string eventType, Action<T, U, V> handler, GameObject bindObject = null)
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

        public static void RemoveEventListener<T, U, V, W>(string eventType, Action<T, U, V, W> handler, GameObject bindObject = null)
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

        public static void TriggerEvent(string eventType, GameObject triggerObject = null)
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

        public static void TriggerEvent<T>(string eventType, T arg1, GameObject triggerObject = null)
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

        public static void TriggerEvent<T, U>(string eventType, T arg1, U arg2, GameObject triggerObject = null)
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

        public static void TriggerEvent<T, U, V>(string eventType, T arg1, U arg2, V arg3, GameObject triggerObject = null)
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

        public static void TriggerEvent<T, U, V, W>(string eventType, T arg1, U arg2, V arg3, W arg4, GameObject triggerObject = null)
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

        
    }

}
