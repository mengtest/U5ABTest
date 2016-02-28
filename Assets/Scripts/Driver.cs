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
        
	}

    private void Init()
    {

    }


}
