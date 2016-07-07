using System;
using System.Collections.Generic;
using System.Linq;
using ExecutableIrt;
using IRT.Data;
using IRT.Parameters;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace IRT.UnitTests
{
    [TestClass]
    // Tests use setup on page 379 of Ayala.  Note that since we are using fewer questions, the questions numbers do not match.
    public class LocationEstimatorTests
    {
        private double Tolerance = 1e-3;

        private TextQuestionLoader _textQuestionLoader = new TextQuestionLoader(" C:\\Users\\Mike\\Documents\\Visual Studio 2013\\Projects\\IRT\\ModelParameters.txt");
        private TextQuestionLoader _textQuestionLoader2 = new TextQuestionLoader(" C:\\Users\\Mike\\Documents\\Visual Studio 2013\\Projects\\IRT\\ModelParameters2.txt");
        private TextAnswerSheetLoader _textAnswerSheetLoader = new TextAnswerSheetLoader(" C:\\Users\\Mike\\Documents\\Visual Studio 2013\\Projects\\IRT\\MockScoreSheet.txt");
        private TextAnswerSheetLoader _textAnswerSheetLoader2 = new TextAnswerSheetLoader("C:\\Users\\Mike\\Documents\\Visual Studio 2013\\Projects\\IRT\\MockScoreSheet2.txt");
        private TextAnswerSheetLoader _textAnswerSheetLoader3 = new TextAnswerSheetLoader(" C:\\Users\\Mike\\Documents\\Visual Studio 2013\\Projects\\IRT\\MockScoreSheet3.txt");
        private TextAnswerSheetLoader _textAnswerSheetLoader4 = new TextAnswerSheetLoader(" C:\\Users\\Mike\\Documents\\Visual Studio 2013\\Projects\\IRT\\MockScoreSheet4.txt");

        [TestMethod]
        public void EstimatePersonLocation_DataFromAyala_ReturnsCorrectFinalTheta()
        {
            CATParameters catParameters = GetDefaultCatParameters();
            LocationEstimator locationEstimator = new LocationEstimator(_textQuestionLoader, _textAnswerSheetLoader, catParameters);
            List<QuestionInfo> questionHistory = locationEstimator.EstimatePersonLocation();

            double estimatedTheta = (double) questionHistory.Last().ThetaEstimate;
            Assert.IsTrue(Math.Abs(estimatedTheta - 1.098) < Tolerance);
        }

        [TestMethod]
        public void EstimatePersonLocation_DataFromAyala_ReturnsCorrectFinalSee()
        {
            CATParameters catParameters = GetDefaultCatParameters();
            LocationEstimator locationEstimator = new LocationEstimator(_textQuestionLoader, _textAnswerSheetLoader, catParameters);
            List<QuestionInfo> questionHistory = locationEstimator.EstimatePersonLocation();

            double estimatedSee = (double)questionHistory.Last().SEE;
            Assert.IsTrue(Math.Abs(estimatedSee - .192) < Tolerance);
        }

        [TestMethod]
        public void EstimatePersonLocation_DataFromAyala_ReturnsCorrectFinalInfo()
        {
            CATParameters catParameters = GetDefaultCatParameters();
            LocationEstimator locationEstimator = new LocationEstimator(_textQuestionLoader, _textAnswerSheetLoader, catParameters);
            List<QuestionInfo> questionHistory = locationEstimator.EstimatePersonLocation();

            double estimatedInformation = (double)questionHistory.Last().Information;
            Assert.IsTrue(Math.Abs(estimatedInformation - .901) < Tolerance);
        }

        [TestMethod]
        public void EstimatePersonLocation_DataFromAyala_ReturnsCorrectFinalQuestion()
        {
            CATParameters catParameters = GetDefaultCatParameters();
            LocationEstimator locationEstimator = new LocationEstimator(_textQuestionLoader, _textAnswerSheetLoader, catParameters);
            List<QuestionInfo> questionHistory = locationEstimator.EstimatePersonLocation();

            string finalQuestion = questionHistory.Last().Question.QuestionLabel;
            Assert.IsTrue(Convert.ToInt32(finalQuestion) == 205);
        }

        [TestMethod]
        public void EstimatePersonLocation_CompletesAnswerSheet2()
        {
            try
            {
                CATParameters catParameters = GetDefaultCatParameters();
                LocationEstimator locationEstimator = new LocationEstimator(_textQuestionLoader, _textAnswerSheetLoader2, catParameters);
                List<QuestionInfo> questionHistory = locationEstimator.EstimatePersonLocation();
 
                Assert.IsTrue(true);
            }
            catch(Exception)
            {
                Assert.IsTrue(false);
            }
        }
 
        [TestMethod]
        public void EstimatePersonLocation_CompletesAnswerSheet3()
        {
            CATParameters catParameters = GetDefaultCatParameters();
            LocationEstimator locationEstimator = new LocationEstimator(_textQuestionLoader, _textAnswerSheetLoader, catParameters);
            List<QuestionInfo> questionHistory = locationEstimator.EstimatePersonLocation();
        }

        [TestMethod]
        public void EstimatePersonLocation_BisectionSolverFailing()
        {
            CATParameters catParameters = GetDefaultCatParameters();
            LocationEstimator locationEstimator = new LocationEstimator(_textQuestionLoader2, _textAnswerSheetLoader4, catParameters);
            List<QuestionInfo> x = locationEstimator.EstimatePersonLocation();
        }

        private CATParameters GetDefaultCatParameters()
        {
            CATParameters catParameters = new CATParameters()
            {
                InformationCutoff = .9,
                MaximumNumberOfQuestions = Int16.MaxValue,
                MinimumNumberOfQuestions = Int16.MinValue,
                SeeCutoff = Double.NegativeInfinity,
                //IncreasingZeroVarianceStepSize = .3
            };

            return catParameters;
        }
    }
}