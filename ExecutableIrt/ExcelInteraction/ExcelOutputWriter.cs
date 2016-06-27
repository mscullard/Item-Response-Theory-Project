using System;
using System.Collections.Generic;
using System.Linq;
using ExecutableIrt.ExcelInteraction.DataObjects;
using ExcelWriter = Microsoft.Office.Interop.Excel;

namespace ExecutableIrt.ExcelInteraction
{
    public class ExcelOutputWriter
    {
        private readonly string _excelLocationString;
        private readonly ExcelWriter.Application _excelApp;

        private int HeaderScaleStartIndex = 2;
        private int PersonRowStartIndexOffset = 2;

        public ExcelOutputWriter(string excelLocationString, ExcelWriter.Application excelApp)
        {
            _excelLocationString = excelLocationString;
            _excelApp = excelApp;
        }

        public void Write(List<ScoreDetails> scoreDetails)
        {
            ExcelWriter.Workbooks excelWorkbooks;
            ExcelWriter.Workbook myExcelWorkbook;

            object misValue = System.Reflection.Missing.Value;

            _excelApp.Visible = false;
            excelWorkbooks = _excelApp.Workbooks;
            String fileName = _excelLocationString;
            myExcelWorkbook = excelWorkbooks.Open(fileName, misValue, false, misValue, misValue, misValue, misValue,
                misValue, misValue, misValue, misValue, misValue, misValue, misValue, misValue);

            ExcelWriter.Worksheet worksheet = (ExcelWriter.Worksheet)myExcelWorkbook.Sheets["Output"];

            ClearWorksheet(worksheet);

            List<string> scaleNames = scoreDetails.Select(x => x.ScaleName).Distinct().ToList();
            PrintHeader(scaleNames, worksheet);
            WritePersons(scoreDetails, worksheet, scaleNames);
            SaveWorksheet(worksheet, _excelApp);
            excelWorkbooks.Close();
            _excelApp.Quit();  

            GC.Collect();
            GC.WaitForPendingFinalizers();
            GC.Collect();
        }

        private void SaveWorksheet(ExcelWriter.Worksheet worksheet, ExcelWriter.Application excelApp)
        {
            excelApp.DisplayAlerts = false;
            worksheet.SaveAs(_excelLocationString, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, ExcelWriter.XlSaveAsAccessMode.xlNoChange, Type.Missing, Type.Missing, Type.Missing);
        }

        private void WritePersons(List<ScoreDetails> scoreDetails, ExcelWriter.Worksheet worksheet, List<string> scaleNames)
        {
            List<string> personNames = scoreDetails.Select(x => x.PersonName).Distinct().ToList();

            int rowIndex = 2;
            foreach (var name in personNames)
            {
                PrintPerson(name, scoreDetails, worksheet, scaleNames, rowIndex);
                rowIndex++;
            }
        }

        private void PrintPerson(string name, List<ScoreDetails> scoreDetails, ExcelWriter.Worksheet worksheet, List<string> scaleNames, int rowIndex)
        {
            List<ScoreDetails> scoresForPerson = scoreDetails.Where(x => x.PersonName == name).ToList();

            worksheet.Cells[rowIndex, 1] = name;
            for (int i = 0; i < scaleNames.Count; i++)
            {
                string scaleName = scaleNames[i];
                var score = scoresForPerson.Single(x => x.ScaleName == scaleName);

                worksheet.Cells[rowIndex, i + PersonRowStartIndexOffset] = score.Score;
            }
        }

        private ExcelWriter.Worksheet OpenWorksheet(ExcelWriter.Application myExcelApp)
        {   
            ExcelWriter.Workbooks myExcelWorkbooks;
            ExcelWriter.Workbook myExcelWorkbook;

            object misValue = System.Reflection.Missing.Value;

            myExcelApp = new ExcelWriter.Application();
            myExcelApp.Visible = false;
            myExcelWorkbooks = myExcelApp.Workbooks;
            String fileName = _excelLocationString;
            myExcelWorkbook = myExcelWorkbooks.Open(fileName, misValue, misValue, misValue, misValue, misValue, misValue,
                misValue, misValue, misValue, misValue, misValue, misValue, misValue, misValue);

            ExcelWriter.Worksheet myExcelWorksheet = (ExcelWriter.Worksheet) myExcelWorkbook.Sheets["Output"];
            return myExcelWorksheet;
        }

        private static void ClearWorksheet(ExcelWriter.Worksheet myExcelWorksheet)
        {
            myExcelWorksheet.Cells.Clear();
        }

        private void PrintHeader(List<string> scaleNames, ExcelWriter.Worksheet myExcelWorksheet)
        {
            myExcelWorksheet.Cells[1, 1].EntireRow.Font.Bold = true;
            myExcelWorksheet.Cells[1, 1] = "ID";
            for (int i = 0; i < scaleNames.Count; i++)
            {
                myExcelWorksheet.Cells[1, i + HeaderScaleStartIndex] = scaleNames[i];
            }
        }
    }
}
