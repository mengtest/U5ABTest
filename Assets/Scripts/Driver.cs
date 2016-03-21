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
        
	}

    public override void Init()
    {
        base.Init();
    }

    //private List<GameObject> cubes = new List<GameObject>();

    void Update()
    {
        //if (Input.GetKeyDown(KeyCode.UpArrow))
        //{
        //    GameObject cube = ObjectPool.Instance.CreateOrFindGameObject("Cube");
        //    cubes.Add(cube);
        //}
        //if (Input.GetKeyDown(KeyCode.DownArrow))
        //{
        //    GameObject cube = GameObject.Find("Cube(Clone)");
        //    ObjectPool.Instance.HideOrDestroyObject(cube);
        //}
        //if (Input.GetKeyDown(KeyCode.LeftArrow))
        //{
        //    GameObject cube = GameObject.Find("Cube(Clone)");
        //    ObjectPool.Instance.AddObjectToPool(cube);
        //}
        //if (Input.GetKeyDown(KeyCode.RightArrow))
        //{
        //    GameObject cube = GameObject.Find("Cube(Clone)");
        //    ObjectPool.Instance.AddObjectToPool(cube, "Cube");
        //}
    }

}
