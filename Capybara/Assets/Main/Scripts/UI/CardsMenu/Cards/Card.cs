using System;
using Coffee.UIEffects;
using Main.Scripts.Configs.Accounts;
using Main.Scripts.Configs.Cards;
using Main.Scripts.Core.Constants;
using Main.Scripts.Core.Enums;
using Main.Scripts.Core.Utilities;
using Main.Scripts.Data.DataObjects;
using Main.Scripts.GameLogic;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Main.Scripts.UI.CardsMenu.Cards
{
    public class Card : MonoBehaviour
    {
        [SerializeField]
        private Button _button;

        [Title("Images")]
        [SerializeField]
        private UIGradient _backgroundGradient;

        [SerializeField]
        private Image _cardIconSimple;

        [SerializeField]
        private Image _cardIconFullCard;

        [SerializeField]
        private Image _lockIcon;

        [SerializeField]
        private CanvasGroup _cardIconCG;

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
        private TextMeshProUGUI _upgradeInfo;

        [SerializeField]
        private TextMeshProUGUI _levelCounter;

        private CardConfig _cardConfig;
        private CardData _cardData;
        private Action<CardConfig> _onCardClick;
        private Func<CardConfig, bool> CanBuy;
        private bool _canBuy;

        public string CardId => _cardConfig != null ? _cardConfig.CardId : string.Empty;

        public void Initialize(CardConfig cardConfig, CardData cardData, Func<CardConfig, bool> canBuy,
            Action<CardConfig> onCardClick)
        {
            CanBuy = canBuy;
            _onCardClick = onCardClick;
            _cardData = cardData;
            _cardConfig = cardConfig;

            if (_cardConfig == null)
            {
                gameObject.SetActive(false);
                return;
            }

            gameObject.SetActive(true);
            _canBuy = CanBuy.Invoke(_cardConfig);

            SetupInfo();
            SetupIcon();
            SetupBackground();
        }

        public void UpdateInfo(CardData cardData)
        {
            _cardData = cardData;
            if (_cardConfig == null)
            {
                gameObject.SetActive(false);
                return;
            }

            gameObject.SetActive(true);
            _canBuy = CanBuy.Invoke(_cardConfig);

            SetupInfo();
            SetupIcon();
        }

        private void Start() =>
            _button.onClick.AddListener(OnButtonClick);

        private void OnDestroy() =>
            _button.onClick.RemoveListener(OnButtonClick);

        private void SetupInfo()
        {
            _cardTitle.text = _cardConfig.ShortName;
            _cardInfo.text = _cardConfig.ShortInfo;

            bool hasData = _cardData != null;
            int level = hasData ? _cardData.CardLevel : 0;
            _levelCounter.text = $"{Phrases.LEVEL} {level}";

            SetupMiningCounter(level, hasData);
            SetupCostCounter(level);
        }

        private void SetupCostCounter(int level)
        {
            long cost = _cardConfig.GetUpgradeCostByLevel(level);
            bool isMaxLevel = level + 1 >= _cardConfig.Mining.Count;

            if (_canBuy)
            {
                _upgradeCounter.gameObject.SetActive(!isMaxLevel);
                _upgradeInfo.gameObject.SetActive(isMaxLevel);

                _upgradeCounter.text = MoneyConverter.ConvertToShortValue(cost);
                _upgradeInfo.text = Phrases.CARD_MAX_LEVEL;
            }
            else
            {
                _upgradeCounter.gameObject.SetActive(false);
                _upgradeInfo.gameObject.SetActive(true);
                _upgradeInfo.text = _cardConfig.UpgradeCardCondition.CardToUpgradeInfo;
            }
        }

        private void SetupMiningCounter(int level, bool hasData)
        {
            long mining = _cardConfig.GetMiningOnLevel(level);

            _miningCounter.text = hasData
                ? MoneyConverter.ConvertToShortValue(mining)
                : $"+{MoneyConverter.ConvertToShortValue(mining)}";
        }

        private void SetupIcon()
        {
            _cardIconCG.alpha = _canBuy ? 1 : 0.5f;
            _lockIcon.enabled = !_canBuy;

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

        private void SetupBackground()
        {
            UIGradientConfig uiGradientConfig = _cardConfig.BackgroundGradient;
            _backgroundGradient.direction = uiGradientConfig.Direction;
            _backgroundGradient.color1 = uiGradientConfig.Color1;
            _backgroundGradient.color2 = uiGradientConfig.Color2;
            _backgroundGradient.color3 = uiGradientConfig.TopLeft;
            _backgroundGradient.color4 = uiGradientConfig.TopRight;
            _backgroundGradient.rotation = uiGradientConfig.Rotation;
            _backgroundGradient.offset = uiGradientConfig.Offset1;
            _backgroundGradient.offset2 = new Vector2(uiGradientConfig.Offset2, uiGradientConfig.Offset1);
            _backgroundGradient.gradientStyle = uiGradientConfig.GradientStyle;
        }

        private void OnButtonClick() =>
            _onCardClick?.Invoke(_cardConfig);
    }
}