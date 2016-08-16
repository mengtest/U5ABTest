using UnityEngine;
using System.Collections;
using UnityEditor;
using System.Linq;
using System.IO;

namespace ResetCore.Excel
{
    public class ExcelExportInMenu
    {

        [MenuItem("Assets/DataHelper/Xml/Export Selected Excel")]
        public static void ExportAllSelectedExcelToXml()
        {
            var selection = Selection.GetFiltered(typeof(UnityEngine.Object), SelectionMode.DeepAssets);
            var paths = (from s in selection
                         let path = AssetDatabase.GetAssetPath(s)
                         where (path.EndsWith(".xlsx") || path.EndsWith(".xls"))
                         select path).ToArray();

            int num = 1;
            Debug.logger.Log("Total " + paths.Length + " Files");
            foreach (string item in paths)
            {
                ExcelReader excelReader = new ExcelReader(item);
                foreach (string sheetName in excelReader.GetSheetNames())
                {
                    EditorUtility.DisplayProgressBar
                         ("Exporting Excel", "Current: " + num + "/" + paths.Length + " File: " + Path.GetFileName(item) +
                         " Sheet: " + sheetName, (float)num / (float)paths.Length);
                    Debug.logger.Log("Current: " + num + "/" + paths.Length + " File: " + Path.GetFileName(item) +
                        " Sheet: " + sheetName, (float)num / (float)paths.Length);

                    excelReader = new ExcelReader(item, sheetName);
                    Excel2Xml.GenXml(excelReader);
                    Excel2Xml.GenCS(excelReader);

                }
                num++;
            }
            EditorUtility.ClearProgressBar();
            Debug.logger.Log("Finished");
        }

        [MenuItem("Assets/DataHelper/Protobuf/Export Selected Excel")]
        public static void ExportAllSelectedExcelToProtobuf()
        {
            var selection = Selection.GetFiltered(typeof(UnityEngine.Object), SelectionMode.DeepAssets);
            var paths = (from s in selection
                         let path = AssetDatabase.GetAssetPath(s)
                         where (path.EndsWith(".xlsx") || path.EndsWith(".xls"))
                         select path).ToArray();

            int num = 1;
            Debug.logger.Log("Total " + paths.Length + " Files");
            foreach (string item in paths)
            {
                Debug.Log(item);
                ExcelReader excelReader = new ExcelReader(item);
                foreach (string sheetName in excelReader.GetSheetNames())
                {
                    EditorUtility.DisplayProgressBar
                        ("Exporting Excel", "Current: " + num + "/" + paths.Length + " File: " + Path.GetFileName(item) +
                        " Sheet: " + sheetName, (float)num / (float)paths.Length);
                    Debug.logger.Log("Current: " + num + "/" + paths.Length + " File: " + Path.GetFileName(item) +
                        " Sheet: " + sheetName, (float)num / (float)paths.Length);

                    excelReader = new ExcelReader(item, sheetName);
                    Excel2Protobuf.GenCS(excelReader);
                    Excel2Protobuf.GenProtobuf(excelReader);
                }
                num++;
            }
            EditorUtility.ClearProgressBar();
            Debug.logger.Log("Finished");
        }

        [MenuItem("Assets/DataHelper/PrefData/Export Selected Excel")]
        public static void ExportAllSelectedExcelToPrefData()
        {
            var selection = Selection.GetFiltered(typeof(UnityEngine.Object), SelectionMode.DeepAssets);
            var paths = (from s in selection
                         let path = AssetDatabase.GetAssetPath(s)
                         where (path.EndsWith(".xlsx") || path.EndsWith(".xls"))
                         select path).ToArray();

            int num = 1;
            Debug.logger.Log("Total " + paths.Length + " Files");
            foreach (string item in paths)
            {
                ExcelReader excelReader = new ExcelReader(item);
                foreach (string sheetName in excelReader.GetSheetNames())
                {
                    EditorUtility.DisplayProgressBar
                        ("Exporting Excel", "Current: " + num + "/" + paths.Length + " File: " + Path.GetFileName(item) + 
                        " Sheet: " + sheetName, (float)num / (float)paths.Length);
                    Debug.logger.Log("Current: " + num + "/" + paths.Length + " File: " + Path.GetFileName(item) +
                        " Sheet: " + sheetName, (float)num / (float)paths.Length);

                    excelReader = new ExcelReader(item, sheetName, ExcelType.Pref);
                    Excel2PrefData.GenPref(excelReader);
                    Excel2PrefData.GenCS(excelReader);

                }
                num++;
            }
            EditorUtility.ClearProgressBar();
            Debug.logger.Log("Finished");
        }
    }

}
