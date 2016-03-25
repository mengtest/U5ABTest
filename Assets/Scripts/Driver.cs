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
        List<string> heihei = new List<string>() { "11", "22" };
        Dictionary<string, string> haha = new Dictionary<string, string>() { { "1", "haha" }, { "2", "haha" } };
        KeyValuePair<string, string> xixi = new KeyValuePair<string, string>("aaa", "aaa111");
        Debug.Log(StringEx.ConverToString(haha));
    }

    //private List<GameObject> cubes = new List<GameObject>();

    void Update()
    {
        
    }
    
}
