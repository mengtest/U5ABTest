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
using ResetCore.Excel;

namespace ResetCore.Data
{
    public class DataClassesGenWindow
    {

        //[MenuItem("Tools/GameData/通过Excel生成XmlGameData类")]
        //public static void CreateNewXmlAndClassesViaExcel()
        //{
        //    string command = PathConfig.csTool_GameDataViaExcel + " " + PathConfig.localGameDataExcelPath + " " + PathConfig.localGameDataXmlPath + " " + PathConfig.localXmlGameDataClassPath;
        //    CSToolLuncher.LaunchCsToolExe(command);
        //}

    }

}
