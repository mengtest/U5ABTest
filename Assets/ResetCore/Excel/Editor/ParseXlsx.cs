//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.IO;
//using System.Reflection;
//using UnityEngine;
//using Excel = Microsoft.Office.Interop.Excel;

//namespace ReadXlsxData
//{


//    static class ParseXlsx
//    {
//        public static Excel.Application m_ExcelFile = new Excel.Application();
//        public static StringBuilder classSource;
//        public static StringBuilder objectData;
//        public static void CloseExcelApplication()
//        {
//            m_ExcelFile.Quit();
//            m_ExcelFile = null;
//        }
//        public static void ReadExcelFile(string excelFilePath)
//        {
//            classSource = new StringBuilder(); ;
//            objectData = new StringBuilder();
//            Excel._Workbook m_Workbook;
//            Excel._Worksheet m_Worksheet;
//            object missing = System.Reflection.Missing.Value;
//            Debug.logger.Log("excelFilePath:" + excelFilePath);
//            m_ExcelFile.Workbooks.Open(excelFilePath);
//            m_ExcelFile.Visible = false;
//            m_Workbook = m_ExcelFile.Workbooks[1];
//            m_Worksheet = (Excel.Worksheet)m_Workbook.ActiveSheet;
//            int clomn_Count = m_Worksheet.UsedRange.Columns.Count;
//            int row_Count = m_Worksheet.UsedRange.Rows.Count;
//            classSource.Append("using System;\n");
//            classSource.Append("[Serializable]\n");
//            classSource.Append("public   class   DynamicClass \n");
//            classSource.Append("{\n");
//            string propertyName, propertyType;
//            for (int j = 2; j < clomn_Count + 1; j++)
//            {
//                propertyName = ((Excel.Range)m_Worksheet.Cells[3, j]).Text.ToString();
//                propertyType = ((Excel.Range)m_Worksheet.Cells[4, j]).Text.ToString();
//                classSource.Append(" private  " + propertyType + "  _" + propertyName + " ;\n");
//                classSource.Append(" public   " + propertyType + "   " + "" + propertyName + "\n");
//                classSource.Append(" {\n");
//                classSource.Append(" get{   return   _" + propertyName + ";}   \n");
//                classSource.Append(" set{   _" + propertyName + "   =   value;   }\n");
//                classSource.Append(" }\n");
//                //classSource.Append("\tpublic " + ((Excel.Range)m_Worksheet.Cells[4, j]).Text.ToString() + " " + ((Excel.Range)m_Worksheet.Cells[3, j]).Text.ToString() + ";\n");
//            }
//            classSource.Append("}\n");
//            Debug.logger.Log(classSource.ToString());
//            for (int i = 7; i < row_Count + 1; i++)//
//            {
//                for (int j = 2; j < clomn_Count + 1; j++)
//                {
//                    objectData.Append(((Excel.Range)m_Worksheet.Cells[i, j]).Text.ToString() + ";");

//                }
//                objectData.Append("\n");
//                try
//                {
//                    Debug.logger.Log(objectData.ToString());
//                }
//                catch (Exception ex)
//                {
//                    Debug.logger.LogException(ex);
//                }


//            }
//            //关闭Excel相关对象
//            m_Worksheet = null;
//            m_Workbook = null;
//        }
//    }

//}
