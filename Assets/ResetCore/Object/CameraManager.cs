using UnityEngine;
using System.Collections;

public static class CameraManager {

    public static readonly float normalSize = 15;
    public static void Recovery()
    {
        Camera.main.orthographicSize = normalSize;
    }

}
