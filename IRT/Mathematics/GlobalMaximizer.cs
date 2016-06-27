using System;

namespace IRT.Mathematics
{
    public class GlobalMaximizer
    {
        public double LowerBound { get; set; }
        private readonly OneDimensionalFunction _function;
        private readonly OneDimensionalFunction _derivativeFunction;
        private double _lowerBound;
        private double _upperBound;
        private const double _searchStepSize = .1;
        private const double EqualityTolerance = 1e-10;

        public GlobalMaximizer(OneDimensionalFunction function, OneDimensionalFunction derivativeFunction, double lowerBound, double upperBound)
        {
            _lowerBound = lowerBound;
            _upperBound = upperBound;
            _function = function;
            _derivativeFunction = derivativeFunction;
        }

        public double FindPosOfMaximum()
        {
            double bestGuessForMax = LinearSearch();
            if (BestGuessIsBoundaryValue(bestGuessForMax))
            {
                return HandleBoundaryCase(bestGuessForMax);
            }

            var max = PerformGradientDescent(bestGuessForMax);

            return max;
        }

        private double PerformGradientDescent(double bestGuessForMax)
        {
            GradientDescentExtremaFinder extremaFinder = new GradientDescentExtremaFinder(_derivativeFunction);
            var positionOfMax = extremaFinder.FindMaximum(bestGuessForMax);
            return positionOfMax;
        }

        private double HandleBoundaryCase(double bestGuessForMax)
        {
            try
            {
                var maxPostion = PerformGradientDescent(bestGuessForMax);
                return maxPostion;
            }
            catch (Exception)
            {
                return bestGuessForMax;
            }
        }

        private bool BestGuessIsBoundaryValue(double bestGuessForMax)
        {
            return Equal(bestGuessForMax, _upperBound) || Equal(bestGuessForMax, _lowerBound);
        }

        private bool Equal(double x, double y)
        {
            return Math.Abs(x - y) < EqualityTolerance;
        }

        private double LinearSearch()
        {
            double locationOfBestMax = double.NegativeInfinity;
            double bestMax = double.NegativeInfinity;
            for (double x = _lowerBound; x <= _upperBound; x += _searchStepSize)
            {
                double y = _function(x);
                if (y > bestMax)
                {
                    bestMax = y;
                    locationOfBestMax = x;
                }
            }

            return locationOfBestMax;
        }
    }
}
