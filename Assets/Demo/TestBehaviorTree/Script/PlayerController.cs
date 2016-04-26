using UnityEngine;
using System.Collections;
using ResetCore.Util;

public class PlayerController : MonoBehaviour {

    private BaseBlock block;

    void Awake()
    {
        block = GetComponent<BaseBlock>();
    }

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        Controll();
	}

    void Controll()
    {

        if (Input.GetMouseButtonDown(0))
        {
            Debug.logger.Log(Input.mousePosition);
            Debug.logger.Log(Camera.main.ScreenToWorldPoint(Input.mousePosition));
            block.Attack(Camera.main.ScreenToWorldPoint(Input.mousePosition));
        }

        if (Input.GetMouseButton(3))
        {
            //task = CoroutineTaskManager.Instance
        }
    }
}
