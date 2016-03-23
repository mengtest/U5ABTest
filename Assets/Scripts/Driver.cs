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
	void Start () 
    {

	}

    public override void Init()
    {
        base.Init();
        Debug.Log("laowulaowulaowu");
        System.Action act = () =>
        {
            Debug.Log("heihei");
        };
        CoroutineTaskManager.Instance.WaitSecondTodo(act, 3);
    }

    //private List<GameObject> cubes = new List<GameObject>();

    void Update()
    {
        
    }
    
}
