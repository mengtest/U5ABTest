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
        //GameObject go = ResourcesLoaderHelper.Instance.LoadAndGetInstance("Cube.prefab");
        //go.transform.position = Vector3.zero;
        //go = ResourcesLoaderHelper.Instance.LoadAndGetInstance("Cube.prefab");
        //go.transform.position = new Vector3(1, 1, 1);

        //NetTaskDispatcher.instance.AddNetPostTask(new ExampleNetTask(null));

        //XmlSceneBuilder.Instance.SceneBuilder("TestLoadScene", (bo) => { });
        //Debug.Log(ItemData.dataMap[1].description);
	}

    public override void Init()
    {
        base.Init();
    }

}
