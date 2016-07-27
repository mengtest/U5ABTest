using UnityEngine;
using System.Collections;
using UnityEditor;
using System.Linq;
using System.IO;

namespace ResetCore.Excel
{
    public class ExcelExportInMenu
    {

        [MenuItem("Assets/DataHelper/Xml/导出选中的Excel")]
        public static void ExportAllSelectedExcelToXml()
        {
            var selection = Selection.GetFiltered(typeof(UnityEngine.Object), SelectionMode.DeepAssets);
            var paths = (from s in selection
                         let path = AssetDatabase.GetAssetPath(s)
                         where (path.EndsWith(".xlsx") || path.EndsWith(".xls"))
                         select path).ToArray();

            int num = 1;
            Debug.logger.Log("共" + paths.Length + "个文件");
            foreach (string item in paths)
            {
                ExcelReader excelReader = new ExcelReader(item);
                foreach (string sheetName in excelReader.GetSheetNames())
                {
                    EditorUtility.DisplayProgressBar
                        ("正在导出Excel", "当前进度 " + num + "/" + paths.Length + " 文件为 " + Path.GetFileName(item) + " 表名为 " + sheetName, (float)num / (float)paths.Length);
                    Debug.logger.Log("当前进度 " + num + "/" + paths.Length + " 文件为 " + Path.GetFileName(item) + " 表名为 " + sheetName);
                    excelReader = new ExcelReader(item, sheetName);
                    Excel2Xml.GenXml(excelReader);
                    Excel2Xml.GenCS(excelReader);

                }
                num++;
            }
            EditorUtility.ClearProgressBar();
            Debug.logger.Log("导出完成");
        }

        [MenuItem("Assets/DataHelper/Protobuf/导出选中的Excel")]
        public static void ExportAllSelectedExcelToProtobuf()
        {
            var selection = Selection.GetFiltered(typeof(UnityEngine.Object), SelectionMode.DeepAssets);
            var paths = (from s in selection
                         let path = AssetDatabase.GetAssetPath(s)
                         where (path.EndsWith(".xlsx") || path.EndsWith(".xls"))
                         select path).ToArray();

            int num = 1;
            Debug.logger.Log("共" + paths.Length + "个文件");
            foreach (string item in paths)
            {
                Debug.Log(item);
                ExcelReader excelReader = new ExcelReader(item);
                foreach (string sheetName in excelReader.GetSheetNames())
                {
                    EditorUtility.DisplayProgressBar
                        ("正在导出Excel", "当前进度 " + num + "/" + paths.Length + " 文件为 " + Path.GetFileName(item) + " 表名为 " + sheetName, (float)num / (float)paths.Length);
                    Debug.logger.Log("当前进度 " + num + "/" + paths.Length + " 文件为 " + Path.GetFileName(item) + " 表名为 " + sheetName);
                    excelReader = new ExcelReader(item, sheetName);
                    Excel2Protobuf.GenCS(excelReader);
                    Excel2Protobuf.GenProtobuf(excelReader);
                }
                num++;
            }
            EditorUtility.ClearProgressBar();
            Debug.logger.Log("导出完成");
        }
    }

}
