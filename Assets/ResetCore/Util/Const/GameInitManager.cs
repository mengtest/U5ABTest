using UnityEngine;
using System.Collections;
using ResetCore.Util;
using ResetCore.Asset;
using ResetCore.Lua;

namespace ResetCore.Util
{
    public class GameInitManager : MonoBehaviour
    {
        static System.Type[] initCompTypes = new System.Type[]{
            //typeof(GameInitManager), typeof(CoroutineTaskManager), typeof(ResourcesLoaderHelper), typeof(ModManager)
        };

        [RuntimeInitializeOnLoadMethod]
        static void Initialize()
        {
            GameObject initObject = new GameObject("Driver2", initCompTypes);
            GameObject.DontDestroyOnLoad(initObject);
            //initObject.hideFlags = HideFlags.HideInHierarchy;
            Debug.Log("RuntimeInitializeOnLoadMethod");
        }
    }

}
