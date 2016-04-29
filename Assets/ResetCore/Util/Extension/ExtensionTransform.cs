using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public static class ExtensionTransform {

    /// <summary>
    /// 将一个物体旋转到指向另一个物体
    /// </summary>
    /// <param name="from">旋转的物体</param>
    /// <param name="to">被指向的物体</param>
    /// <returns>旋转物体的最终欧拉角</returns>
    public static Quaternion FaceTo(this Transform from, Transform to)
    {
        float euler = Vector2.Angle(to.transform.position - from.transform.position, Vector2.up);
        if (to.position.x - from.position.x > 0)
        {
            return Quaternion.Euler(new Vector3(0, 0, -euler));
        }
        else
        {
            return Quaternion.Euler(new Vector3(0, 0, euler));
        }
    }

    /// <summary>
    /// 将一个向量旋转到指向另一个向量所需的角度（有符号）
    /// </summary>
    /// <param name="from">旋转的向量</param>
    /// <param name="to">目标向量</param>
    /// <returns>旋转向量所需的角度（有符号）</returns>
    public static float EulerRotateTo2D(this Vector3 from, Vector3 to)
    {
        float euler = Vector2.Angle(from, to);
        if (to.x - from.x > 0)
        {
            return -euler;
        }
        else
        {
            return euler;
        }
    }

    /// <summary>
    /// 获得所有子物体
    /// </summary>
    /// <param name="tran"></param>
    /// <returns></returns>
    public static List<Transform> GetAllChildren(this Transform tran)
    {
        List<Transform> children = new List<Transform>();

        for (int i = 0; i < tran.childCount; i++)
        {
            children.Add(tran.GetChild(i));
        }

        return children;
    }

    /// <summary>
    /// 对所有子物体做动作
    /// </summary>
    /// <param name="tran"></param>
    /// <param name="todo"></param>
    public static void DoToAllChildren(this Transform tran, System.Action<Transform> todo)
    {
        List<Transform> children = tran.GetAllChildren();

        foreach (Transform child in children)
        {
            todo(child);
        }
    }

    /// <summary>
    /// 删除所有子物体
    /// </summary>
    /// <param name="tran"></param>
    public static void DeleteAllChild(this Transform tran)
    {
        tran.DoToAllChildren((child) =>
        {
            GameObject.Destroy(child);
        });
    }
}
