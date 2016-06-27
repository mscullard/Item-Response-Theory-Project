using System;
using System.Collections.Generic;
using System.IO;

namespace IRT.Data
{
    public class TextAnswerSheetLoader  : IAnswerSheetLoader
    {
        private readonly string _filename;

        public TextAnswerSheetLoader(string filename)
        {
            _filename = filename;
        }

        public Dictionary<string, int> LoadAnswerSheet()
        {
            Dictionary<string, int> scoreSheet = new Dictionary<string, int>();
            using (var reader = new StreamReader(_filename))
            {
                string line;
                while ((line = reader.ReadLine()) != null)
                {
                    string[] words = line.Split(' ');
                    string questionNumber = Convert.ToString(words[0]);
                    int itemScore = Convert.ToInt32(words[1]);

                    scoreSheet[questionNumber] = itemScore;
                }
            }

            return scoreSheet;
        }
    }

    public interface IAnswerSheetLoader
    {
        Dictionary<string, int> LoadAnswerSheet();
    }
}