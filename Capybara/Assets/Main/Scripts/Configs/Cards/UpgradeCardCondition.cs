using System;
using Main.Scripts.Core.Constants;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Main.Scripts.Configs.Cards
{
    [Serializable]
    public class UpgradeCardCondition
    {
        [SerializeField, Required]
        private CardConfig _cardToUpgrade;

        [SerializeField, MinValue(1)]
        private int _targetLevel = 1;

        public string CardToUpgradeId => _cardToUpgrade.CardId;

        public string CardToUpgradeInfo => $"{_cardToUpgrade.ShortName} {Phrases.LEVEL} {_targetLevel}";
        /*
         _targetLevel > 1
        ? $"{_cardToUpgrade.ShortName} lvl {_targetLevel}"
        : $"{_cardToUpgrade.ShortName}";
        */

        public int TargetLevel => _targetLevel;
    }
}