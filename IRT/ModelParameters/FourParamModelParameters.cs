namespace IRT.ModelParameters
{
    public class FourParamModelParameters : IModelParameters
    {
        // Epsilon is upper asymptote.  1 - epsilon is the probability an infinite theta makes a mistake in answering.
        public FourParamModelParameters(double alpha, double delta, double chi, double epsilon)
        {
            Alpha = alpha;
            Delta = delta;
            Chi = chi;
            Epsilon = epsilon;
        }

        public double Alpha;
        public double Delta;
        public double Chi;
        public double Epsilon;
    }
}