using UnityEngine;
using System.Collections;
#if UNITY_EDITOR
using UnityEditor;
#endif

public static class GameObjectEx {

    public static void ResetTransform(this GameObject go)
    {
        go.transform.position = Vector3.zero;
        go.transform.eulerAngles = Vector3.zero;
        go.transform.localScale = Vector3.one;
        go.SetActive(true);
    }

#if UNITY_EDITOR
    

#endif

}
