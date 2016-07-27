using UnityEngine;
using System.Collections;
using ResetCore.Asset;
using ResetCore.Util;
using System.Collections.Generic;
using ResetCore.Data;
using System;
using System.IO;
using ResetCore.AOP;
using ResetCore.Event;
using ResetCore.Xml;
using System.Xml.Linq;
using ResetCore.MySQL;
using ResetCore.NetPost;
using ResetCore.UGUI;
using UnityEngine.UI;
using ResetCore.Lua;
using ResetCore.Data.GameDatas.Xml;


//using ResetCore.Data.GameDatas;

public class Driver : MonoSingleton<Driver> {


    
    void Awake()
    {
        Init();
    }
	// Use this for initialization
    void Start()
    {

    }

    public override void Init()
    {
        base.Init();
    }

    //private List<GameObject> cubes = new List<GameObject>();

    void Update()
    {
        
        
    }

    
}
