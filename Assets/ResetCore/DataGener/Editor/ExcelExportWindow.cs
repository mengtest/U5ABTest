using UnityEngine;
using System.Collections;
using UnityEditor;
using System.IO;
using ResetCore.Excel;
using System;
using System.Collections.Generic;
using NPOI.SS.UserModel;

namespace ResetCore.Excel
{
    public class ExcelExportWindow : EditorWindow
    {

        //显示窗口的函数
        [MenuItem("Tools/GameData/Excel导出工具")]
        static void ShowMainWindow()
        {
            Rect wr = new Rect(0, 0, 800, 800);
            ExcelExportWindow window =
                EditorWindow.GetWindowWithRect(typeof(ExcelExportWindow), wr, true, "Excel导出工具") as ExcelExportWindow;
            window.Show();
        }

        ExcelReader excelReader;
        void OnGUI()
        {
            ShowOpenExcel();
            EditorGUILayout.Space();
            if (excelReader != null)
            {
                ShowSetType();
                EditorGUILayout.Space();
                ShowExportXml();
                EditorGUILayout.Space();
                ShowExportProtobuf();
                EditorGUILayout.Space();
                ShowExportObj();
                EditorGUILayout.Space();
                ShowExportJson();
                
            }

        }

        string excelFilePath = "";
        string fileName = "";
        string[] sheetsNames = { "" };
        int currentSheetIndex = 0;
        string currentSheetName = "";

