using System;
using System.Collections.Generic;
using System.Linq;
using IRT.Data;
using IRT.ModelParameters;

namespace IRT.ThetaEstimation
{
    public class BestThetaEstimator
    {
        private readonly List<double> _increasingZeroVarianceStepSize;
        private readonly List<double> _decreasingZeroVarianceStepSize;

        private int _zeroVarianceStepSizeCounter;

        public BestThetaEstimator(List<double> increasingZeroVarianceStepSize, List<double> decreasingZeroVarianceStepSize)
        {
            _increasingZeroVarianceStepSize = increasingZeroVarianceStepSize;
            _decreasingZeroVarianceStepSize = decreasingZeroVarianceStepSize;
            _zeroVarianceStepSizeCounter = 0;
        }

        public double EstimateBestTheta(List<QuestionInfo> questionHistory, double previousTheta)
        {
            List<IModelParameters> modelParametersList = questionHistory.Select(x => x.Question.ModelParameters).ToList();
            List<int> responseVector = questionHistory.Select(x => (int)x.Score).ToList();

            if (AllResponsesCorrect(responseVector))
            {
                return previousTheta + GetIncreasingNonZeroVarianceStep();
            }

            if (AllResponsesIncorrect(responseVector))
            {
                return previousTheta - GetDecreasingNonZeroVarianceStep();
            }

            MaximumLikelihoodEstimator mleEstimator = new MaximumLikelihoodEstimator(modelParametersList);
            var mleOfTheta = mleEstimator.GetMle(responseVector);

            return mleOfTheta;
        }

        private double GetIncreasingNonZeroVarianceStep()
        {
            double stepSize = _increasingZeroVarianceStepSize[_zeroVarianceStepSizeCounter];
            if (_zeroVarianceStepSizeCounter < _increasingZeroVarianceStepSize.Count - 1)
            {
                _zeroVarianceStepSizeCounter++;    
            }

            return stepSize;
        }

        private double GetDecreasingNonZeroVarianceStep()
        {
            double stepSize = _decreasingZeroVarianceStepSize[_zeroVarianceStepSizeCounter];
            if (_zeroVarianceStepSizeCounter < _decreasingZeroVarianceStepSize.Count - 1)
            {
                _zeroVarianceStepSizeCounter++;
            }

            return stepSize;
        }


        // MLE does not work well when the item reponses is either all correct or all incorrect.  See the bottom section on page 354 of Ayala for a discussion of this.
        // In this case, we use the strategy described in the paragraph spanning pages 378-379 of Ayala.
        private bool IsZeroVarianceReponsePattern(List<int> responseVector)
        {
            bool allResponsesIncorrect = AllResponsesIncorrect(responseVector);
            bool allResponsesCorrect = AllResponsesCorrect(responseVector);

            return allResponsesCorrect || allResponsesIncorrect;
        }

        private static bool AllResponsesCorrect(List<int> responseVector)
        {
            return responseVector.TrueForAll(x => x == 1);
        }

        private static bool AllResponsesIncorrect(List<int> responseVector)
        {
            return responseVector.TrueForAll(x => x == 0);
        }
    }
}
