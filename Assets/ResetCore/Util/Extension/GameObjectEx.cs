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

    public static void ResetTransform(this Transform tran)
    {
        tran.position = Vector3.zero;
        tran.eulerAngles = Vector3.zero;
        tran.localScale = Vector3.one;
        tran.gameObject.SetActive(true);
    }

    public static Vector3 NewRotateAround(this Transform tran, Vector3 pos, Vector3 euler)
    {
        Quaternion rotation = Quaternion.Euler(euler) * tran.localRotation;
        Vector3 newPosition = rotation * (tran.position - pos);
        return newPosition;
    }

   
}
