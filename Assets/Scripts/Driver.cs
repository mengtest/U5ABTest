using UnityEngine;
using System.Collections;
using ResetCore.Asset;
using ResetCore.Util;
using System.Collections.Generic;
using ResetCore.Data;
using ResetCore.Data.GameDatas;
using System;
using System.IO;
//using ResetCore.Data.GameDatas;

public class Driver : MonoSingleton<Driver> {

    
    void Awake()
    {
        Init();
    }

	// Use this for initialization
	void Start () 
    {
        //DownloadManager.instance.AsynDownLoadText("http://localhost/ResetResources/heihei.txt", handle, () => { });
        string res = DownloadManager.instance.DownLoadText("http://localhost/ResetResources/heihei.txt");
        Debug.logger.Log(res);
        //DownloadManager.instance.DownloadFileBreakPoint("http://localhost/ResetResources/book.pdf", Path.Combine(Application.dataPath, "book.pdf"));
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
