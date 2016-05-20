using UnityEngine;
using System.Collections;
using System.IO;
using System;

public class StreamingDataLoader {

	public static AssetBundle LoadAsset(string path)
    {
        if(Application.platform == RuntimePlatform.Android){
            path = Path.Combine(Application.dataPath + "!assets", path);
        }else{
            path = Path.Combine(Application.streamingAssetsPath, path);
        }

        return AssetBundle.LoadFromFile(path);
    }

}
