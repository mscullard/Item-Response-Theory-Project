using System;

namespace IRT.Mathematics
{
    public class GradientDescentExtremaFinder
    {
        private readonly OneDimensionalFunction _derivativeFunction;
        private const int MaxIterations = 5000;
        private double Alpha = .01;
        private const double Tolerance = .000001;

        public GradientDescentExtremaFinder(OneDimensionalFunction derivativeFunction)
        {
            _derivativeFunction = derivativeFunction;
        }

        public double FindMinimum(double initialGuess)
        {
            return InternalFindMin(initialGuess, _derivativeFunction);
        }

        private double InternalFindMin(double initialGuess, OneDimensionalFunction derivativeFunction)
        {
            double x = initialGuess;
            for (int i = 0; i < MaxIterations; i++)
            {
                var derivative = derivativeFunction(x);
                double newX = x - Alpha*derivative;
                if (DifferenceWithinTolerance(x, newX))
                {
                    return newX;
                }

                x = newX;
            }

            throw new Exception(String.Format("Unable to find minimum using gradient descent within {0} iterations",
                MaxIterations));
        }

        public double FindMaximum(double initialGuess)
        {
            OneDimensionalFunction negativeOfDerivative = x => -_derivativeFunction(x);
            return InternalFindMin(initialGuess, negativeOfDerivative);
        }

        private bool DifferenceWithinTolerance(double x1, double x2)
        {
            return Math.Abs(x1 - x2) < Tolerance;
        }
    }
}
