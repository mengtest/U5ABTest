using UnityEngine;
using System.Collections;
using UnityEditor;
using ProtoBuf;
using ResetCore.CodeDom;

public class EditorTest {

    [MenuItem("Tools/TestBtn")]
    public static void CreateNewXmlAndClassesViaExcel()
    {
        Debug.logger.Log(PlayerSettings.GetScriptingDefineSymbolsForGroup(EditorUserBuildSettings.selectedBuildTargetGroup));
    }

}
