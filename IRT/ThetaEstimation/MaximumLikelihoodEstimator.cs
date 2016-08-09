using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using IRT.Mathematics;
using IRT.ModelParameters;

namespace IRT.ThetaEstimation
{
    public class MaximumLikelihoodEstimator
    {
        private readonly List<IModelParameters> _modelParametersList;
        private readonly double _gradientDescentTolerance;

        public MaximumLikelihoodEstimator(List<IModelParameters> modelParametersList, double gradientDescentTolerance)
        {
            _modelParametersList = modelParametersList;
            _gradientDescentTolerance = gradientDescentTolerance;
        }

        public double GetMle(List<int> responseVector)
        {
            LogLikelihoodFunction logLikelihoodFunction = new LogLikelihoodFunction(_modelParametersList);

            //const double initialGuess = 0;
            OneDimensionalFunction function = x => logLikelihoodFunction.LogLikelihood(responseVector, x);
            OneDimensionalFunction firstDerivativeFunction = x => logLikelihoodFunction.LogLikelihoodFirstDerivative(responseVector, x);
            //OneDimensionalFunction secondDerivativeFunction = x => logLikelihoodFunction.LogLikelihoodSecondDerivative(responseVector, x);
            //NewtonRhapsonSolver rootSolver = new NewtonRhapsonSolver(firstDerivativeFunction, secondDerivativeFunction, initialGuess);
            //BisectionSolver rootSolver = new BisectionSolver(firstDerivativeFunction, -6, 6);

            //double mle = rootSolver.FindRoot();
            //double mle = rootSolver.FindRoot();

            GlobalMaximizer globalMaximizer = new GlobalMaximizer(function, firstDerivativeFunction, -6, 6, _gradientDescentTolerance);
            var mle = globalMaximizer.FindPosOfMaximum();

            return mle;
        }
    }
}

