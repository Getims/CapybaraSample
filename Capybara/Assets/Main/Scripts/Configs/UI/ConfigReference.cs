using System;
using Main.Scripts.Core.Enums;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Main.Scripts.Configs.UI
{
    [Serializable]
    public class ConfigReference
    {
        [SerializeField]
        private GameState _gameState;

        [SerializeField, FilePath(ParentFolder = "Assets/Main/Resources", IncludeFileExtension = false)]
        private string _configPath = string.Empty;

        public GameState GameState => _gameState;
        public string ConfigPath => _configPath;
    }
}