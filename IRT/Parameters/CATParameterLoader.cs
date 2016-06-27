using System;
using System.IO;

namespace ExecutableIrt
{
    public static class CATParameterLoader
    {
        public static CATParameters Load(string fileLocation)
        {
            CATParameters parameters = new CATParameters();
            using (var reader = new StreamReader(fileLocation))
            {
                parameters.MinimumNumberOfQuestions = Convert.ToInt32(reader.ReadLine());
                parameters.MaximumNumberOfQuestions = Convert.ToInt32(reader.ReadLine());
                parameters.SeeCutoff = Convert.ToDouble(reader.ReadLine());
                parameters.InformationCutoff = Convert.ToDouble(reader.ReadLine());
                parameters.IncreasingZeroVarianceStepSize = Convert.ToDouble(reader.ReadLine());                
            }

            return parameters;
        }

        public static void PrintParameterRequestOrder()
        {
            Console.WriteLine("The file containing the parameters to use for computized adaptive testing should be in the following format, with one value per line:");
            Console.WriteLine("Line 1.): The minimum number of questions to ask.");
            Console.WriteLine("Line 2.): The maximum number of questions to ask.");
            Console.WriteLine("Line 3.): The standard error cutoff.");
            Console.WriteLine("Line 4.): The information cutoff");
            Console.WriteLine("Line 5.): The stepsize to use in the case where there are only correct or only incorrect answers.");
        }
    }
}
