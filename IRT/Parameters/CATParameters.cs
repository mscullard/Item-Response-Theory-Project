using System.Collections.Generic;

namespace ExecutableIrt
{
    public class CATParameters
    {
        public int MinimumNumberOfQuestions;
        public int MaximumNumberOfQuestions;
        public double SeeCutoff;
        public double InformationCutoff;
        public List<double> IncreasingZeroVarianceStepSize; 
        public List<double> DecreasingZeroVarianceStepSize;
    }
}
