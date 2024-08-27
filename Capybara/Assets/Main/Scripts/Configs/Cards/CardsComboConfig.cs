using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Main.Scripts.Configs.Cards
{
    [Serializable]
    public class CardsComboConfig
    {
        [SerializeField, ListDrawerSettings(ShowIndexLabels = true, ListElementLabelName = "ShortName")]
        private List<CardConfig> _testCombo = new List<CardConfig>();

        [SerializeField, MinValue(0)]
        private long _comboReward;

        public IReadOnlyCollection<CardConfig> TestCombo => _testCombo;
        public long ComboReward => _comboReward;
    }
}