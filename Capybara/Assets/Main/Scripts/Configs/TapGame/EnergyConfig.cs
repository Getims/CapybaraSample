using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Main.Scripts.Configs.TapGame
{
    [Serializable]
    public class EnergyConfig
    {
        [SerializeField, MinValue(1)]
        private int _startEnergy = 500;

        [SerializeField, MinValue(0)]
        [Tooltip("When calculating recovery the amount of energy will be rounded to an integer")]
        private float _recoveryPerSecond = 1f;

        [SerializeField, MinValue(0)]
        private float _recoveryPerAccountLevel = 0.2f;

        [SerializeField]
        private List<long> _upgradeCost;

        [SerializeField, MinValue(1)]
        private int _energyPerUpgrade = 500;

        [SerializeField, MinValue(1)]
        private int _energyPerAccountLevel = 500;

        [SerializeField, MinValue(0)]
        private int _fullRecoveryCount = 3;

        [SerializeField, MinValue(0)]
        private int _fullRecoveryPerAccountLevel = 1;

        [SerializeField, MinValue(0), MaxValue(23)]
        private int _hoursBetweenFullRecoveryUse = 1;

        public int StartEnergy => _startEnergy;
        public float RecoveryPerSecond => _recoveryPerSecond;
        public float RecoveryPerAccountLevel => _recoveryPerAccountLevel;
        public IReadOnlyCollection<long> UpgradeCost => _upgradeCost;
        public int EnergyPerUpgrade => _energyPerUpgrade;
        public int EnergyPerAccountLevel => _energyPerAccountLevel;
        public int FullRecoveryCount => _fullRecoveryCount;
        public int FullRecoveryPerAccountLevel => _fullRecoveryPerAccountLevel;
        public int HoursBetweenFullRecoveryUse => _hoursBetweenFullRecoveryUse;

        public long GetUpgradeCost(int upgradeLevel)
        {
            if (upgradeLevel >= _upgradeCost.Count)
                return -1;

            return _upgradeCost[upgradeLevel];
        }
    }
}