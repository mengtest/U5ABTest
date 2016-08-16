using UnityEngine;
using System.Collections;
using ResetCore.Excel;
using System.Xml.Linq;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using ResetCore.Data.GameDatas.Xml;

namespace ResetCore.Excel
{
    public class Excel2PrefData
    {

        public static void GenPref(ExcelReader excelReader)
        {
            ExcelReader exReader = excelReader;

            XDocument xDoc = new XDocument();
            XElement root = new XElement("Root");
            xDoc.Add(root);

            List<string> name = exReader.GetMemberNames();
            List<string> value = exReader.GetLine(2);
            for (int i = 0; i < name.Count; i++)
            {
                XElement item = new XElement(name[i], value[i]);
                root.Add(item);
            }

            string outputPath = PathConfig.localPrefDataPath + Path.GetFileNameWithoutExtension(excelReader.currentSheetName) + PrefData.m_fileExtention;
            if (!Directory.Exists(Path.GetDirectoryName(outputPath)))
            {
                Directory.CreateDirectory(Path.GetDirectoryName(outputPath));
            }

            xDoc.Save(outputPath);
            AssetDatabase.Refresh();
        }

        public static void GenCS(ExcelReader excelReader)
        {
            string className = excelReader.currentSheetName;
            DataClassesGener.CreateNewClass(className, typeof(PrefData), excelReader.fieldDict);
        }

    }

}
