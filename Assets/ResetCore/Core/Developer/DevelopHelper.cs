using UnityEngine;
using UnityEditor;
using System.Collections;

public static class DevelopHelper {

    private static string isDevelopKey = "isResetCoreDeveloper";
    public static bool isDevelopMode
    {
        get
        {
            if (!EditorPrefs.HasKey(isDevelopKey))
            {
                EditorPrefs.SetBool(isDevelopKey, false);
            }
            return EditorPrefs.GetBool(isDevelopKey);
        }
        set
        {
            EditorPrefs.SetBool(isDevelopKey, value);
        }
    }

    [MenuItem("DeveloperTools/Export the ResetCore Package")]
    public static void ExportResetCore()
    {
        
    }

    [MenuItem("DeveloperTools/Compress Extra Tools")]
    public static void ExportExtraToolsToPackage()
    {
        CompressHelper.CompressDirectory(PathConfig.ExtraToolPath, PathConfig.ExtraToolPathInPackage);
        Debug.logger.Log("Compress To " + PathConfig.ExtraToolPathInPackage);
    }
}
