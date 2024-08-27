using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Main.Scripts.Configs.TapGame
{
    [Serializable]
    public class MoneyConfig
    {
        [SerializeField, MinValue(1)]
        private int _moneyPerTap = 1;

        [SerializeField, MinValue(0)]
        private int _moneyTapPerAccountLevel = 1;

        [SerializeField, ListDrawerSettings(ShowIndexLabels = true), LabelWidth(80)]
        private List<long> _upgradeCost;

        [SerializeField, MinValue(0)]
        private int _moneyTapPerUpgradeLevel = 1;

        public int MoneyPerTap => _moneyPerTap;
        public int MoneyTapPerAccountLevel => _moneyTapPerAccountLevel;
        public IReadOnlyCollection<long> UpgradeCost => _upgradeCost;
        public int MoneyTapPerUpgradeLevel => _moneyTapPerUpgradeLevel;

        public long GetUpgradeCost(int upgradeLevel)
        {
            if (upgradeLevel >= _upgradeCost.Count)
                return -1;

            return _upgradeCost[upgradeLevel];
        }
    }
}