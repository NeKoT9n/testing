using System;
using System.Collections.Generic;
using App.Scripts.Libs.Factory;
using App.Scripts.Scenes.SceneWordSearch.Features.Level.Models.Level;

namespace App.Scripts.Scenes.SceneWordSearch.Features.Level.BuilderLevelModel
{
    public class FactoryLevelModel : IFactory<LevelModel, LevelInfo, int>
    {
        public LevelModel Create(LevelInfo value, int levelNumber)
        {
            var model = new LevelModel();

            model.LevelNumber = levelNumber;

            model.Words = value.words;
            model.InputChars = BuildListChars(value.words);

            return model;
        }

        private List<char> BuildListChars(List<string> words)
        {
            Dictionary<char, int> information = new Dictionary<char, int>();

            foreach (string word in words)
            {
                Dictionary<char, int> infoForCurrentWord = new Dictionary<char, int>();

                foreach(char symbol in word)
                {
                    if(infoForCurrentWord.TryGetValue(symbol, out int count))
                    {
                        infoForCurrentWord[symbol] = count + 1;
                    }
                    else
                    {
                        infoForCurrentWord.Add(symbol, 1);
                    }
                }

                foreach (KeyValuePair<char, int> current in infoForCurrentWord)
                {
                    if(information.TryGetValue(current.Key, out int count))
                    {
                        if(count < current.Value)
                        {
                            information[current.Key] = current.Value;
                        }
                    }
                    else
                    {
                        information.Add(current.Key, current.Value);
                    }
                }
            }

            List<char> answer = new List<char>();

            foreach(KeyValuePair<char, int> current in information)
            {
                for(int i = 0; i < current.Value; i++)
                {
                    answer.Add(current.Key);
                }
            }

            return answer;
        }
    }
}