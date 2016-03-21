using UnityEngine;
using System.Collections;
using ResetCore.Util;

public class CoroutineHelper : MonoSingleton<CoroutineHelper> {

    public void DoCoroutine(IEnumerator coroutine)
    {
        StartCoroutine(coroutine);
    }
}
