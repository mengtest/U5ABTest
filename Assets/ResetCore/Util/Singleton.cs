using UnityEngine;
using System.Collections;
using System;
using System.Reflection;
using System.Diagnostics;

namespace ResetCore.Util
{
    public abstract class Singleton<T> where T : Singleton<T>, new()
    {
        protected Singleton()
        {
            if(typeof(T).GetConstructors(BindingFlags.Instance|BindingFlags.NonPublic).Length == 0)
            {
                throw new Exception("单例模式不允许使用公共构造函数构造");
            }
        }
        private static T Instance = null;
        public static T instance
        {
            get
            {
                if (Instance == null)
                {
                    Instance = typeof(T).GetConstructor(new Type[] { }).Invoke(new object[] { }) as T;
                    Instance.Init();
                }
                return Instance;
            }
        }

        public virtual void Init() { }

    }

}

