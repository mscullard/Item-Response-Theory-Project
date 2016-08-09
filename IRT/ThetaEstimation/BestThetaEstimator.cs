using System;
using System.Collections.Generic;
using System.Linq;
using IRT.Data;
using IRT.ModelParameters;
using IRT.Parameters;

namespace IRT.ThetaEstimation
{
    public class BestThetaEstimator
    {
        private readonly List<double> _increasingZeroVarianceStepSize;
        private readonly List<double> _decreasingZeroVarianceStepSize;
        private readonly bool _useDiscriminationParameter;
        private readonly double _gradientDescentTolerance;

        private int _zeroVarianceStepSizeCounter;

        public BestThetaEstimator(List<double> increasingZeroVarianceStepSize, List<double> decreasingZeroVarianceStepSize, bool useDiscriminationParameter, double gradientDescentTolerance)
        {
            _increasingZeroVarianceStepSize = increasingZeroVarianceStepSize;
            _decreasingZeroVarianceStepSize = decreasingZeroVarianceStepSize;
            _useDiscriminationParameter = useDiscriminationParameter;
            _gradientDescentTolerance = gradientDescentTolerance;
            _zeroVarianceStepSizeCounter = 0;
        }

        public double EstimateBestTheta(List<QuestionInfo> questionHistory, double previousTheta)
        {
            List<IModelParameters> modelParametersList = GetModelParametersList(questionHistory);
                //questionHistory.Select(x => x.Question.ModelParameters).ToList();

            List<int> responseVector = questionHistory.Select(x => (int)x.Score).ToList();

            if (AllResponsesCorrect(responseVector))
            {
                return previousTheta + GetIncreasingNonZeroVarianceStep();
            }

            if (AllResponsesIncorrect(responseVector))
            {
                return previousTheta - GetDecreasingNonZeroVarianceStep();
            }

            MaximumLikelihoodEstimator mleEstimator = new MaximumLikelihoodEstimator(modelParametersList, _gradientDescentTolerance);
            var mleOfTheta = mleEstimator.GetMle(responseVector);

            return mleOfTheta;
        }

        private List<IModelParameters> GetModelParametersList(List<QuestionInfo> questionHistory)
        {
            var modelParametersList = questionHistory.Select(x => x.Question.ModelParameters).ToList();
            if (_useDiscriminationParameter)
            {
                return modelParametersList;
            }

            List<IModelParameters> modelParametersListCopy = DeepCopy(modelParametersList);
            List<IModelParameters> noDiscriminantModelParametersList = SetDiscriminantToOne(modelParametersListCopy);

            return noDiscriminantModelParametersList;
        }

        private List<IModelParameters> SetDiscriminantToOne(List<IModelParameters> modelParametersList)
        {
            Type modelParameterType = modelParametersList[0].GetType();

            foreach (var modelParameter in modelParametersList)
            {
                if (modelParameterType == typeof(TwoParamModelParameters))
                {
                    ((TwoParamModelParameters) modelParameter).Alpha = 1;
                }
                if (modelParameterType == typeof(ThreeParamModelParameters))
                {
                    ((ThreeParamModelParameters)modelParameter).Alpha = 1;
                }
                if (modelParameterType == typeof(FourParamModelParameters))
                {
                    ((FourParamModelParameters)modelParameter).Alpha = 1;
                }
            }

            return modelParametersList;
        }

        private List<IModelParameters> DeepCopy(List<IModelParameters> modelParametersList)
        {
            List<IModelParameters> listCopy = new List<IModelParameters>();
            foreach (var parameter in modelParametersList)
            {
                listCopy.Add(parameter.DeepCopy());
            }

            return listCopy;
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
