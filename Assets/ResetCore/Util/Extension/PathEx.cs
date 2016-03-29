using UnityEngine;
using System.Collections;
using System.IO;

public class PathEx {

    public static void MakeDirectoryExist(string path)
    {
        if (!Directory.Exists(path))
        {
            Directory.CreateDirectory(path);
        }
    }
}
