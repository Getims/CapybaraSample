using System.Collections.Generic;
using Main.Scripts.Configs.Cards;
using Main.Scripts.Core.Enums;
using Main.Scripts.Core.Utilities;
using Main.Scripts.Data.DataObjects;
using Main.Scripts.Data.Services;
using Main.Scripts.GameLogic;
using Main.Scripts.GameLogic.Cards;
using Main.Scripts.UI.CardsMenu.Cards;
using Main.Scripts.UI.Common;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Main.Scripts.UI.CardsMenu
{
    public class CardUpgradePopup : PopupPanel
    {
        [SerializeField]
        private CardUpgradeButton _cardUpgradeButton;

        [Title("Images")]
        [SerializeField]
        private Image _cardIconSimple;

        [SerializeField]
        private Image _cardIconFullCard;

        [Title("Text")]
        [SerializeField]
        private TextMeshProUGUI _cardTitle;

        [SerializeField]
        private TextMeshProUGUI _cardInfo;

        [SerializeField]
        private TextMeshProUGUI _miningCounter;

        [SerializeField]
        private TextMeshProUGUI _upgradeCounter;

        [SerializeField]
        private GameObject _upgradeContainer;

        private ICardsDataService _cardsDataService;
        private IPlayerDataService _playerDataService;
        private IMiningController _miningController;
        private ICardsConditionChecker _cardsConditionChecker;

        private CardConfig _cardConfig;
        private CardData _cardData;
        private long _upgradeCost;

        [Inject]
        public void Construct(ICardsDataService cardsDataService, IPlayerDataService playerDataService,
            IMiningController miningController, ICardsConditionChecker cardsConditionChecker)
        {
            _cardsConditionChecker = cardsConditionChecker;
            _miningController = miningController;
            _playerDataService = playerDataService;
            _cardsDataService = cardsDataService;
        }

        public void Initialize(CardConfig cardConfig)
        {
            _cardConfig = cardConfig;
            _cardData = _cardsDataService.GetCardData(cardConfig.CardId);

            SetupInfo();
            SetupIcon();
        }

        private void SetupInfo()
        {
            _cardTitle.text = _cardConfig.LargeName;
            _cardInfo.text = _cardConfig.LargeInfo;

            bool hasData = _cardData != null;
            int level = hasData ? _cardData.CardLevel : 0;
            bool isMaxLevel = level + 1 >= _cardConfig.Mining.Count;

            SetupMiningCounter(level, isMaxLevel);

            _upgradeCost = _cardConfig.GetUpgradeCostByLevel(level);
            SetupCostCounter(isMaxLevel);
            SetupClaimButton(isMaxLevel);
        }

        private void SetupCostCounter(bool isMaxLevel)
        {
            _upgradeContainer.SetActive(!isMaxLevel);
            _upgradeCounter.text = MoneyConverter.ConvertToSpaceValue(_upgradeCost);
        }

        private void SetupClaimButton(bool isMaxLevel)
        {
            bool canBuy = _cardsConditionChecker.CanBuyCard(_cardConfig);

            bool hasCondition = false;
            string conditionText = string.Empty;

            if (!canBuy)
            {
                hasCondition = true;
                conditionText = _cardConfig.UpgradeCardCondition.CardToUpgradeInfo;
            }

            _cardUpgradeButton.Initialize(_upgradeCost <= _playerDataService.Money, isMaxLevel, hasCondition,
                conditionText);
        }

        private void SetupMiningCounter(int level, bool isMaxLevel)
        {
            long mining = 0;
            if (isMaxLevel)
            {
                mining = _cardConfig.GetMiningOnLevel(level);
                _miningCounter.text = $"{MoneyConverter.ConvertToShortValue(mining)}";
            }
            else
            {
                mining = _cardConfig.GetMiningOnLevel(level + 1) - _cardConfig.GetMiningOnLevel(level);
                _miningCounter.text = $"+{MoneyConverter.ConvertToShortValue(mining)}";
            }
        }

        private void SetupIcon()
        {
            if (_cardConfig.IconType == CardIconType.JustIcon)
            {
                _cardIconSimple.enabled = true;
                _cardIconSimple.sprite = _cardConfig.CardIcon;
                _cardIconFullCard.enabled = false;
            }
            else
            {
                _cardIconSimple.enabled = false;
                _cardIconFullCard.enabled = true;
                _cardIconFullCard.sprite = _cardConfig.CardIcon;
                _cardIconFullCard.rectTransform.pivot = Utils.GetSpritePivot(_cardConfig.CardIcon);
            }
        }

        protected override void OnClaimButtonClick()
        {
            if (_upgradeCost >= 0 && _upgradeCost <= _playerDataService.Money)
            {
                _playerDataService.SpendMoney(_upgradeCost);

                int level = 0;
                bool hasData = _cardData != null;
                if (hasData)
                    level = _cardData.CardLevel;

                level += 1;
                level = Mathf.Min(level, _cardConfig.Mining.Count - 1);

                if (hasData)
                    _cardsDataService.UpdateCardLevel(_cardConfig.CardId, level);
                else
                    _cardsDataService.AddCard(_cardConfig.CardId, level);

                CheckForComboCard(_cardConfig);

                _miningController.UpdateMoneyPerHour();
                Hide();
                base.OnClaimButtonClick();
            }
            else
                OnCloseButtonClick();
        }

        private void CheckForComboCard(CardConfig cardConfig)
        {
            IReadOnlyCollection<CardConfig> combo = _miningController.CardsCombo;

            foreach (CardConfig comboConfig in combo)
            {
                if (comboConfig.CardId.Equals(cardConfig.CardId))
                {
                    _cardsDataService.AddComboCard(cardConfig.CardId);
                    break;
                }
            }
        }
    }
}