using System;
using System.Collections.Generic;
using System.Reflection;
using Microsoft.Office.Interop.Excel;

namespace ExecutableIrt.ExcelInteraction
{
    public class CellReader
    {
        public static List<string> GetRange(string range, Worksheet excelWorksheet)
        {
            Range workingRangeCells = excelWorksheet.get_Range(range, Type.Missing);

            Array array = (Array)workingRangeCells.Cells.Value2;

            List<string> stringList = new List<string>();
            foreach (var row in array)
            {
                if (row != null)
                {
                    stringList.Add(row.ToString());
                }
            }

            return stringList;
        }

        public static string GetCell(string cell, Worksheet excelWorksheet)
        {
            Range workingRangeCells = excelWorksheet.get_Range(cell, Type.Missing);
            string cellValue = workingRangeCells.get_Value(Missing.Value).ToString();

            return cellValue;
        }
    }
}
