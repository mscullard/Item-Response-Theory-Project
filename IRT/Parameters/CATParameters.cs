using System.Collections.Generic;

namespace IRT.Parameters
{
    public class CATParameters
    {
        public int MinimumNumberOfQuestions;
        public int MaximumNumberOfQuestions;
        public double SeeCutoff;
        public double InformationCutoff;
        public List<double> IncreasingZeroVarianceStepSize; 
        public List<double> DecreasingZeroVarianceStepSize;
        public int NumQuestionsBeforeCatBegins;
        public double MistakeProbability;
        public ModelType ModelType;
        public double? BayesianVariance;
        public bool UseDiscriminationParameterForEstimation;
    }
}
