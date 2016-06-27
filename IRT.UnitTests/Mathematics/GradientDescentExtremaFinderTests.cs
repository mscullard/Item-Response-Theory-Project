using System;
using IRT.Mathematics;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace IRT.UnitTests.Mathematics
{
    [TestClass]
    public class GradientDescentExtremaFinderTests
    {
        private const double Tolerance = 1e-4;

        [TestMethod]
        public void FindMinimum_XSquared_ReturnsZero()
        {
            OneDimensionalFunction derivative = x => 2*x;
            GradientDescentExtremaFinder extremaFinder = new GradientDescentExtremaFinder(derivative);

            double initialGuess = 3;
            var calculatedMinimum = extremaFinder.FindMinimum(initialGuess);

            double expectedMinimum = 0;

            Assert.AreEqual(expectedMinimum, calculatedMinimum, Tolerance);
        }

        [TestMethod]
        public void FindMinimum_SinX_ReturnsNegativePiOverTwo()
        {
            OneDimensionalFunction derivative = x => Math.Cos(x);
            GradientDescentExtremaFinder extremaFinder = new GradientDescentExtremaFinder(derivative);

            double initialGuess = 0;
            var calculatedMinimum = extremaFinder.FindMinimum(initialGuess);

            double expectedMinimum = -Math.PI/2;

            Assert.AreEqual(expectedMinimum, calculatedMinimum, Tolerance);
        }


        [TestMethod]
        public void FindMaximum_SinX_ReturnsPiOverTwo()
        {
            OneDimensionalFunction derivative = x => Math.Cos(x);
            GradientDescentExtremaFinder extremaFinder = new GradientDescentExtremaFinder(derivative);

            double initialGuess = 0;
            var calculatedMinimumLoc = extremaFinder.FindMaximum(initialGuess);

            double expectedMinimumLoc = Math.PI / 2;

            Assert.AreEqual(expectedMinimumLoc, calculatedMinimumLoc, Tolerance);
        }

        [TestMethod, ExpectedException(typeof(Exception))]
        public void FindMaximum_XSquared_ThrowsExceptions()
        {
            OneDimensionalFunction derivative = x => 2 * x;
            GradientDescentExtremaFinder extremaFinder = new GradientDescentExtremaFinder(derivative);

            double initialGuess = 3;
            var calculatedMinimum = extremaFinder.FindMaximum(initialGuess);

            double expectedMinimum = 0;

            Assert.AreEqual(expectedMinimum, calculatedMinimum, Tolerance);
        }
    }
}
