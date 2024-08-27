using System;
using System.Collections;
using System.Collections.Generic;
using Lean.Pool;
using Main.Scripts.Configs.Cards;
using Main.Scripts.Data.DataObjects;
using Main.Scripts.Data.Services;
using Main.Scripts.GameLogic.Cards;
using Main.Scripts.Infrastructure.Providers.Configs;
using Main.Scripts.UI.Common;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Main.Scripts.UI.CardsMenu.Cards
{
    public class CardsPanel : MonoBehaviour
    {
        [SerializeField]
        private Transform _cardsContainer;

        [SerializeField]
        private LeanGameObjectPool _cardsPool;

        [SerializeField]
        private PopupResizer _popupResizer;

        [SerializeField]
        private ContentSizeFitter _mainSizeFilter;

        [SerializeField]
        private VerticalLayoutGroup _mainVerticalLayoutGroup;

        private ICardsDataService _cardsDataService;
        private CardsGroupConfig _cardsConfig;
        private ICardsConditionChecker _cardsConditionChecker;
        private int _currentCardsGroupId;
        private Coroutine _showCO;
        private List<Card> _cards = new List<Card>();

        public event Action<CardConfig> OnCardClick;

        [Inject]
        public void Construct(ICardsConfigProvider cardsConfigProvider, ICardsDataService cardsDataService,
            ICardsConditionChecker cardsConditionChecker)
        {
            _cardsConditionChecker = cardsConditionChecker;
            _cardsDataService = cardsDataService;
            _cardsConfig = cardsConfigProvider.Config;
        }

        public void Initialize()
        {
            _currentCardsGroupId = -1;
        }

        public void Refresh()
        {
            foreach (Card card in _cards)
            {
                CardData cardData = _cardsDataService.GetCardData(card.CardId);
                card.UpdateInfo(cardData);
            }
        }

        public void ShowCardsFromGroup(int cardsGroupId)
        {
            if (_currentCardsGroupId == cardsGroupId)
                return;

            if (_showCO != null)
                StopCoroutine(_showCO);

            _cards.Clear();
            _cardsPool.DespawnAll();
            _currentCardsGroupId = cardsGroupId;

            CardGroup cardGroup = _cardsConfig.GetCardGroup(cardsGroupId);
            if (cardGroup == null)
                return;

            _showCO = StartCoroutine(ShowCO(cardGroup));
        }

        private void OnDestroy() =>
            StopAllCoroutines();

        private IEnumerator ShowCO(CardGroup cardGroup)
        {
            int i = 0;
            foreach (CardConfig cardConfig in cardGroup.Cards)
            {
                CardData cardData = _cardsDataService.GetCardData(cardConfig.CardId);
                ShowCard(cardConfig, cardData);
                if (i % 10 == 0)
                    yield return null;
            }

            int fixCount = (int) (cardGroup.Cards.Count / 5) + 5;

            for (int j = 0; j < fixCount; j++)
            {
                yield return null;
                yield return null;
                _mainVerticalLayoutGroup.enabled = false;
                _mainSizeFilter.enabled = false;
                _popupResizer.Resize();
                yield return null;
                _mainSizeFilter.enabled = true;
                _mainVerticalLayoutGroup.enabled = true;
            }
        }

        private void ShowCard(CardConfig cardConfig, CardData cardData)
        {
            GameObject instance = _cardsPool.Spawn(_cardsContainer);
            if (instance == null)
                return;

            Card card = instance.GetComponent<Card>();
            card.Initialize(cardConfig, cardData, _cardsConditionChecker.CanBuyCard, OnCardClick);
            _cards.Add(card);
        }
    }
}