using UnityEngine;
using System.Collections;
using System.Xml.Linq;
using System.Collections.Generic;
using System;
using System.IO;
using UnityEditor;

namespace ResetCore.Excel
{
    public class Excel2Xml
    {
        public static void GenXml(ExcelReader excelReader)
        {

            ExcelReader exReader = excelReader;

            XDocument xDoc = new XDocument();
            XElement root = new XElement("Root");
            xDoc.Add(root);

            List<Dictionary<string, string>> rows = exReader.GetRows();
            for (int i = 0; i < rows.Count; i++)
            {
                XElement item = new XElement("item");
                root.Add(item);
                foreach (KeyValuePair<string, string> pair in rows[i])
                {
                    item.Add(new XElement(pair.Key, pair.Value));
                }

            }

            string outputPath = PathConfig.localGameDataXmlPath + Path.GetFileNameWithoutExtension(excelReader.currentSheetName) + ".xml";
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
            DataClassesGener.CreateNewClass(className, excelReader.fieldDict, GameDataType.Xml);
        }
       
    }

}
