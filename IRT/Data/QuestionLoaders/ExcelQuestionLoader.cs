using System.Collections.Generic;
using IRT.ModelParameters;
using IRT.Parameters;

namespace IRT.Data.QuestionLoaders
{
    public class ExcelQuestionLoader : IQuestionLoader
    {
        private readonly List<ItemInformation> _itemInformationList;

        public ExcelQuestionLoader(List<ItemInformation> itemInformationList)
        {
            _itemInformationList = itemInformationList;
        }

        public List<Question> LoadQuestions()
        {
            List<Question> questions = new List<Question>();

            foreach (var item in _itemInformationList)
            {
                IModelParameters modelParameters = new ThreeParamModelParameters(item.ParameterA, item.ParameterB, item.ParameterC);
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
