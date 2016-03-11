using UnityEngine;
using System.Collections;
using System.IO;

public class DownloadManager : MonoBehaviour {

	// Use this for initialization
	void Start () {
        StartCoroutine(DownloadWWW("http://10.224.32.115/ResetTest/0.0.0.11"));
	}
	
    IEnumerator DownloadWWW(string url,System.Action afterAct = null)
    {
        WWW www = new WWW(url);
        while (!www.isDone)
        {
            yield return www;
        }
        File.WriteAllBytes(PathConfig.bundleRootPath + "/0.0.0.11", www.bytes);
    }

}
