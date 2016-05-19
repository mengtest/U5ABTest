using UnityEngine;
using System.Collections;
using System.Diagnostics;

public class CSToolLuncher {

    public static void LaunchCsToolExe(string command)
    {

        Process myProcess = new Process();

        string fileName = PathConfig.csToolPath;

        UnityEngine.Debug.logger.Log(command);

        ProcessStartInfo myProcessStartInfo = new ProcessStartInfo(fileName, command);

        myProcess.StartInfo = myProcessStartInfo;

        myProcess.Start();

    }
}
