﻿using UnityEngine;
using System.Collections;
using ResetCore.Asset;
using ResetCore.Util;
using System.Collections.Generic;
using ResetCore.Data;
using ResetCore.Data.GameDatas;
using System;
using System.IO;
using ResetCore.UGUI;
using ResetCore.AOP;
using ResetCore.Event;
using ResetCore.Xml;
using System.Xml.Linq;
using ResetCore.MySQL;


//using ResetCore.Data.GameDatas;

public class Driver : MonoSingleton<Driver> {

    
    void Awake()
    {
        Init();
    }
	// Use this for initialization
	void Start () 
    {
        //ReadXlsxData.ParseXlsx.ReadExcelFile("asdasd");
	}

    public override void Init()
    {
        base.Init();
        
    }

    //private List<GameObject> cubes = new List<GameObject>();

    void Update()
    {
        //Target.localPosition = Target.NewRotateAround(Vector3.zero, new Vector3(0, 1 * Time.deltaTime, 0));
        //Target.LookAt(Vector3.zero);
        //Target.NewLookAt(Vector3.zero);
        //Target.eulerAngles = Target.NewLookAt(Vector3.zero, new Vector3(0, 5, 0));
        //Debug.DrawLine(Target.position, Vector3.zero, Color.red);
    }
    
    
}
