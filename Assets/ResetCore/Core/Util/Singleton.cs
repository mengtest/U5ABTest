using UnityEngine;
using System.Collections;
using System;
using System.Reflection;
using System.Diagnostics;

namespace ResetCore.Util
{
    public abstract class Singleton<T> where T : Singleton<T>, new()
    {

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

