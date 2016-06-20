using UnityEngine;
using System.Collections;
using NPOI.SS.UserModel;
using System.IO;
using NPOI.HSSF.UserModel;
using NPOI.XSSF.UserModel;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Linq;
using System.ComponentModel;
using ResetCore.Util;

namespace ResetCore.Excel
{
    public class ExcelReader
    {

        private readonly IWorkbook workbook = null;
        public ISheet sheet { get; private set; }
        public string filepath { get; private set; }
        public string currentSheetName { get; private set; }

        public Dictionary<string, Type> fieldDict { get; private set; }

        public ExcelReader(string path, string sheetName = "")
        {
            try
            {
                this.filepath = path;
                this.currentSheetName = sheetName;

                using (FileStream fileStream = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
                {
                    string extension = Path.GetExtension(path);
                    //根据不同的后缀进行解析
                    if (extension == ".xls")
                        workbook = new HSSFWorkbook(fileStream);
                    else if (extension == ".xlsx")
                    {
#if UNITY_MAC
                    throw new Exception("xlsx is not supported on OSX.");
#else
                        workbook = new XSSFWorkbook(fileStream);
#endif
                    }
                    else
                    {
                        throw new Exception("Wrong file.");
                    }

                    if (!string.IsNullOrEmpty(currentSheetName))
                    {
                        sheet = workbook.GetSheet(currentSheetName);
                    }
                    else
                    {
                        if (workbook.GetSheetAt(0) != null)
                        {
                            sheet = workbook.GetSheetAt(0);
                            currentSheetName = sheet.SheetName;
                        }
                        else
                        {
                            Debug.logger.LogError("获取Sheet", "该Excel不存在Sheet");
                        }
                        
                    }

                    fieldDict = new Dictionary<string, Type>();
                    List<string> members = GetMemberNames();
                    List<Type> types = GetMemberTypes();

                    for (int i = 0; i < members.Count; i++ )
                    {
                        fieldDict.Add(members[i], types[i]);
                    }
                }
            }
            catch (Exception e)
            {
                Debug.logger.LogException(e);
            }
        }

        /// <summary>
        /// Excel是否有效
        /// </summary>
        public bool IsValid()
        {
            if (this.workbook != null && this.sheet != null)
                return true;

            return false;
        }

        /// <summary>
        /// 得到表头
        /// </summary>
        /// <param name="cellnum"></param>
        /// <returns></returns>
        string GetHeaderColumnName(int cellnum)
        {
            ICell headerCell = sheet.GetRow(0).GetCell(cellnum);
            if (headerCell != null)
                return headerCell.StringCellValue;
            return string.Empty;
        }

        /// <summary>
        /// Deserialize all the cell of the given sheet.
        /// 
        /// NOTE:
        ///     The first row of a sheet is header column which is not the actual value 
        ///     so it skips when it deserializes.
        ///     第一排和第二排为标题与变量类型，所以在解析时跳过
        /// </summary>
        public List<T> Deserialize<T>(int start = 2)
        {
            var t = typeof(T);
            PropertyInfo[] p = t.GetProperties();

            var result = new List<T>();

            int current = 0;
            foreach (IRow row in sheet)
            {
                if (current < start)
                {
                    current++; // skip header column.
                    continue;
                }

                var item = (T)Activator.CreateInstance(t);
                for (var i = 0; i < p.Length; i++)
                {
                    ICell cell = row.GetCell(i);

                    var property = p[i];
                    if (property.CanWrite)
                    {
                        try
                        {
                            var value = cell.StringCellValue.GetValue<T>();
                            property.SetValue(item, value, null);
                        }
                        catch (Exception e)
                        {
                            string pos = string.Format("Row[{0}], Cell[{1}]", (current + 1).ToString(), GetHeaderColumnName(i));
                            Debug.LogError(string.Format("Excel File {0} Deserialize Exception: {1} at {2}", this.filepath, e.Message, pos));
                        }
                    }
                }

                result.Add(item);

                current++;
            }

            return result;
        }

        /// <summary>
        /// 获取所有表名
        /// </summary>
        public string[] GetSheetNames()
        {
            List<string> sheetList = new List<string>();
            if (this.workbook != null)
            {
                int numSheets = this.workbook.NumberOfSheets;
                for (int i = 0; i < numSheets; i++)
                {
                    sheetList.Add(this.workbook.GetSheetName(i));
                }
            }
            else
            {
                Debug.LogError("Workbook is null. Did you forget to import excel file first?");
            }
            return (sheetList.Count > 0) ? sheetList.ToArray() : null;
        }

        /// <summary>
        /// 获得完整的表头
        /// </summary>
        /// <param name="start"></param>
        /// <returns></returns>
        public List<string> GetTitle(int start = 0)
        {
            List<string> result = new List<string>();
            IRow title = sheet.GetRow(start);

            if (title != null)
            {
                for (int i = 0; i < title.LastCellNum; i++)
                {
                    string total = title.GetCell(i).StringCellValue;
                    result.Add(total);
                }
                return result;
            }
            else
            {
                Debug.logger.LogError("GetTitle", "行无效");
                return null;
            }
        }

        /// <summary>
        /// 获取标题
        /// </summary>
        public List<string> GetMemberNames(int start = 0)
        {
            List<string> result = new List<string>();

            IRow title = sheet.GetRow(start);
            if (title != null)
            {
                for (int i = 0; i < title.LastCellNum; i++)
                {
                    string total = title.GetCell(i).StringCellValue;
                    if (total.Contains("|"))
                    {
                        result.Add(total.Split('|')[0]);
                    }
                    else
                    {
                        result.Add(total);
                    }
                }
                return result;
            }
            else
            {
                Debug.logger.LogError("GetTitle", "行无效");
                return null;
            }
        }

        /// <summary>
        /// 获取标题
        /// </summary>
        public List<Type> GetMemberTypes(int start = 0)
        {
            List<Type> result = new List<Type>();

            IRow title = sheet.GetRow(start);
            if (title != null)
            {
                for (int i = 0; i < title.LastCellNum; i++)
                {
                    string total = title.GetCell(i).StringCellValue;
                    if (total.Contains("|"))
                    {
                        result.Add(total.Split('|')[1].GetTypeByString());
                    }
                    else
                    {
                        result.Add(typeof(string));
                    }
                }
                return result;
            }
            else
            {
                Debug.logger.LogError("GetTitle", "行无效");
                return null;
            }
        }


        public List<string> GetComment(int start = 1)
        {
            List<string> result = new List<string>();

            IRow typeStr = sheet.GetRow(start);
            if (typeStr != null)
            {
                for (int i = 0; i < typeStr.LastCellNum; i++)
                {
                    result.Add(typeStr.Cells[i].StringCellValue);
                }
                return result;
            }
            else
            {
                Debug.logger.LogError("GetClomnType", "行无效");
                return null;
            }
        }

        public List<Dictionary<string, string>> GetRows(int start = 2)
        {
            
            List<string> clomnNameList = GetMemberNames();
            int length = clomnNameList.Count;
            List<string> tagName = new List<string>();
            for (int i = 0; i < length; i++)
            {
                tagName.Add(clomnNameList[i]);
            }

            List<Dictionary<string, string>> rows = new List<Dictionary<string, string>>();

            int current = 0;
            foreach (IRow row in sheet)
            {
                if (current < start)
                {
                    current++;
                    continue;
                }

                Dictionary<string, string> rowDict = new Dictionary<string, string>();
                for (int i = 0; i < length; i++)
                {
                    row.GetCell(i).SetCellType(CellType.String);
                    rowDict.Add(tagName[i], row.GetCell(i).StringCellValue);
                }
                rows.Add(rowDict);
                current++;
            }

            return rows;
        }


        public List<Dictionary<string, object>> GetRowObjs(int start = 2)
        {
            List<string> clomnNameList = GetMemberNames();
            List<Type> typeList = GetMemberTypes();
            int length = clomnNameList.Count;
            List<string> tagName = new List<string>();
            for (int i = 0; i < length; i++)
            {
                tagName.Add(clomnNameList[i]);
            }

            List<Dictionary<string, object>> rows = new List<Dictionary<string, object>>();

            int current = 0;
            foreach (IRow row in sheet)
            {
                if (current < start)
                {
                    current++;
                    continue;
                }

                Dictionary<string, object> rowDict = new Dictionary<string, object>();
                for (int i = 0; i < length; i++)
                {
                    row.GetCell(i).SetCellType(CellType.String);
                    rowDict.Add(tagName[i], row.GetCell(i).StringCellValue.GetValue(typeList[i]));
                }
                rows.Add(rowDict);
                current++;
            }

            return rows;
        }
    }

}