        private void ShowOpenExcel()
        {
            GUIStyle headStyle = GUIHelper.MakeHeader();
            GUILayout.Label("选择文件", headStyle);
            EditorGUILayout.Space();

            GUILayout.BeginHorizontal();
            GUILayout.Label("File:", GUILayout.Width(50));

            string path = string.Empty;
            if (string.IsNullOrEmpty(excelFilePath))
                path = Application.dataPath;
            else
                path = excelFilePath;

            excelFilePath = GUILayout.TextField(path, GUILayout.Width(250));
            if (GUILayout.Button("...", GUILayout.Width(20)))
            {
                string folder = Path.GetDirectoryName(path);
#if UNITY_EDITOR_WIN
                path = EditorUtility.OpenFilePanel("Open Excel file", folder, "excel files;*.xls;*.xlsx");
#else
            path = EditorUtility.OpenFilePanel("Open Excel file", folder, "xls");
#endif

                if (path.Length != 0)
                {
                    fileName = Path.GetFileName(path);

                    // the path should be relative not absolute one to make it work on any platform.
                    int index = path.IndexOf("Assets");
                    if (index >= 0)
                    {
                        // set relative path
                        excelFilePath = path.Substring(index);

                        //
                        excelReader = new ExcelReader(path);

                        // pass absolute path
                        sheetsNames = excelReader.GetSheetNames();
                    }
                    else
                    {
                        EditorUtility.DisplayDialog("Error",
                            @"Wrong folder is selected.
                        Set a folder under the 'Assets' folder! \n
                        The excel file should be anywhere under  the 'Assets' folder", "OK");
                        return;
                    }
                }
            }
            GUILayout.EndHorizontal();

            GUILayout.Label("Spreadsheet File: " + fileName);

            EditorGUILayout.Space();

            if (excelReader == null) return;

            using (new GUILayout.HorizontalScope())
            {
                EditorGUILayout.LabelField("Worksheet: ", GUILayout.Width(100));
                currentSheetIndex = EditorGUILayout.Popup(currentSheetIndex, sheetsNames, GUILayout.Width(60));
                if (sheetsNames != null)
                {
                    currentSheetName = sheetsNames[currentSheetIndex];
                }
                if (GUILayout.Button("Refresh", GUILayout.Width(60)))
                {
                    // reopen the excel file e.g) new worksheet is added so need to reopen.
                    excelReader = new ExcelReader(path, currentSheetName);
                    sheetsNames = excelReader.GetSheetNames();

                    // one of worksheet was removed, so reset the selected worksheet index
                    // to prevent the index out of range error.
                    if (sheetsNames.Length <= currentSheetIndex)
                    {
                        currentSheetIndex = 0;

                        string message = "Worksheet was changed. Check the 'Worksheet' and 'Update' it again if it is necessary.";
                        EditorUtility.DisplayDialog("Info", message, "OK");
                    }
                }
            }

        }

        private void ShowSetType()
        {
            GUIStyle headStyle = GUIHelper.MakeHeader();
            GUILayout.Label("设置类型种类", headStyle);
            EditorGUILayout.Space();

            //表头部
            const int MEMBER_WIDTH = 100;
            GUILayout.BeginHorizontal(EditorStyles.toolbar);
            GUILayout.Label("成员", GUILayout.MinWidth(MEMBER_WIDTH));
            GUILayout.FlexibleSpace();
            string[] names = { "类型", "注释" };
            int[] widths = { 100, 100 };
            for (int i = 0; i < names.Length; i++)
            {
                GUILayout.Label(new GUIContent(names[i]), GUILayout.Width(widths[i]));
            }
            GUILayout.EndHorizontal();//EditorStyles.toolbar

            EditorGUILayout.BeginVertical("box");

            Dictionary<string, Type> fieldDict = excelReader.fieldDict;

            List<string> comment = excelReader.GetComment();


            int index = 0;
            //表内容
            foreach (string fieldName in fieldDict.Keys)
            {
                GUILayout.BeginHorizontal();

                // member field label
                EditorGUILayout.LabelField(fieldName, GUILayout.MinWidth(MEMBER_WIDTH));
                GUILayout.FlexibleSpace();

                // type enum popup
                //header.type = (CellType)EditorGUILayout.EnumPopup(header.type, GUILayout.Width(100));
                GUILayout.Label(fieldDict[fieldName].Name, GUILayout.Width(100));
                //excelReader.fieldDict[field.Key] = Type.GetType(GUILayout.TextField(field.Value.Name, GUILayout.Width(100)));
                GUILayout.Space(20);

                // array toggle
                GUILayout.Label(comment[index], GUILayout.Width(100));

                GUILayout.Space(10);
                GUILayout.EndHorizontal();

                index++;

            }
            EditorGUILayout.EndVertical();
        }

        private void ShowExportXml()
        {
            GUIStyle headStyle = GUIHelper.MakeHeader();
            GUILayout.Label("导出Xml（支持大多数类型）", headStyle);
            EditorGUILayout.Space();

            GUILayout.BeginHorizontal();
            if (GUILayout.Button("导出Xml", GUILayout.Width(100)))
            {
                Excel2Xml.GenXml(excelReader);
            }
            if (GUILayout.Button("导出XmlData.cs", GUILayout.Width(100)))
            {
                Excel2Xml.GenCS(excelReader);
            }
            GUILayout.EndHorizontal();

            if (GUILayout.Button("全部导出", GUILayout.Width(100)))
            {
                Excel2Xml.GenXml(excelReader);
                Excel2Xml.GenCS(excelReader);
            }
        }

        private void ShowExportObj()
        {
            GUIStyle headStyle = GUIHelper.MakeHeader();
            GUILayout.Label("导出Obj(开发中)", headStyle);
            EditorGUILayout.Space();

            GUILayout.BeginHorizontal();
            if (GUILayout.Button("导出Objdat", GUILayout.Width(100)))
            {
                Excel2ScrObj.GenObj(excelReader);
            }
            if (GUILayout.Button("导出ObjData.cs", GUILayout.Width(100)))
            {
                Excel2ScrObj.GenCS(excelReader);
            }
            GUILayout.EndHorizontal();

            if (GUILayout.Button("全部导出", GUILayout.Width(100)))
            {
                Excel2ScrObj.GenObj(excelReader);
                Excel2ScrObj.GenCS(excelReader);
            }
        }

        private void ShowExportJson()
        {
            GUIStyle headStyle = GUIHelper.MakeHeader();
            GUILayout.Label("导出Json(开发中)", headStyle);
            EditorGUILayout.Space();

            GUILayout.BeginHorizontal();
            if (GUILayout.Button("导出Json", GUILayout.Width(100)))
            {

            }
            if (GUILayout.Button("导出JsonData.cs", GUILayout.Width(100)))
            {

            }
            GUILayout.EndHorizontal();

            if (GUILayout.Button("全部导出", GUILayout.Width(100)))
            {

            }
        }

        private void ShowExportProtobuf()
        {
            GUIStyle headStyle = GUIHelper.MakeHeader();
            GUILayout.Label("导出Protobuf(仅支持C#内置类型)", headStyle);
            EditorGUILayout.Space();

            GUILayout.BeginHorizontal();
            if (GUILayout.Button("导出Protobuf", GUILayout.Width(100)))
            {
                Excel2Protobuf.GenProtobuf(excelReader);
            }
            if (GUILayout.Button("导出ProtoData.cs", GUILayout.Width(100)))
            {
                Excel2Protobuf.GenCS(excelReader);
            }
            GUILayout.EndHorizontal();

            if (GUILayout.Button("全部导出", GUILayout.Width(100)))
            {
                Excel2Protobuf.GenCS(excelReader);
                Excel2Protobuf.GenProtobuf(excelReader);
            }
        }

    }

}
