using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using ExecutableIrt.Excel.DataObjects;
using ExecutableIrt.ExcelInteraction;
using ExecutableIrt.ExcelInteraction.DataObjects;
using IRT;
using IRT.Data;
using IRT.Data.AnswerSheetLoaders;
using IRT.Data.QuestionLoaders;
using IRT.Parameters;
using Microsoft.Office.Interop.Excel;

namespace ExecutableIrt
{
    class ExecutableIrt
    {
        public const string DefaultFileLocation = "IRT_Input";
        public const string SettingsSheetName = "Settings";
        public const string ItemInformationSheetName = "Scale Parameters";
        public const string AnswersSheetName = "Inputs";

        public const string ItemInputRequestString =
            "Enter the Excel filename containing the scale and item information. \n" +
            "Do not include the '.xlsx file extension. \n" +
            "Alternatively, you may press enter to default to '" + DefaultFileLocation + ".xlsx'. \n";
                                               

        static void Main(string[] args)
        {
            string excelLocationString = GetExcelFileLocationFromUser();
            ExcelClient excelClient = new ExcelClient();
            Workbook workbook = GetWb(excelLocationString, excelClient);

            SettingsInput settingsInput = GetSettingsInput(workbook);
            List<ItemInformation> itemInformationList = GetItemInformation(workbook);
            List<UserAnswers> answersInput = GetAnswersInput(workbook);
            workbook.Close();

            CATParameters catParameters = ConvertToCatParameters(settingsInput);

            List<string> scaleNames = itemInformationList.Select(x => x.ScaleName).Distinct().ToList();
            List<string> personNames = answersInput.Select(x => x.PersonName).Distinct().ToList();

            var scores = GetScores(scaleNames, itemInformationList, personNames, answersInput, catParameters);

            WriteOutput(excelLocationString, scores, excelClient.GetApplication());
        }

        private static Workbook GetWb(string excelLocationString, ExcelClient excelClient)
        {
            Workbook workBook = excelClient.GetSheets(excelLocationString);

            return workBook;
        }

        private static void WriteOutput(string excelLocationString, List<ScoreDetails> scores, Application app)
        {
            ExcelOutputWriter writer = new ExcelOutputWriter(excelLocationString, app);
            writer.Write(scores);
        }

        private static List<ScoreDetails> GetScores(List<string> scaleNames, List<ItemInformation> itemInformationList, List<string> personNames, List<UserAnswers> answersInput,
            CATParameters catParameters)
        {
            List<ScoreDetails> scores = new List<ScoreDetails>();
            foreach (var scaleName in scaleNames)
            {
                List<ItemInformation> itemInfoForScale = itemInformationList.Where(x => x.ScaleName.Equals(scaleName)).ToList();
                IQuestionLoader questionLoader = new ExcelQuestionLoader(itemInfoForScale, catParameters.MistakeProbability);
                foreach (var personName in personNames)
                {
                    UserAnswers personAnswers = answersInput.Single(x => x.PersonName.Equals(personName));
                    ScaleAnswers answers = personAnswers.ScaleAnswers.Single(x => x.ScaleName.Equals(scaleName));
                    IAnswerSheetLoader answerSheetLoader = new ExcelAnswerSheetLoader(answers);

                    LocationEstimator locationEstimator = new LocationEstimator(questionLoader, answerSheetLoader, catParameters);
                    List<QuestionInfo> output = locationEstimator.EstimatePersonLocation();

                    ScoreDetails scoreDetails = new ScoreDetails()
                    {
                        PersonName = personName,
                        ScaleName = scaleName,
                        Score = (double)output.Last().ThetaEstimate
                    };
                    scores.Add(scoreDetails);
                }
            }
            return scores;
        }

        private static CATParameters ConvertToCatParameters(SettingsInput settingsInput)
        {
            CATParameters catParameters = new CATParameters()
            {
                DecreasingZeroVarianceStepSize = settingsInput.DecreasingZeroVarianceStepsize,
                IncreasingZeroVarianceStepSize = settingsInput.IncreasingZeroVarianceStepsize,
                InformationCutoff = settingsInput.InformationCutoff,
                MaximumNumberOfQuestions = settingsInput.MaximumNumberOfQuestions,
                MinimumNumberOfQuestions = settingsInput.MinimumNumberOfQuestions,
                SeeCutoff = settingsInput.SeeCutoff,
                UseDiscriminationParameterForEstimation = settingsInput.UseDiscriminationParamForEstimation,
                BayesianVariance = settingsInput.BayesianVariance,
                MistakeProbability = settingsInput.MistakeProbability,
                ModelType = settingsInput.ModelType,
                NumQuestionsBeforeCatBegins = settingsInput.NumQuestionsBeforeCatBegins,
            };

            return catParameters;
        }

        private static List<UserAnswers> GetAnswersInput(Workbook wb)
        {
            Worksheet sheet = (Worksheet)wb.Sheets[AnswersSheetName];

            InputsReader inputsReader = new InputsReader();
            var answers = inputsReader.ReadAnswers(sheet);

            return answers;
        }

        private static List<ItemInformation> GetItemInformation(Workbook wb)
        {
            Worksheet sheet = (Worksheet)wb.Sheets[ItemInformationSheetName];
            ItemInformationReader reader = new ItemInformationReader();
            var itemInformation = reader.GetItemInformation(sheet);

            return itemInformation;
        }

        private static SettingsInput GetSettingsInput(Workbook wb)
        {
            Worksheet sheet = (Worksheet)wb.Sheets[SettingsSheetName];
            SettingsInputReader inputReader = new SettingsInputReader();
            SettingsInput settingsInput = inputReader.ReadSettings(sheet);

            return settingsInput;
        }

        private static string GetExcelFileLocationFromUser()
        {
            string currentDirectory = Directory.GetCurrentDirectory();
            Console.WriteLine(ItemInputRequestString);
            string parameterFileLocation = Console.ReadLine();
            if (parameterFileLocation.Equals(""))
            {
                parameterFileLocation = DefaultFileLocation;
            }

            return currentDirectory + "\\" + parameterFileLocation + ".xlsx";
        }
    }
}
