using UnityEngine;
using System.Collections;

public static class FuncTimer {

	public static float CountTime(System.Action act){
        float startTime = Time.realtimeSinceStartup;
        act();
        float useTime = Time.realtimeSinceStartup - startTime;
        return useTime;
    }
}
