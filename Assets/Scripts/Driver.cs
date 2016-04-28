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
        //string XmlPath = Application.dataPath + "/SteamingAsset/test.xml";

        
        ////XMLWriter.WriteValueToXML<string>(XmlPath, new string[] { "heihei" }, "asdasdasd");
        ////xDoc.WriteValueToXML<string>(XmlPath, new string[] { "heihei" }, "aaaaa");

        //List<string> testList = new List<string>(){
        //    "asdasdasdasd",
        //    "zzzzzz",
        //    "ssssss",
        //    "vvvvvvvv"
        //};

        //Dictionary<string, int> testDict = new Dictionary<string, int>()
        //{
        //    {"asdasd", 1},
        //    {"aszxc", 2},
        //    {"aqwed", 3},
        //    {"aswerd", 4},
        //};

        //XMLWriter.Open(XmlPath)
        //    .WriteList<string>(new string[] { "asdasd" }, testList)
        //    .WriteDictionary<int>(new string[] { "testDict" }, testDict)
        //    .Submit(XmlPath);

        string host = "localhost";  
	    //如果是局域网，那么写上本机的局域网IP
	    //static string host = "192.168.1.106";  
        string id = "root";
        string pwd = "vgvgvvvqazwsx123";
        string database = "student";


        MySQLManager.OpenSql(host, database, id, pwd);
        MySQLManager.ExecuteQuery("asdasd");
        MySQLManager.Close();
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
