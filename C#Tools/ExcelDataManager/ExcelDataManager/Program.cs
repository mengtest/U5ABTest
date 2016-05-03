using ExcelDataManager.Code;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExcelDataManager
{
    class Program
    {
        static void Main(string[] args)
        {
            ParseXlsx.ReadExcelFile(@"D:\Users\ResetOTER\Desktop\test.xlsx");
        }
    }
}
