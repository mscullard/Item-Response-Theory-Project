using System.Collections.Generic;

namespace IRT.Data
{
    public interface IQuestionLoader
    {
        List<Question> LoadQuestions();
    }
}