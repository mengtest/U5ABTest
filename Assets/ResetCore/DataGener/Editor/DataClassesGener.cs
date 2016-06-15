using UnityEngine;
using System.Collections;
using System.CodeDom.Compiler;
using System.CodeDom;
using System.Reflection;
using System.Xml.Linq;
using ResetCore.Util;
using System.IO;
using System.Collections.Generic;
using System;
using ResetCore.Data;
using UnityEditor;


public enum GameDataType
{
    Xml,
    Obj
}
public class DataClassesGener {

    //
    private static string className;
    private static string baseClassName;
    private static string nameSpace;
    private static string[] importNameSpaces;
    private static string outputFile;

    

    public static void CreateNewClass(string className, Dictionary<string, Type> fieldDict, GameDataType type)
    {
        GetPropString(className, type);

        CodeCompileUnit unit;
        CodeTypeDeclaration NewClass;
        CreateNewClass(out unit, out NewClass);

        AddProp(fieldDict, NewClass);

        GenCSharp(outputFile, unit);

    }

    private static void GetPropString(string className, GameDataType type)
    {
        importNameSpaces = new string[]{
                "System","System.Collections.Generic", "UnityEngine"
            };
        DataClassesGener.className = className;
        if (type == GameDataType.Xml)
        {
            baseClassName = typeof(XmlData).Name + "<" + className + ">";
            nameSpace = XmlData.nameSpace;
            if (!Directory.Exists(PathConfig.localXmlGameDataClassPath))
            {
                Directory.CreateDirectory(PathConfig.localXmlGameDataClassPath);
            }
            outputFile = PathConfig.localXmlGameDataClassPath + className + ".cs";
            
        }
        else if (type == GameDataType.Obj)
        {
            baseClassName = typeof(ObjData).Name + "<" + className + ">";
            nameSpace = ObjData.nameSpace;
            if (!Directory.Exists(PathConfig.localObjGameDataClassPath))
            {
                Directory.CreateDirectory(PathConfig.localObjGameDataClassPath);
            }
            outputFile = PathConfig.localObjGameDataClassPath + className + ".cs";
        }
        else
        {
            Debug.logger.LogError("GameData", "无效的数据类型");
        }
        
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
    private static void AddProp(Dictionary<string, Type> fieldDic, CodeTypeDeclaration NewClass)
    {
        foreach (KeyValuePair<string, Type> pair in fieldDic)
        {
            string propName = pair.Key;
            string propType = pair.Value.Name;

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
