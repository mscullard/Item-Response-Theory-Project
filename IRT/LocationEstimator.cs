using System;
using System.Collections.Generic;
using System.Linq;
using ExecutableIrt;
using IRT.Data;
using IRT.InformationFunctions;
using IRT.Parameters;
using IRT.ThetaEstimation;

namespace IRT
{
    public class LocationEstimator
    {
        private readonly IQuestionLoader _questionLoader;
        private readonly IAnswerSheetLoader _answerSheetLoader;
        private readonly CATParameters _catParameters;
        private Dictionary<string, int> _answerSheet;
        private BestThetaEstimator _bestThetaEstimator;

        // This value can be changed.  Value is chosen to match the value in Ayala.  This value is used when the MLE cannot be estimated because the response vector is either all correct
        // or all incorrect.
        private const double IndeterminantSee = double.NaN;

        // ToDo: This must be replaced in the future, since answers must be determine after asking the question.  It is used here only to test the grid given on page 379 of Ayala.
        public Dictionary<string, int> AnswerSheet
        {
            get
            {
                if (_answerSheet == null)
                {
                    _answerSheet = LoadAnswerSheet();
                }

                return _answerSheet;
            }
        }

        public LocationEstimator(IQuestionLoader questionLoader, IAnswerSheetLoader answerSheetLoader, CATParameters catParameters)
        {
            _questionLoader = questionLoader;
            _answerSheetLoader = answerSheetLoader;
            _catParameters = catParameters;
            _bestThetaEstimator = new BestThetaEstimator(catParameters.IncreasingZeroVarianceStepSize, catParameters.DecreasingZeroVarianceStepSize, catParameters.UseDiscriminationParameterForEstimation);
        }

        public List<QuestionInfo> EstimatePersonLocation()
        {
            List<Question> unaskedQuestions = LoadQuestions();
            double theta = GetInitialTheta();

            List<QuestionInfo> questionHistory = new List<QuestionInfo>();
            for (int i = 0; i < _catParameters.MaximumNumberOfQuestions && unaskedQuestions.Count != 0; i++)
            {
                QuestionInfo questionInfo = GetNextQuestion(theta, unaskedQuestions, i);
                unaskedQuestions.Remove(questionInfo.Question);

                AskQuestion(questionInfo.Question.QuestionLabel);

                questionInfo.Score = ScoreItem(questionInfo.Question.QuestionLabel);

                questionHistory.Add(questionInfo);

                theta = GetNewLocationEstimate(questionHistory, theta);
                questionHistory.Last().ThetaEstimate = theta;
                double seOfEstimate = GetSEE(questionHistory, theta);
                questionHistory.Last().SEE = seOfEstimate;

                if (TerminationConditionsSatisfied(seOfEstimate, questionInfo.Information, i + 1))
                {
                    return questionHistory;
                }
            }

            return questionHistory;
        }

        // This function must be replaced by one which actually scores the item, instead of using a pre-determined score.
        private int ScoreItem(string questionNumber)
        {
            return AnswerSheet[questionNumber];
        }

        private double GetNewLocationEstimate(List<QuestionInfo> questionHistory, double theta)
        {
            var bestThetaEstimate = _bestThetaEstimator.EstimateBestTheta(questionHistory, theta);

            return bestThetaEstimate;
        }

        private List<Question> LoadQuestions()
        {
            List<Question> questions = _questionLoader.LoadQuestions();

            return questions;
        }

        private bool TerminationConditionsSatisfied(double seOfEstimate, double information, int numQuestionsAsked)
        {
            var minNumQuestionsAsked = numQuestionsAsked >= _catParameters.MinimumNumberOfQuestions;
            var cutoffConditionSatisfied = (seOfEstimate < _catParameters.SeeCutoff || information < _catParameters.InformationCutoff) ;
            return cutoffConditionSatisfied && minNumQuestionsAsked;
        }

        private double GetSEE(List<QuestionInfo> questionHistory, double theta)
        {
            // If there the response vector consists of all correct or all incorrect answers, we use a fixed low value for the SEE.
            if (IsZeroVarianceReponsePattern(questionHistory.Select(x => (int) x.Score).ToList()))
            {
                return IndeterminantSee;
            }

            StandardErrorOfEstimateCalculator calculator = new StandardErrorOfEstimateCalculator();
            double see = calculator.Calculate(questionHistory, theta);

            return see;
        }

        private bool IsZeroVarianceReponsePattern(List<int> responseVector)
        {
            bool allResponsesIncorrect = responseVector.TrueForAll(x => x == 0);
            bool allResponsesCorrect = responseVector.TrueForAll(x => x == 1);

            return allResponsesCorrect || allResponsesIncorrect;
        }

        // For the given theta, returns the best information not yet used.
        private QuestionInfo GetNextQuestion(double theta, List<Question> unaskedQuestions, int numQuestionsAsked)
        {
            InformationFunctionFactory factory = new InformationFunctionFactory();
            double maxInformation = Double.MinValue;
            Question questionWithHighestInfo = new Question();
            if (_catParameters.NumQuestionsBeforeCatBegins > numQuestionsAsked)
            {
                var firstRemainingQuestion = unaskedQuestions.First();

                IItemInformationFunction informationFunction = factory.Build(firstRemainingQuestion.ModelParameters);
                double information = informationFunction.GetInformation(theta);

                return new QuestionInfo()
                {
                    Question = firstRemainingQuestion,
                    Information = information
                };
            }

            foreach (var question in unaskedQuestions)
            {
                IItemInformationFunction informationFunction = factory.Build(question.ModelParameters);
                double information = informationFunction.GetInformation(theta);

                if (information > maxInformation)
                {
                    maxInformation = information;
                    questionWithHighestInfo = question;
                }
            }

            return new QuestionInfo()
            {
                Question = questionWithHighestInfo,
                Information = maxInformation
            };
        }

        private Dictionary<string, int> LoadAnswerSheet()
        {
            var answerSheet = _answerSheetLoader.LoadAnswerSheet();

            return answerSheet;
        }

        private double GetInitialTheta()
        {
            // An initial theta of 0 assumes that the candidate is of average proficiency.  It is worth considering using a random initial theta which is close to 0 (say, between -.5 and .5) in
            // order to avoid 'overexposing' items.  See the last paragraph on page 376 and the following paragraph on page 377 of Ayala for a discussion.
            return 0;
        }

        private void AskQuestion(string questionNumber)
        {
            // Function is a placeholder for this step.
        }
    }
}

