using System;
using IRT.ModelParameters;

namespace IRT.ProbabilityFunctions
{
    public class FourParamProbabilityFunction : IProbabilityFunction
    {
        private readonly double _alpha;
        private readonly double _delta;
        private readonly double _chi;
        private TwoParamProbabilityFunction _twoParamProbabilityFunction;
        private double _epsilon;

        // The two parameter model corresponding to this three parameter model.  It uses the same alpha and delta but does not include the chi parameter.
        public TwoParamProbabilityFunction CorrespondingTwoParamProbFunction
        {
            get
            {
                if (_twoParamProbabilityFunction == null)
                {
                    TwoParamModelParameters parameters = new TwoParamModelParameters(_alpha, _delta);
                    _twoParamProbabilityFunction = new TwoParamProbabilityFunction(parameters);
                }

                return _twoParamProbabilityFunction;
            }
        }

        public FourParamProbabilityFunction(FourParamModelParameters parameters)
        {
            _alpha = parameters.Alpha;
            _delta = parameters.Delta;
            _chi = parameters.Chi;
            _epsilon = parameters.Epsilon;
        }

        public double ProbabilityOfCorrectResponse(double theta)
        {
            double exponential = Math.Exp(_alpha * (theta - _delta));
            double probability = _chi + (_epsilon - _chi) * exponential / (1 + exponential);

            return probability;
        }

        public double FirstThetaDerivative(double theta)
        {
            double twoParamModelDeriv = CorrespondingTwoParamProbFunction.FirstThetaDerivative(theta);
            double derivative = (_epsilon - _chi) * twoParamModelDeriv;

            return derivative;
        }

        public double SecondThetaDerivative(double theta)
        {
            double twoParamModelSecondDeriv = CorrespondingTwoParamProbFunction.SecondThetaDerivative(theta);
            double derivative = (_epsilon - _chi) * twoParamModelSecondDeriv;

            return derivative;
        }
    }
}
