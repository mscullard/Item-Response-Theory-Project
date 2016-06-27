using System.Collections.Generic;
using ExecutableIrt.Excel.DataObjects;

namespace IRT.Data.AnswerSheetLoaders
{
    public class ExcelAnswerSheetLoader : IAnswerSheetLoader
    {
        private readonly ScaleAnswers _scaleAnswers;

        public ExcelAnswerSheetLoader(ScaleAnswers scaleAnswers)
        {
            _scaleAnswers = scaleAnswers;
        }

        public Dictionary<string, int> LoadAnswerSheet()
        {
            return _scaleAnswers.ItemToAnswerMap;
        }
    }
}
