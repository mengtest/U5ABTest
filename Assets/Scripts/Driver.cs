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
    public Transform Target;
	// Use this for initialization
    int i = 0;
	void Start () 
    {
        //Quaternion rotation = Quaternion.Euler(0f, 30f, 0f) * Target.rotation;
        //Vector3 newPos = rotation * new Vector3(10f, 0f, 0f);
        //Debug.DrawLine(newPos, Vector3.zero, Color.red);
        //Debug.Log("newpos " + newPos + " nowpos " + Target.position + " distance " + Vector3.Distance(newPos, Target.position));
        //GameObject go = GameObject.CreatePrimitive(PrimitiveType.Capsule);
        //go.transform.position = newPos;
        //UIManager.Instance.ShowUI();

        CoroutineTaskManager.CoroutineTask task = CoroutineTaskManager.Instance.LoopTodoByTime(() => {
            i++;
            Debug.Log(i);
        }, 1, 10);

        CoroutineTaskManager.Instance.WaitSecondTodo(() =>
        {
            Debug.Log("停止任务");
            task.Pause();

            CoroutineTaskManager.Instance.WaitSecondTodo(() => 
            {
                Debug.Log("继续任务");
                task.Unpause();
            }, 2);
            
        }, 5);

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
