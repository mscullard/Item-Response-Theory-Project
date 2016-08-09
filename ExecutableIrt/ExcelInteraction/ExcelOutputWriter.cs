using System;
using System.Collections.Generic;
using System.Linq;
using ExecutableIrt.DataObjects;
using ExecutableIrt.ExcelInteraction.DataObjects;
using IRT.Data;
using IRT.ModelParameters;
using IRT.Parameters;
using ExcelWriter = Microsoft.Office.Interop.Excel;

namespace ExecutableIrt.ExcelInteraction
{
    public class ExcelOutputWriter
    {
        private readonly string _excelLocationString;
        private readonly ExcelWriter.Application _excelApp;

        private int HeaderScaleStartIndex = 2;
        private int PersonRowStartIndexOffset = 2;

        private int DetailedPersonStartingOffset = 2;

        private int QuestionLabelColumnIndex = 1;
        private int ScoreColumnIndex = 2;
        private int ThetaColumnIndex = 3;
        private int SeeColumnIndex = 4;
        private int InfoColumnIndex = 5;
        private int ParameterAColumnIndex = 6;
        private int ParameterBColumnIndex = 7;
        private int ParameterCColumnIndex = 8;
        private int ParameterDColumnIndex = 9;

        public ExcelOutputWriter(string excelLocationString, ExcelWriter.Application excelApp)
        {
            _excelLocationString = excelLocationString;
            _excelApp = excelApp;
        }

        public void Write(ScoringOutput scoringOutput)
        {
            ExcelWriter.Workbooks excelWorkbooks;
            ExcelWriter.Workbook myExcelWorkbook;

            object misValue = System.Reflection.Missing.Value;

            _excelApp.Visible = false;
            excelWorkbooks = _excelApp.Workbooks;
            String fileName = _excelLocationString;
            myExcelWorkbook = excelWorkbooks.Open(fileName, misValue, false, misValue, misValue, misValue, misValue,
                misValue, misValue, misValue, misValue, misValue, misValue, misValue, misValue);

            ExcelWriter.Worksheet outputTabWorksheet = (ExcelWriter.Worksheet)myExcelWorkbook.Sheets["Output"];
            ExcelWriter.Worksheet firstPersonDetailsTabWorksheet = (ExcelWriter.Worksheet)myExcelWorkbook.Sheets["First Person Detailed Output"];

            ClearWorksheet(outputTabWorksheet);
            ClearWorksheet(firstPersonDetailsTabWorksheet);

            var scoreDetails = scoringOutput.ScoreDetails;

            List<string> scaleNames = scoreDetails.Select(x => x.ScaleName).Distinct().ToList();
            PrintOutputTabHeader(scaleNames, outputTabWorksheet);
            WriteOutputTabPersons(scoreDetails, outputTabWorksheet, scaleNames);

            PrintFirstPersonDetailsHeader(firstPersonDetailsTabWorksheet);
            PrintFirstPersonDetailsResults(firstPersonDetailsTabWorksheet, scoringOutput.FirstPersonQuestionInfo);

            SaveWorksheet(outputTabWorksheet, _excelApp);
            excelWorkbooks.Close();
            _excelApp.Quit();  

            GC.Collect();
            GC.WaitForPendingFinalizers();
            GC.Collect();
        }

        private void PrintFirstPersonDetailsResults(ExcelWriter.Worksheet worksheet, List<QuestionInfo> firstPersonQuestionInfo)
        {
            for (int i = 0; i < firstPersonQuestionInfo.Count; i++)
            {
                worksheet.Cells[i + DetailedPersonStartingOffset, QuestionLabelColumnIndex] = firstPersonQuestionInfo[i].Question.QuestionLabel;
                worksheet.Cells[i + DetailedPersonStartingOffset, ScoreColumnIndex] = firstPersonQuestionInfo[i].Score;
                worksheet.Cells[i + DetailedPersonStartingOffset, ThetaColumnIndex] = firstPersonQuestionInfo[i].ThetaEstimate;
                worksheet.Cells[i + DetailedPersonStartingOffset, SeeColumnIndex] = firstPersonQuestionInfo[i].SEE;
                worksheet.Cells[i + DetailedPersonStartingOffset, InfoColumnIndex] = firstPersonQuestionInfo[i].Information;

                var modelParameters = (FourParamModelParameters) firstPersonQuestionInfo[i].Question.ModelParameters;
                worksheet.Cells[i + DetailedPersonStartingOffset, ParameterAColumnIndex] = modelParameters.Alpha;
                worksheet.Cells[i + DetailedPersonStartingOffset, ParameterBColumnIndex] = modelParameters.Delta;
                worksheet.Cells[i + DetailedPersonStartingOffset, ParameterCColumnIndex] = modelParameters.Chi;
                worksheet.Cells[i + DetailedPersonStartingOffset, ParameterDColumnIndex] = modelParameters.Epsilon;
            }
        }

        private void PrintFirstPersonDetailsHeader(ExcelWriter.Worksheet worksheet)
        {
            List<string> headerLabels = new List<string>()
            {
                "QuestionLabel", "Score", "Theta", "SEE", "Info", "a", "b", "c", "d"
            };

            worksheet.Cells[1, 1].EntireRow.Font.Bold = true;
            for (int i = 1; i < headerLabels.Count+1; i++)
            {
                worksheet.Cells[1, i] = headerLabels[i-1];
            }
        }

        private void SaveWorksheet(ExcelWriter.Worksheet worksheet, ExcelWriter.Application excelApp)
        {
            excelApp.DisplayAlerts = false;
            worksheet.SaveAs(_excelLocationString, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, ExcelWriter.XlSaveAsAccessMode.xlNoChange, Type.Missing, Type.Missing, Type.Missing);
        }

        private void WriteOutputTabPersons(List<ScoreDetails> scoreDetails, ExcelWriter.Worksheet worksheet, List<string> scaleNames)
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

        private void PrintOutputTabHeader(List<string> scaleNames, ExcelWriter.Worksheet myExcelWorksheet)
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
