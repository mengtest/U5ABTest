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
using Unity.Linq;


//using ResetCore.Data.GameDatas;

public class Driver : MonoSingleton<Driver> {

    class tt
    {
        public int a = 0;
    }
    
    void Awake()
    {
        Init();
    }
    // Use this for initialization
    void Start()
    {
        //DownloadManager.instance.AddNewDownloadTask(Path.Combine(PathConfig.wwwPath, "a.pdf"), @"C:\Users\hzcm1\Desktop\test.pdf", null, (x) =>
        //{
        //    Debug.logger.Log(x.ToString());
        //}, () =>
        //{
        //    Debug.logger.Log("finish");
        //})
        //.CheckDownLoadList();
        tt asd = new tt();
        asd.a = 1;
        Debug.Log(asd.a);
        asd.BindField<tt, int>("asds", "a");

        DataBind.ChangeData("asds", 5);
        Debug.Log(asd.a);
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
