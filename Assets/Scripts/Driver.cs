using UnityEngine;
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

//using ResetCore.Data.GameDatas;

public class Driver : MonoSingleton<Driver> {

    
    void Awake()
    {
        Init();
    }
    public Transform Target;
	// Use this for initialization
    int i = 0;
	void Start () 
    {
        string XmlPath = Application.dataPath + "/SteamingAsset/test.xml";

        XDocument xDoc = XDocument.Load(XmlPath);

        //XMLWriter.WriteValueToXML<string>(XmlPath, new string[] { "heihei" }, "asdasdasd");
        //xDoc.WriteValueToXML<string>(XmlPath, new string[] { "heihei" }, "aaaaa");

        List<string> testList = new List<string>(){
            "asdasdasdasd",
            "zzzzzz",
            "ssssss",
            "vvvvvvvv"
        };

        XMLWriter.WriteListToXML<string>(XmlPath, new string[] { "hoho" }, testList);
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
