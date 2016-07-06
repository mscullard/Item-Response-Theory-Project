using System.Collections.Generic;
using IRT.ModelParameters;
using IRT.Parameters;

namespace IRT.Data.QuestionLoaders
{
    public class ExcelQuestionLoader : IQuestionLoader
    {
        private readonly List<ItemInformation> _itemInformationList;
        private readonly double _mistakeProbability;

        public ExcelQuestionLoader(List<ItemInformation> itemInformationList)
        {
            _itemInformationList = itemInformationList;
        }

        public ExcelQuestionLoader(List<ItemInformation> itemInformationList, double mistakeProbability)
        {
            _itemInformationList = itemInformationList;
            _mistakeProbability = mistakeProbability;
        }

        public List<Question> LoadQuestions()
        {
            List<Question> questions = new List<Question>();

            foreach (var item in _itemInformationList)
            {
                IModelParameters modelParameters = new FourParamModelParameters(item.ParameterA, item.ParameterB, item.ParameterC, 1 - _mistakeProbability);
                Question question = new Question()
                {
                    ModelParameters = modelParameters,
                    QuestionLabel = item.ItemName
                };

                questions.Add(question);
            }

            return questions;
        }
    }
}
