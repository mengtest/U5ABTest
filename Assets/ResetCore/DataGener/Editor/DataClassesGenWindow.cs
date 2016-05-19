using UnityEngine;
using System.Collections;
using UnityEditor;
using System.Collections.Generic;
using System.CodeDom;
using System;
using System.Reflection;
using ResetCore.Data;
using System.CodeDom.Compiler;
using System.Xml.Linq;
using ResetCore.Util;
using System.IO;

public class DataClassesGenWindow {


    [MenuItem("Tools/GameData/通过Xml生成GameData类")]
    public static void CreateNewClassesViaXml()
    {
        
        if (!Directory.Exists(PathConfig.localGameDataXmlPath))
        {
            Directory.CreateDirectory(PathConfig.localGameDataXmlPath);
        }
        DirectoryInfo dirInfo = new DirectoryInfo(PathConfig.localGameDataXmlPath);
        FileInfo[] fileInfos = dirInfo.GetFiles();
        CreateEveryFilesClasses(fileInfos);
        AssetDatabase.Refresh();
    }

    [MenuItem("Tools/GameData/通过Excel生成GameData类")]
    public static void CreateNewXmlAndClassesViaExcel()
    {
        string command = PathConfig.csTool_GameDataViaExcel + " " + PathConfig.localGameDataExcelPath + " " + PathConfig.localGameDataXmlPath + " " + PathConfig.localGameDataClassPath;
        CSToolLuncher.LaunchCsToolExe(command);
    }

    private static void CreateEveryFilesClasses(FileInfo[] fileInfos)
    {
        foreach (FileInfo fileInfo in fileInfos)
        {
            if (fileInfo.Extension == ".xml")
            {
                CreateNewClass(fileInfo.FullName.Replace("\\", "/"));
            }
        }
    }

    //
    private static string className;
    private static string baseClassName;
    private static string nameSpace;
    private static string[] importNameSpaces;
    private static string outputFile;

    private static void CreateNewClass(string filePath)
    {
        GetPropString(filePath);

        XDocument xDoc = LoadXml(filePath);

        CodeCompileUnit unit;
        CodeTypeDeclaration NewClass;
        CreateNewClass(out unit, out NewClass);

        AddProp(xDoc, NewClass);

        GenCSharp(outputFile, unit);
        
    }
    private static void GetPropString(string filePath)
    {
        className = StringEx.GetFileNameWithoutExtention(filePath);
        baseClassName = "GameData<" + className + ">";
        nameSpace = "ResetCore.Data.GameDatas";
        importNameSpaces = new string[]{
            "System","System.Collections.Generic"
        };
        if (!Directory.Exists(PathConfig.localGameDataClassPath))
        {
            Directory.CreateDirectory(PathConfig.localGameDataClassPath);
        }
        outputFile = PathConfig.localGameDataClassPath + className + ".cs";
    }
    private static XDocument LoadXml(string filePath)
    {
        XDocument xDoc = XDocument.Load(filePath);
        if (xDoc == null)
        {
            Debug.logger.LogError("创建GameData", "没有成功加载Xml");
        }
        return xDoc;
    }
    private static void CreateNewClass(out CodeCompileUnit unit, out CodeTypeDeclaration NewClass)
    {
        unit = new CodeCompileUnit();
        CodeNamespace theNamespace = new CodeNamespace(nameSpace);
        unit.Namespaces.Add(theNamespace);


        NewClass = new CodeTypeDeclaration(className);
        theNamespace.Types.Add(NewClass);

        foreach (string ns in importNameSpaces)
        {
            CodeNamespaceImport import = new CodeNamespaceImport(ns);
            theNamespace.Imports.Add(import);
        }


        NewClass.TypeAttributes = TypeAttributes.Public;
        NewClass.BaseTypes.Add(baseClassName);
        NewClass.IsClass = true;

        CodeMemberField fileNameField = new CodeMemberField("String", "fileName");
        fileNameField.Attributes = MemberAttributes.Static | MemberAttributes.Public;
        fileNameField.InitExpression = new CodeSnippetExpression("\"" + className + "\"");
        NewClass.Members.Add(fileNameField);
    }
    private static void AddProp(XDocument xDoc, CodeTypeDeclaration NewClass)
    {
        foreach (XElement el in xDoc.Root.Element("item").Elements())
        {
            string[] propAttrs = el.Name.LocalName.Split('_');
            if (propAttrs.Length < 2)
            {
                Debug.logger.LogError("生成数据类", "属性" + el.Name.LocalName + "不存在属性类型信息");
            }
            string propName = propAttrs[0];
            string propType = propAttrs[1];
            string propComment = "";
            if (propAttrs.Length > 2)
            {
                for (int i = 2; i < propAttrs.Length; i++)
                {
                    propComment += propAttrs[i] + " ";
                }
            }
            //添加字段

            CodeMemberField field = new CodeMemberField(propType, "_" + propName);
            field.Attributes = MemberAttributes.Private;

            NewClass.Members.Add(field);

            //添加属性
            CodeMemberProperty property = new CodeMemberProperty();
            property.Attributes = MemberAttributes.Public | MemberAttributes.Final;
            property.Name = propName;
            property.HasGet = true;
            property.HasSet = true;
            property.Type = new CodeTypeReference(propType);
            property.Comments.Add(new CodeCommentStatement(propComment));
            property.GetStatements.Add(new CodeMethodReturnStatement(new CodeFieldReferenceExpression(new CodeThisReferenceExpression(), "_" + propName)));
            property.SetStatements.Add(new CodeAssignStatement(new CodeFieldReferenceExpression(new CodeThisReferenceExpression(), "_" + propName), new CodePropertySetValueReferenceExpression()));

            NewClass.Members.Add(property);
        }
    }
    private static void GenCSharp(string outputFile, CodeCompileUnit unit)
    {
        //生成代码
        CodeDomProvider provider = CodeDomProvider.CreateProvider("CSharp");

        //缩进样式
        CodeGeneratorOptions options = new CodeGeneratorOptions();
        options.BracingStyle = "C";
        //空行
        options.BlankLinesBetweenMembers = true;

        using (System.IO.StreamWriter sw = new System.IO.StreamWriter(outputFile))
        {
            Debug.Log("生成代码" + outputFile);
            provider.GenerateCodeFromCompileUnit(unit, sw, options);

        }
    }
   
	
}
