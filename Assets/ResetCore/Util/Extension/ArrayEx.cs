using UnityEngine;
using System.Collections;
using System;

public static class ArrayEx {

	public static void Foreach<T>(this Array array, Action<int, T> act)
    {
        for(int i = 0; i < array.Length; i++)
        {
            act(i, (T)array.GetValue(i));
        }
    }

    public static void Foreach<T>(this Array array, Action<T> act)
    {
        for (int i = 0; i < array.Length; i++)
        {
            act((T)array.GetValue(i));
        }
    }

}
