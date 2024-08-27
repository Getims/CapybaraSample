using System;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Main.Scripts.Configs.TapGame
{
    [Serializable]
    public class TurboConfig
    {
        [SerializeField]
        private bool _turboEnabled = false;

        [ShowIf(nameof(_turboEnabled))]
        [SerializeField, MinValue(1)]
        private int _turboOnScreenTime = 3;

        [ShowIf(nameof(_turboEnabled))]
        [SerializeField, MinValue(1)]
        private int _turboWorkTime = 10;

        [ShowIf(nameof(_turboEnabled))]
        [SerializeField, MinValue(1)]
        private float _moneyBoostMultiplier = 2;

        [ShowIf(nameof(_turboEnabled))]
        [SerializeField, PropertyRange(0, 1)]
        private float _energyBoostMultiplier = 0;

        [ShowIf(nameof(_turboEnabled))]
        [SerializeField, PropertyRange(0, 100)]
        private int _turboSpawnChance = 50;

        [ShowIf(nameof(_turboEnabled))]
        [SerializeField, MinMaxSlider(1, 600)]
        private Vector2Int _timeBetweenTurboSpawn = new Vector2Int(10, 60);

        [ShowIf(nameof(_turboEnabled))]
        [SerializeField, MinValue(0)]
        private int _turboCountPerDay = 3;

        [ShowIf(nameof(_turboEnabled))]
        [SerializeField, MinValue(0)]
        private int _turboCountPerAccountLevel = 1;

        public bool TurboEnabled => _turboEnabled;
        public int TurboOnScreenTime => _turboOnScreenTime;
        public int TurboWorkTime => _turboWorkTime;
        public float MoneyBoostMultiplier => _moneyBoostMultiplier;
        public float EnergyBoostMultiplier => _energyBoostMultiplier;
        public int TurboSpawnChance => _turboSpawnChance;
        public Vector2Int TimeBetweenTurboSpawn => _timeBetweenTurboSpawn;
        public int TurboCountPerDay => _turboCountPerDay;
        public int TurboCountPerAccountLevel => _turboCountPerAccountLevel;
    }
}