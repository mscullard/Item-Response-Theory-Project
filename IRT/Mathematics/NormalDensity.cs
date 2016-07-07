using System;

namespace IRT.Mathematics
{
    public class NormalDensity
    {
        private readonly double _mu;
        private readonly double _sigma;

        public NormalDensity(double mu, double sigma)
        {
            _mu = mu;
            _sigma = sigma;
        }

        public double N(double x)
        {
            double term1 = 1/(Math.Sqrt(2*Math.PI*_sigma*_sigma));
            double term2 = Math.Exp(-(x - _mu) * (x - _mu) / (2 * _sigma * _sigma));

            return term1*term2;
        }

    }
}
