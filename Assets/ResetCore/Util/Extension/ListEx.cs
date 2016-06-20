using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;

public static class ListEx {

	public static void ForEach<T>(this List<T> list, Action<int, T> act)
    {
        for(int i = 0; i < list.Count; i++)
        {
            act(i, list[i]);
        }
    }
}
