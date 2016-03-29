using UnityEngine;
using System.Collections;
using System;

namespace ResetCore.Util
{
    public class Singleton<T> where T : Singleton<T>, new()
    {

        private static T Instance = null;
        public static T instance
        {
            get
            {
                if (Instance == null)
                {
                    Instance = typeof(T).GetConstructor(new Type[] { }).Invoke(new object[] { }) as T;
                }
                return Instance;
            }
        }

        protected virtual void Init()
        {

        }
    }

}
