﻿using System;
using System.Collections.Generic;
using Main.Scripts.Data.Core;
using Main.Scripts.Data.DataObjects;
using Main.Scripts.Data.SaveLoad;
using UnityEngine;

namespace Main.Scripts.Data
{
    public class GameDatabase : IDatabase
    {
        private readonly List<GameData> _allData;
        private readonly ISaveLoadService _saveLoadService;

        public GameDatabase()
        {
            _saveLoadService = new PlayerPrefsSaveLoadService();
            _allData = new List<GameData>();
        }

        public void Initialize()
        {
            _allData.Add(new PlayerData());
            _allData.Add(new CardsData());
            _allData.Add(new TapGameData());
            _allData.Add(new FriendsData());
            _allData.Add(new TasksData());

            ReloadData();
        }

        public void ReloadData()
        {
            int iterations = _allData.Count;

            for (int i = 0; i < iterations; i++)
            {
                GameData gameData = _allData[i];
                _saveLoadService.TryLoadData(ref gameData);
            }
        }

        public void SaveData()
        {
            int iterations = _allData.Count;

            for (int i = 0; i < iterations; i++)
            {
                GameData gameData = _allData[i];
                _saveLoadService.TrySaveData(gameData);
            }
        }

        public void DeleteData()
        {
            int iterations = _allData.Count;

            for (int i = 0; i < iterations; i++)
            {
                GameData gameData = _allData[i];
                _saveLoadService.TryDeleteData(ref gameData);
            }
        }

        public IEnumerable<GameData> GetAllData() =>
            _allData;

        public T GetData<T>() where T : GameData
        {
            Type type = typeof(T);

            foreach (GameData data in _allData)
            {
                bool isMatches = data.GetType() == type;

                if (isMatches)
                    return (T) data;
            }

            LogDataNotFound(type);
            return null;
        }

        private void LogDataNotFound(Type dataType)
        {
            string errorLog = $"Data of type <gb>({dataType}</gb> <rb>not found</rb>!";
            Debug.LogError(errorLog);
        }
    }
}