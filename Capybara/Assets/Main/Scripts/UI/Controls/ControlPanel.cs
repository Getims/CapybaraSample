using System;
using System.Collections.Generic;
using Main.Scripts.Configs.Global;
using Main.Scripts.Core.Enums;
using Main.Scripts.Data.Services;
using Main.Scripts.Infrastructure.Providers.Configs;
using Main.Scripts.Infrastructure.Providers.Events;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Main.Scripts.UI.Controls
{
    public class ControlPanel : MonoBehaviour
    {
        [SerializeField]
        private List<ControlButton> _controlButtons = new List<ControlButton>();

        [SerializeField]
        private Image _stockIcon;

        private GlobalConfig _globalConfig;
        private GlobalEventProvider _globalEventProvider;
        private Func<string> GetStockIdFromData;
        private Func<bool> GetFriendsNotification;
        private Func<bool> GetDailyTaskClaimed;

        private GameState _currentButton = GameState.MainMenu;

        public event Action<GameState> OnControlButtonClick;

        [Inject]
        public void Construct(IGlobalConfigProvider globalConfigProvider, IPlayerDataService playerDataService,
            GlobalEventProvider globalEventProvider, IFriendsDataService friendsDataService,
            ITasksDataService tasksDataService)
        {
            _globalEventProvider = globalEventProvider;
            _globalConfig = globalConfigProvider.Config;

            GetStockIdFromData = () => playerDataService.StockId;
            GetFriendsNotification = () => friendsDataService.HasNotification;
            GetDailyTaskClaimed = () => tasksDataService.IsDailyTaskClaimed;
        }

        public void Initialize()
        {
            foreach (ControlButton controlButton in _controlButtons)
            {
                controlButton.Initialize(OnButtonClick);

                switch (controlButton.ButtonType)
                {
                    case GameState.Friends:
                        controlButton.SetNotification(GetFriendsNotification());
                        break;
                    case GameState.Tasks:
                        controlButton.SetNotification(!GetDailyTaskClaimed());
                        break;
                }
            }

            _controlButtons[0].SetSelected(true);
            SetupStock(GetStockIdFromData());
        }

        private void Start()
        {
            _globalEventProvider.TryToSwitchGameState.AddListener(OnButtonClick);
            _globalEventProvider.StockChangedEvent.AddListener(SetupStock);
        }

        private void OnDestroy()
        {
            _globalEventProvider.TryToSwitchGameState.RemoveListener(OnButtonClick);
            _globalEventProvider.StockChangedEvent.RemoveListener(SetupStock);
        }

        private void SetupStock(string stockId)
        {
            StockConfig stockConfig = _globalConfig.GetStockConfig(stockId);
            if (_stockIcon != null)
                _stockIcon.sprite = stockConfig.StockIcon;
        }

        private void OnButtonClick(GameState buttonType)
        {
            foreach (ControlButton controlButton in _controlButtons)
                controlButton.SetSelected(controlButton.ButtonType == buttonType);

            _currentButton = buttonType;
            OnControlButtonClick?.Invoke(buttonType);
        }
    }
}