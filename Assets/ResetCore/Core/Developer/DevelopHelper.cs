#if RESET_DEVELOPER

using UnityEngine;
using UnityEditor;
using System.Collections;

public static class DevelopHelper {

    

    //[MenuItem("DeveloperTools/Export the ResetCore Package")]
    //public static void ExportResetCore()
    //{
        
    //}

    [MenuItem("DeveloperTools/Compress Extra Tools")]
    public static void ExportExtraToolsToPackage()
    {
        CompressHelper.CompressDirectory(PathConfig.ExtraToolPath, PathConfig.ExtraToolPathInPackage);
        Debug.logger.Log("Compress To " + PathConfig.ExtraToolPathInPackage);
    }
}

#endif