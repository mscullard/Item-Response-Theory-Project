using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using ExecutableIrt.ExcelInteraction.DataObjects;
using IRT.Parameters;
using Microsoft.Office.Interop.Excel;

namespace ExecutableIrt.ExcelInteraction
{
    public class SettingsInputReader
    {
        private string MinNumQuestionsCell = "B3";
        private string MaxNumQuestionsCell = "B4";
        private string StartingThetaRow = "B5:Z5";
        private string SeeCutoffCell = "B6";
        private string InformationCutoffCell = "B7";
        private string StepSizeIncreasingRange = "B8:Z8";
        private string StepSizeDecreasingRange = "B9:Z9";
        private string NumQuestionsBeforeCatBeginsCell = "B10";        
        private string MistakeProbabilityCell = "B11";
        private string UseDiscriminationParameterForEstimationCell = "B12";
        private string ModelTypeCell = "B13";
        private string BayesianVarianceCell = "B14";

        public SettingsInput ReadSettings(Worksheet sheet)
        {
            SettingsInput settingsInputReader = new SettingsInput()
            {
                DecreasingZeroVarianceStepsize = GetDecreasingStepSize(sheet),
                IncreasingZeroVarianceStepsize = GetIncreasingStepSize(sheet),
                InformationCutoff = GetInformationCutoff(sheet),
                MaximumNumberOfQuestions = GetMaxNumQuestions(sheet),
                MinimumNumberOfQuestions = GetMinNumQuestions(sheet),
                SeeCutoff = GetSeeCutoff(sheet),
                StartingThetaList = GetStartingThetaList(sheet),
                MistakeProbability = GetMistakeProbability(sheet),
                UseDiscriminationParamForEstimation = GetUseDiscriminationParamForEstimation(sheet),
                NumQuestionsBeforeCatBegins = GetNumQuestionsBeforeCatBegins(sheet)
            };

            return settingsInputReader;
        }

        private int GetNumQuestionsBeforeCatBegins(Worksheet sheet)
        {
            string row = CellReader.GetCell(NumQuestionsBeforeCatBeginsCell, sheet);

            return Convert.ToInt32(row);
        }

        private bool GetUseDiscriminationParamForEstimation(Worksheet sheet)
        {
            string row = CellReader.GetCell(UseDiscriminationParameterForEstimationCell, sheet);
            row = row.ToLower();

            return String.Equals(row, "true") ;
        }

        private double GetMistakeProbability(Worksheet sheet)
        {
            string row = CellReader.GetCell(MistakeProbabilityCell, sheet);

            return Convert.ToDouble(row);
        }

        private List<double> GetDecreasingStepSize(Worksheet sheet)
        {
            List<string> row = CellReader.GetRange(StepSizeDecreasingRange, sheet);

            return row.Select(Convert.ToDouble).ToList();
        }

        private List<double> GetIncreasingStepSize(Worksheet sheet)
        {
            List<string> row = CellReader.GetRange(StepSizeIncreasingRange, sheet);

            return row.Select(Convert.ToDouble).ToList();
        }

        private double GetInformationCutoff(Worksheet sheet)
        {
            string row = CellReader.GetCell(InformationCutoffCell, sheet);

            return Convert.ToDouble(row);
        }

        private double GetSeeCutoff(Worksheet sheet)
        {
            string row = CellReader.GetCell(SeeCutoffCell, sheet);

            return Convert.ToDouble(row);
        }

        private List<double> GetStartingThetaList(Worksheet sheet)
        {
            List<string> row = CellReader.GetRange(StartingThetaRow, sheet);

            return row.Select(Convert.ToDouble).ToList();
        }

        private int GetMinNumQuestions(Worksheet sheet)
        {
            string row = CellReader.GetCell(MinNumQuestionsCell, sheet);

            return Convert.ToInt32(row);
        }

        private int GetMaxNumQuestions(Worksheet sheet)
        {
            string row = CellReader.GetCell(MaxNumQuestionsCell, sheet);

            return Convert.ToInt32(row);
        }
    }
}
