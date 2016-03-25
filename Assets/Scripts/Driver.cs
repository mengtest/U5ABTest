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
        GameObject go1 = ResourcesLoaderHelper.Instance.LoadAndGetInstance("Cube.prefab");
        
        GameObject go2 = ResourcesLoaderHelper.Instance.LoadAndGetInstance("Cube.prefab");

        CoroutineTaskManager.Instance.WaitSecondTodo(() => 
        {
            
        }, 2);
    }

    //private List<GameObject> cubes = new List<GameObject>();

    void Update()
    {
        
    }
    
}
