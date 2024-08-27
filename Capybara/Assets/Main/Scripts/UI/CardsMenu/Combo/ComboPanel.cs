using System;
using System.Collections.Generic;
using System.Linq;
using Main.Scripts.Configs.Cards;
using Main.Scripts.Data.Services;
using Main.Scripts.GameLogic;
using Main.Scripts.Infrastructure.Providers.Configs;
using Main.Scripts.Infrastructure.Providers.Events;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Main.Scripts.UI.CardsMenu.Combo
{
    public class ComboPanel : MonoBehaviour
    {
        [SerializeField]
        private DotsController _dotsController;

        [SerializeField]
        private List<ComboCard> _comboCards = new List<ComboCard>();

        [SerializeField]
        private Button _infoButton;

        [SerializeField]
        private Button _rewardButton;

        [SerializeField]
        private GameObject _fakeRewardButton;

        [SerializeField]
        private TextMeshProUGUI _rewardCounter;

        [SerializeField]
        private TextMeshProUGUI _fakeRewardCounter;

        private CardsGroupConfig _cardsConfig;
        private ICardsDataService _cardsDataService;
        private UpgradeEventProvider _upgradeEventProvider;

        public event Action OnInfoClick;
        public event Action OnRewardClick;

        [Inject]
        public void Construct(ICardsConfigProvider cardsConfigProvider, ICardsDataService cardsDataService,
            UpgradeEventProvider upgradeEventProvider)
        {
            _upgradeEventProvider = upgradeEventProvider;
            _cardsDataService = cardsDataService;
            _cardsConfig = cardsConfigProvider.Config;

            _upgradeEventProvider.ComboCardBoughtEvent.AddListener(OnCardBought);
        }

        public void Initialize()
        {
            SetReward(_cardsConfig.ComboConfig.ComboReward);
            SetDots(_cardsDataService.ComboCards.Count);
            SetCards();
            SetupRewardButton();
        }

        public void SetupRewardButton()
        {
            _fakeRewardButton.SetActive(_cardsDataService.ComboClaimed);
        }

        private void Start()
        {
            _infoButton.onClick.AddListener(OnInfoButtonClick);
            _rewardButton.onClick.AddListener(OnRewardButtonClick);
        }

        protected void OnDestroy()
        {
            _infoButton.onClick.RemoveListener(OnInfoButtonClick);
            _rewardButton.onClick.RemoveListener(OnRewardButtonClick);
            _upgradeEventProvider.ComboCardBoughtEvent.RemoveListener(OnCardBought);
        }

        private void SetDots(int count) =>
            _dotsController.SetDots(count);

        private void SetReward(long reward)
        {
            _rewardCounter.text = $"+ {MoneyConverter.ConvertToSpaceValue(reward)}";
            _fakeRewardCounter.text = $"+ {MoneyConverter.ConvertToSpaceValue(reward)}";
        }

        private void SetCards()
        {
            string[] comboCards = _cardsDataService.ComboCards.ToArray();
            int openedCardsCount = comboCards.Length;

            for (int i = 0; i < _comboCards.Count; i++)
            {
                CardConfig cardConfig = null;
                if (i < openedCardsCount)
                {
                    string cardId = comboCards[i];
                    cardConfig = _cardsConfig.GetCardConfig(cardId);
                }

                _comboCards[i].Initialize(cardConfig);
            }
        }

        private void OnCardBought()
        {
            SetDots(_cardsDataService.ComboCards.Count);
            SetCards();
        }

        private void OnInfoButtonClick() =>
            OnInfoClick?.Invoke();

        private void OnRewardButtonClick()
        {
            if (_cardsDataService.ComboComplete && !_cardsDataService.ComboClaimed)
                OnRewardClick?.Invoke();
        }
    }
}