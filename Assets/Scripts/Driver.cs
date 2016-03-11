using UnityEngine;
using System.Collections;
using ResetCore.Asset;

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
