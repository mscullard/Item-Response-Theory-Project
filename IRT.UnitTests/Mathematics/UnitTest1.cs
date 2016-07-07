using System;
using IRT.Mathematics;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace IRT.UnitTests.Mathematics
{
    [TestClass]
    public class NormalDensityTests
    {
        [TestMethod]
        public void NormalDistribution_StandardNormal_ReturnsCorrectValues()
        {
            double mu = 0;
            double sigma = 1;

            NormalDensity normalDensity = new NormalDensity(mu, sigma);
            double tolerance = 1e-6;

            var output1 = normalDensity.N(0);
            double expectedOutput1 = 0.39894228;
            Assert.AreEqual(expectedOutput1, output1, tolerance);

            var output2 = normalDensity.N(1);
            double expectedOutput2 = 0.241970725;
            Assert.AreEqual(expectedOutput2, output2, tolerance);

            var output3 = normalDensity.N(-1);
            double expectedOutput3 = 0.241970725;
            Assert.AreEqual(expectedOutput3, output3, tolerance);
        }

        [TestMethod]
        public void NormalDistribution_NonStandardNormal_ReturnsCorrectValues()
        {
            double mu = .5;
            double sigma = 2;

            NormalDensity normalDensity = new NormalDensity(mu, sigma);
            double tolerance = 1e-6;

            var output1 = normalDensity.N(0);
            double expectedOutput1 = 0.193334058;
            Assert.AreEqual(expectedOutput1, output1, tolerance);

            var output2 = normalDensity.N(1);
            double expectedOutput2 = 0.193334058;
            Assert.AreEqual(expectedOutput2, output2, tolerance);

            var output3 = normalDensity.N(-1);
            double expectedOutput3 = 0.150568716;
            Assert.AreEqual(expectedOutput3, output3, tolerance);
        }
    }
}
