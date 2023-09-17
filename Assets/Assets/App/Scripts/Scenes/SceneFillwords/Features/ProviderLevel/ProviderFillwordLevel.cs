using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using App.Scripts.Scenes.SceneFillwords.Features.FillwordModels;
using DG.Tweening.Plugins.Core.PathCore;
using UnityEngine;
using UnityEngine.Windows;

namespace App.Scripts.Scenes.SceneFillwords.Features.ProviderLevel
{
    public class ProviderFillwordLevel : IProviderFillwordLevel
    {
        private const string PathToWords = "Fillwords/words_list";
        private const string PathToLevelInformation = "Fillwords/pack_0";

        private readonly string[] _words;
        private readonly Dictionary<int, List<KeyValuePair<int, List<int>>>> _levelInformationDictionary;

        public ProviderFillwordLevel()
        {
            FillWords(out _words);
            FillLevelInformation(out _levelInformationDictionary);
        }



        public GridFillWords LoadModel(int index)
        {
            index--;

            while(_levelInformationDictionary.TryGetValue(index++, out List<KeyValuePair<int, List<int>>> level))
            {
                int symbolCount = 0;

                bool error = false;

                foreach(KeyValuePair<int, List<int>> word in level)
                {

                    int symbols = _words[word.Key].Length;
                    int positions = word.Value.Count;


                    if (symbols != positions)
                    {
                        error = true;
                        break;
                    }

                    symbolCount += symbols;

                }

                if (error)
                {
                    Debug.LogError("Неверное количество позиций");
                    continue;
                }
                 
                int lines = (int)Mathf.Sqrt(symbolCount);

                if(lines * lines != symbolCount)
                {
                    continue;
                }

                GridFillWords grid = new GridFillWords(new Vector2Int(lines, lines));

                foreach (KeyValuePair<int, List<int>> word in level)
                {
                    string cachedWord = _words[word.Key];
                    int symbolIndex = 0;

                    foreach (int position in word.Value)
                    {
                        if(position >= symbolCount)
                        {
                            error = true;
                            break;
                        }
                        int x = position % lines;
                        int y = position / lines;

                        if (grid.Get(x, y) != null)
                        {
                            error = true;
                            break;
                        }
                        grid.Set(x, y, new CharGridModel(cachedWord[symbolIndex]));

                        symbolIndex++;
                    }

                    if(error)
                    {
                        break;
                    }
                }

                if (error)
                {
                    continue;
                }

                return grid;
            }

            return null;
        }

        private void FillWords(out string[] words)
        {
            TextAsset textAsset = Resources.Load<TextAsset>(PathToWords);

            if(textAsset == null)
            {
                throw new Exception();
            }


            words = textAsset.text.Split(Environment.NewLine, StringSplitOptions.RemoveEmptyEntries);
        }

        private void FillLevelInformation(out Dictionary<int, List<KeyValuePair<int, List<int>>>> levelInformationDictionary)
        {
            levelInformationDictionary = new Dictionary<int, List<KeyValuePair<int, List<int>>>>();

            TextAsset textAsset = Resources.Load<TextAsset>(PathToLevelInformation);
            string[] levelInformationString = textAsset.text.Split(Environment.NewLine);

            for (int i = 0; i < levelInformationString.Length; i++)
            {
                string[] words = levelInformationString[i].Split(' ');

                List<int> wordIndexes = new List<int>();
                List<List<int>> symbolNumbers = new List<List<int>>();
                List<KeyValuePair<int, List<int>>> levelInformation = new List<KeyValuePair<int, List<int>>>();

                for (int j = 0; j < words.Length; j += 2)
                {
                    wordIndexes.Add(Int32.Parse(words[j]));
                }

                for(int j = 1; j < words.Length; j += 2)
                {
                    List<int> numbers = new List<int>();
                    string[] stringNumbers = words[j].Split(';');
                
                    for(int k = 0; k < stringNumbers.Length; k++)
                    {
                        numbers.Add(Int32.Parse(stringNumbers[k]));
                    }

                    symbolNumbers.Add(numbers);
                }

                for(int j = 0; j < wordIndexes.Count; j++)
                {
                    levelInformation.Add(new KeyValuePair<int, List<int>>(wordIndexes[j], symbolNumbers[j]));
                }

                levelInformationDictionary.Add(i, levelInformation);
            }

        }

    }
}