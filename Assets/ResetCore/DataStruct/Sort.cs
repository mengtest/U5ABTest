using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Text;

public static class Sort{

    /// <summary>
    /// 插入排序
    /// 将右边无序的数字插入左边的队列中
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="list"></param>
    public static void InsertionSort<T>(this List<T> list) where T : System.IComparable
    {
        int N = list.Count;
        for (int i = 1; i < N; i++)
        {
            for (int j = i; j > 0 && Less(list[j], list[j - 1]); j--)
            {
                list.Exch(j, j - 1);
            }
        }
    }

    /// <summary>
    /// 选择排序
    /// 右边找到最小的然后放到左边
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="list"></param>
    public static void SelectionSort<T>(this List<T> list) where T:System.IComparable
    {
        int N = list.Count;
        for (int i = 0; i < N; i++)
        {
            int min = 1;
            for (int j = i; j < N; j++)
            {
                if (Less(list[j], list[min]))
                {
                    min = j;
                }
                list.Exch(i, min);
            }
        }
    }

    public static bool Less<T>(T v, T w) where T:System.IComparable
    {
        return v.CompareTo(w) < 0;
    }

    public static void Exch<T>(this List<T> list, int i, int j)
    {
        T t = list[i];
        list[i] = list[j];
        list[j] = t;
    }

    public static void Show<T>(this List<T> list)
    {
        StringBuilder str = new StringBuilder();
        foreach (T v in list)
        {
            str.Append(v.ToString() + " ");
        }
        Debug.logger.Log(str.ToString());
    }

    public static bool IsSorted<T>(this List<T> list) where T:System.IComparable
    {
        for (int i = 1; i < list.Count; i++)
        {
            if (Less(list[i], list[i - 1])) return false;
        }
        return true;
    }
}
