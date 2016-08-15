using UnityEngine;
using System.Collections;

namespace ResetCore.Util
{
    public static class ComponentEx
    {

        public static T GetOrCreateComponent<T>(this GameObject go) where T : Component
        {
            T comp = go.GetComponent<T>();
            if (comp == null)
            {
                comp = go.AddComponent<T>();
            }
            return comp;
        }
    }

}
