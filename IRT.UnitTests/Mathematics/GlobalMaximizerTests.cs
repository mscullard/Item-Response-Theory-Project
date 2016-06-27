using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IRT.Mathematics;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace IRT.UnitTests.Mathematics
{
    [TestClass]
    public class GlobalMaximizerTests
    {
        public double Tolerance = 1e-5;

        [TestMethod]
        public void FindMaximize_FunctionWithSeveralEqualGlobalMinima_ReturnsAValueWhichGivesMax()
        {
            OneDimensionalFunction function = Math.Sin;
            OneDimensionalFunction derivativeFunction = Math.Cos;
            double lowerBound = -5;
            double upperBound = 5;
            GlobalMaximizer maximizer = new GlobalMaximizer(function, derivativeFunction, lowerBound, upperBound);

            var x = maximizer.FindPosOfMaximum();
            double max = function(x);

            double expectedMax = 1;
            Assert.AreEqual(expectedMax, max, Tolerance);
        }

        [TestMethod]
        public void FindMaximize_MaxiumOccursAtBoundary_ReturnsCorrectValue()
        {
            OneDimensionalFunction function = x => -x*x*x*x -x*x*x +11*x*x +9*x - 18;
            OneDimensionalFunction derivativeFunction = x => -4*x*x*x -3*x*x + 22*x + 9 ;
            double lowerBound = -5;
            double upperBound = 5;
            GlobalMaximizer maximizer = new GlobalMaximizer(function, derivativeFunction, lowerBound, upperBound);

            var maxLocation = maximizer.FindPosOfMaximum();

            double expectedMaxPostion = 2.20582;
            Assert.AreEqual(expectedMaxPostion, maxLocation, Tolerance);
        }
    }
}
