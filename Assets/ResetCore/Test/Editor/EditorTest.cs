using UnityEngine;
using System.Collections;
using UnityEditor;
using ProtoBuf;
using ResetCore.CodeDom;

public class EditorTest {

    [MenuItem("Tools/TestBtn")]
    public static void CreateNewXmlAndClassesViaExcel()
    {
        CodeGener gener = new CodeGener("ResetCore.Test", "Test");
        gener.AddImport("System", "UnityEngine")
            .AddMemberProperty(typeof(string), "testStr", (member) =>
            {
                member.AddMemberCostomAttribute<SpaceAttribute>("100");
                member.AddComment("heihei", true);
            })
            .AddMemberField(typeof(string), "testFieldStr", (member) => {
                member.AddFieldMemberInit("\"haha\"");
            })
            .GenCSharp(Application.dataPath);

    }

}
