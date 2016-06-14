using UnityEngine;
using System.Collections;
using ResetCore.Asset;
using ResetCore.Util;
using System.Collections.Generic;
using ResetCore.Data;
using ResetCore.Data.GameDatas;
using System;
using System.IO;
using ResetCore.AOP;
using ResetCore.Event;
using ResetCore.Xml;
using System.Xml.Linq;
using ResetCore.MySQL;
using ResetCore.NetPost;
using ResetCore.NGUI;


//using ResetCore.Data.GameDatas;

public class Driver : MonoSingleton<Driver> {

    
    void Awake()
    {
        Init();
    }
	// Use this for initialization
    void Start()
    {
        //ReadXlsxData.ParseXlsx.ReadExcelFile("asdasd");
        //MySQLManager.OpenSql();
        //MySQLManager.ExecuteQuery();
        //MySQLManager.Close();

        //EventDispatcher.AddEventListener("asdasd", () => { }, gameObject);

        //UIManager.Instance.ShowUI(UIConst.UIName.TestPanel);
        //var person = new Person
        //{
        //    Id = 1,
        //    Name = "First",
        //    Address = new Address { Line1 = "Line1", Line2 = "Line2" }
        //};
        //if (!Directory.Exists(Application.streamingAssetsPath))
        //{
        //    Directory.CreateDirectory(Application.streamingAssetsPath);
        //}
        //using (var file = System.IO.File.Create(Application.streamingAssetsPath + "/Person.bin"))
        //{
        //    ProtoBuf.Serializer.Serialize(file, person);
        //}
        List<int> test = new List<int>()
        {
            1, 2, 3, 4, 5, 6, 7, 8, 9, 10
        };


        //ActionQueue queue = new ActionQueue();
        //AQAopManager.Aop
        //    .Log("haha", "asdasd")
        //    .ShowUseTime()
        //    .Work(() =>
        //    {
        //        Debug.logger.Log("zhongjian");
        //    })
        //    .Submit();

        AQAopManager.Aop.Work(() =>
        {
            Debug.logger.Log("zhongjian");
        }).Work(() =>
        {
            Debug.logger.Log("zhongjian");
        }).WorkAfterTimes(() =>
        {
            Debug.logger.Log("zhongjian after 2 sec");
        }, 2)
        .Submit();
           
    }

    public override void Init()
    {
        base.Init();
        
    }

    //private List<GameObject> cubes = new List<GameObject>();

    void Update()
    {
        //Debug.logger.Log(www.progress);
        //Target.localPosition = Target.NewRotateAround(Vector3.zero, new Vector3(0, 1 * Time.deltaTime, 0));
        //Target.LookAt(Vector3.zero);
        //Target.NewLookAt(Vector3.zero);
        //Target.eulerAngles = Target.NewLookAt(Vector3.zero, new Vector3(0, 5, 0));
        //Debug.DrawLine(Target.position, Vector3.zero, Color.red);
    }
    
    
}
