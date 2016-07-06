using IRT.ModelParameters;
using IRT.ProbabilityFunctions;

namespace IRT.InformationFunctions
{
    public class FourParamItemInformationFunction : IItemInformationFunction
    {
        private readonly IProbabilityFunction _probabilityFunction;
        private readonly double _alpha;
        private readonly double _chi;
        private double _epsilon;

        public FourParamItemInformationFunction(FourParamModelParameters modelParameters)
        {
            _alpha = modelParameters.Alpha;
            _chi = modelParameters.Chi;
            _epsilon = modelParameters.Epsilon;

            _probabilityFunction = new FourParamProbabilityFunction(modelParameters);
        }

        // Formula given on page 307 of https://ppw.kuleuven.be/okp/_pdf/Magis2013ANOTI.pdf 
        public double GetInformation(double theta)
        {
            double p = _probabilityFunction.ProbabilityOfCorrectResponse(theta);
            double term1 = (p - _chi) / (_epsilon - _chi);
            double term2 = (_epsilon - p);
            double term3 = p*(1-p);

            return _alpha * _alpha * term1 * term1 * term2 * term2 / term3;
        }
    }
}
