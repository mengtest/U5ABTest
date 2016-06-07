using UnityEngine;
using System.Collections;
using System.Runtime.InteropServices;

public class DllManager {

    [DllImport("ResetLua")]
    public extern static int Add(int a, int b);

    public static void Test()
    {
        Debug.logger.Log(Add(10, 12));
    }

}
