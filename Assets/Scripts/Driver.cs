using UnityEngine;
using System.Collections;
using ResetCore.Asset;
using ResetCore.Util;
using System.Collections.Generic;
using ResetCore.Data;
using ResetCore.Data.GameDatas;
//using ResetCore.Data.GameDatas;

public class Driver : MonoSingleton<Driver> {


    void Awake()
    {
        Init();
    }

	// Use this for initialization
	void Start () {
        Debug.Log(0);
        CoroutineHelper.Instance.DoCoroutine(Do());
	}

    public override void Init()
    {
        base.Init();
    }

    //private List<GameObject> cubes = new List<GameObject>();

    void Update()
    {
        
    }
    
    IEnumerator Do()
    {
        Debug.Log(0.1f);
        yield return new WaitForSeconds(3);
        Debug.Log(0.1f);
    }

}
