using UnityEngine;
using System.Collections;

public class Driver : MonoBehaviour {

    private static Driver Instance;
    public static Driver instance
    {
        get { return Instance; }
    }

    void Awake()
    {
        Instance = this;
        Init();
    }

	// Use this for initialization
	void Start () {
        //Resources.Load("Cube");
        //resourcesLoaderHelper.loader.LoadResource("New Material.mat", (ob) =>
        //{
        //    resourcesLoaderHelper.loader.LoadResource("Cube.prefab", (obj) =>
        //    {
        //        GameObject go = GameObject.Instantiate(obj) as GameObject;
        //        go.transform.position = Vector3.zero;

        //    });
        //});

        GameObject go = ResourcesLoaderHelper.Instance.LoadAndGetInstance("Cube.prefab");
        go.transform.position = Vector3.zero;
        go = ResourcesLoaderHelper.Instance.LoadAndGetInstance("Cube.prefab");
        go.transform.position = new Vector3(1, 1, 1);
	}

    private void Init()
    {
        //ProtoData<m.TestData> testData = new ProtoData<m.TestData>();
        //Debug.Log(testData[1].id);
    }


}
