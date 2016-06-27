using System;
using System.Collections.Generic;
using System.Linq;
using ExecutableIrt.Excel.DataObjects;
using Microsoft.Office.Interop.Excel;

namespace ExecutableIrt.ExcelInteraction
{
    public class InputsReader
    {
        private string PersonLabelsColumn = "A1:A1000";
        private int RowOffset = 3;
        private string FinalColumn = "ZZ";
        private int IdRowIndex = 2;
        private int PersonNameIndex = 0;
        private int ItemNameStartingIndex = 1;
        private int ScaleRowIndex = 1;
        private int NumEmptyScores = 1;

        public List<UserAnswers> ReadAnswers(Worksheet sheet)
        {
            int numPeople = GetNumPeople(sheet);

            List<UserAnswers> answersList = new List<UserAnswers>();
            for (int i = 0; i < numPeople; i++)
            {
                UserAnswers userAnswers = ReadPerson(sheet, i + RowOffset);
                answersList.Add(userAnswers);
            }

            return answersList;
        }

        private UserAnswers ReadPerson(Worksheet sheet, int personRowIndex)
        {
            string itemRange = "A" + IdRowIndex + ":" + FinalColumn + IdRowIndex;
            string personRange = "A" + personRowIndex + ":" + FinalColumn + personRowIndex;
            string scaleRange = "A" + ScaleRowIndex + ":" + FinalColumn + ScaleRowIndex;

            List<string> scaleRow = CellReader.GetRange(scaleRange, sheet);
            List<string> itemRow = CellReader.GetRange(itemRange, sheet).Skip(NumEmptyScores).ToList();
            List<string> scoresRow = CellReader.GetRange(personRange, sheet);
            string personName = scoresRow[0];
            scoresRow = scoresRow.Skip(NumEmptyScores).ToList();

            List<string> scaleNames = scaleRow.Distinct().ToList();

            List<ScaleAnswers> scaleAnswersList = new List<ScaleAnswers>();

            foreach (var scaleName in scaleNames)
            {
                List<int> indices = Enumerable.Range(0, scaleRow.Count)
                                        .Where(index => scaleRow[index] == scaleName)
                                        .ToList();

                Dictionary<string, int> itemsToAnswersMap = new Dictionary<string, int>();
                for (int k = 0; k < indices.Count; k++)
                {
                    var index = indices[k];
                    var itemName = itemRow[index];
                    itemsToAnswersMap[itemName] = Convert.ToInt32(scoresRow[index]);
                }

                ScaleAnswers scaleAnswers = new ScaleAnswers()
                {
                    ScaleName = scaleName,
                    ItemToAnswerMap = itemsToAnswersMap
                };

                scaleAnswersList.Add(scaleAnswers);
            }
            

            UserAnswers answers = new UserAnswers()
            {
                PersonName = personName,
                ScaleAnswers = scaleAnswersList
            };

            return answers; 
        }

        private int GetNumPeople(Worksheet sheet)
        {
            List<string> personsList = CellReader.GetRange(PersonLabelsColumn, sheet);

            return personsList.Count - 1; // Substract 1 for ID
        }
    }
}