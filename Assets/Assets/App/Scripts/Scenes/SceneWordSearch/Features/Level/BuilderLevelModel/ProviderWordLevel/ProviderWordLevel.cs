using System;
using App.Scripts.Scenes.SceneWordSearch.Features.Level.Models.Level;
using Assets.Assets.App.Scripts.Scenes.SceneWordSearch.Features.Level.BuilderLevelModel.ProviderWordLevel;
using Newtonsoft.Json;
using UnityEngine;

namespace App.Scripts.Scenes.SceneWordSearch.Features.Level.BuilderLevelModel.ProviderWordLevel
{
    public class ProviderWordLevel : IProviderWordLevel
    {
       
        public LevelInfo LoadLevelData(int levelIndex)
        {
            string path = $"WordSearch/Levels/{levelIndex}";
            
            TextAsset textAsset = Resources.Load<TextAsset>(path);
            LevelConfig level = JsonConvert.DeserializeObject<LevelConfig>(textAsset.text);
            LevelInfo levelInfo = new LevelInfo();

            foreach(string word in level.words)
            {
                levelInfo.words.Add(word);
            }

            return levelInfo;
        }
    }
}