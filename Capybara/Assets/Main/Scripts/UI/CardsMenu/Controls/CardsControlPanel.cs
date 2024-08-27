using System;
using System.Collections.Generic;
using Main.Scripts.Configs.Cards;
using Main.Scripts.Infrastructure.Providers.Configs;
using UnityEngine;
using Zenject;

namespace Main.Scripts.UI.CardsMenu.Controls
{
    public class CardsControlPanel : MonoBehaviour
    {
        [SerializeField]
        private CardsControlButton _controlButtonPrefab;

        [SerializeField]
        private Transform _buttonsContainer;

        [SerializeField]
        private List<CardsControlButton> _controlButtons = new List<CardsControlButton>();

        private CardsGroupConfig _cardsConfig;
        private bool _buttonsSetuped = false;
        private int _currentButton = 0;

        public event Action<int> OnControlButtonClick;
        
        [Inject]
        public void Construct(ICardsConfigProvider cardsConfigProvider)
        {
            _cardsConfig = cardsConfigProvider.Config;
        }

        public void Initialize()
        {
            _currentButton = -1;
            if (_buttonsSetuped)
            {
                foreach (CardsControlButton controlButton in _controlButtons)
                    controlButton.SetSelected(false);

                SelectFirstButton();
                return;
            }

            _buttonsSetuped = true;
            int groupsCount = _cardsConfig.GroupsCount;
            int buttonsCount = _controlButtons.Count;
            if (buttonsCount < groupsCount)
            {
                CreateButtons(groupsCount - buttonsCount);
                buttonsCount = groupsCount;
            }

            for (int i = 0; i < buttonsCount; i++)
            {
                bool isActive = i < groupsCount && _cardsConfig.HasCards(i);
                if (isActive)
                {
                    CardGroup cardGroup = _cardsConfig.GetCardGroup(i);
                    _controlButtons[i].Initialize(i, cardGroup.GroupName, OnButtonClick);
                }

                _controlButtons[i].SetActive(isActive);
            }

            SelectFirstButton();
        }

        private void SelectFirstButton()
        {
            for (int i = 0; i < _controlButtons.Count; i++)
            {
                if (!_controlButtons[i].IsActive)
                    continue;

                _controlButtons[i].SetSelected(true);
                OnButtonClick(i);
                break;
            }
        }

        private void CreateButtons(int controlButtonsCount)
        {
            if (controlButtonsCount <= 0)
                return;

            for (int i = 0; i < controlButtonsCount; i++)
            {
                CardsControlButton controlButton = Instantiate(_controlButtonPrefab, _buttonsContainer);
                _controlButtons.Add(controlButton);
            }
        }

        private void OnButtonClick(int buttonId)
        {
            if(_currentButton==buttonId)
                return;
            
            int i = 0;
            foreach (CardsControlButton controlButton in _controlButtons)
            {
                controlButton.SetSelected(i == buttonId);
                i++;
            }

            _currentButton = buttonId;
            OnControlButtonClick?.Invoke(buttonId);
        }
    }
}