using System;
using System.Collections.Generic;
using Main.Scripts.Configs.Accounts;
using Main.Scripts.Configs.Core;
using Main.Scripts.Core.Enums;
using Main.Scripts.Core.Utilities;
using Sirenix.OdinInspector;
using Sirenix.Utilities;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Main.Scripts.Configs.Cards
{
    [Serializable]
    public class CardConfig : ScriptableConfig
    {
        [SerializeField, ReadOnly]
        private string _cardID = Utils.GetUniqueID(8);

        [SerializeField, InlineEditor(InlineEditorModes.LargePreview, Expanded = true)]
        private Sprite _cardIcon;

        [Space(5)]
        [SerializeField]
        private CardIconType _cardIconType;

        [Title("Visual")]
        [SerializeField]
        [Tooltip("Show on card")]
        private string _shortName;

        [SerializeField]
        [Tooltip("Show on card")]
        private string _shortInfo;

        [SerializeField]
        [Tooltip("Show in card popup")]
        private string _largeName;

        [SerializeField]
        [Tooltip("Show in card popup")]
        private string _largeInfo;

        [SerializeField]
        private UIGradientConfig _backgroundGradient;

        [Title("Upgrades")]
        [SerializeField]
        private CardBuyCondition _cardBuyCondition;

        [SerializeField, ShowIf(nameof(_cardBuyCondition), CardBuyCondition.UpgradeCard)]
        private UpgradeCardCondition _upgradeCardCondition;

        [Space(10)]
        [SerializeField, ListDrawerSettings(ShowIndexLabels = true), LabelWidth(60)]
        [Tooltip("List of income per hour on level")]
        private List<long> _mining;

        [Space(10)]
        [SerializeField, ListDrawerSettings(ShowIndexLabels = true), LabelWidth(60)]
        private List<long> _upgradeCost;

        public string CardId => _cardID;
        public CardIconType IconType => _cardIconType;
        public Sprite CardIcon => _cardIcon;
        public string ShortName => _shortName.IsNullOrWhitespace() ? _largeName : _shortName;
        public string ShortInfo => _shortInfo.IsNullOrWhitespace() ? _largeInfo : _shortInfo;
        public string LargeName => _largeName.IsNullOrWhitespace() ? _shortName : _largeName;
        public string LargeInfo => _largeInfo.IsNullOrWhitespace() ? _shortInfo : _largeInfo;
        public UIGradientConfig BackgroundGradient => _backgroundGradient;
        public IReadOnlyCollection<long> Mining => _mining;
        public IReadOnlyCollection<long> UpgradeCost => _upgradeCost;

        public CardBuyCondition BuyCondition => _cardBuyCondition;
        public UpgradeCardCondition UpgradeCardCondition => _upgradeCardCondition;

        public long GetMiningOnLevel(int level)
        {
            if (level >= _mining.Count)
                level = _mining.Count - 1;

            if (level < 0)
                return 0;

            return _mining[level];
        }

        public long GetUpgradeCostByLevel(int level)
        {
            if (level >= _upgradeCost.Count)
                level = _upgradeCost.Count - 1;

            if (level < 0)
                return 0;

            return _upgradeCost[level];
        }

#if UNITY_EDITOR

        public override string GetConfigCategory() =>
            ConfigsCategories.CARDS_CATEGORY;

        [Title("Utils")]
        [Button]
        private void GenerateCardID() =>
            _cardID = Utils.GetUniqueID(8);

        [Button]
        private void GenerateRandomMiningValues(float minMoneyPerLevel = 50, float maxMoneyPerLevel = 100)
        {
            int count = _mining.Count;
            _mining.Clear();
            long current = 0;
            for (int i = 0; i < count; i++)
            {
                current += (long) Random.Range(minMoneyPerLevel, maxMoneyPerLevel);
                _mining.Add(current);
            }
        }

        [Button]
        private void GenerateRandomCostValues(float minMoneyPerLevel = 50, float maxMoneyPerLevel = 100)
        {
            int count = _upgradeCost.Count;
            _upgradeCost.Clear();
            long current = 0;
            for (int i = 0; i < count; i++)
            {
                current += (long) Random.Range(minMoneyPerLevel, maxMoneyPerLevel);
                _upgradeCost.Add(current);
            }
        }
#endif
    }
}