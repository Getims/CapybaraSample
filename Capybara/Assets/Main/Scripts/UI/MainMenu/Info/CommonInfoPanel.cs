using System;
using Main.Scripts.Configs.Global;
using Main.Scripts.Data.Services;
using Main.Scripts.GameLogic;
using Main.Scripts.Infrastructure.Providers.Configs;
using Main.Scripts.Infrastructure.Providers.Events;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Main.Scripts.UI.MainMenu.Info
{
    public class CommonInfoPanel : MonoBehaviour
    {
        [SerializeField]
        private TextMeshProUGUI _moneyGain;

        [SerializeField]
        private Image _stockIcon;

        [SerializeField]
        private Button _stockButton;

        [SerializeField]
        private Button _infoButton;

        [SerializeField]
        private Button _settingsButton;

        private GlobalConfig _globalConfig;
        private GlobalEventProvider _globalEventProvider;
        private ICardsDataService _cardsDataService;
        private Func<string> GetStockIdFromData;
        private UpgradeEventProvider _upgradeEventProvider;

        public event Action OnStockClick;
        public event Action OnInfoClick;
        public event Action OnSettingsClick;

        [Inject]
        public void Construct(IGlobalConfigProvider globalConfigProvider, IPlayerDataService playerDataService,
            GlobalEventProvider globalEventProvider, UpgradeEventProvider upgradeEventProvider,
            ICardsDataService cardsDataService)
        {
            _upgradeEventProvider = upgradeEventProvider;
            _globalEventProvider = globalEventProvider;
            _globalConfig = globalConfigProvider.Config;
            _cardsDataService = cardsDataService;
            GetStockIdFromData = () => playerDataService.StockId;
        }

        public void Initialize()
        {
            SetupMoney(_cardsDataService.MoneyPerHour);
            SetupStock(GetStockIdFromData());
        }

        private void Start()
        {
            _stockButton.onClick.AddListener(OnLevelButtonClick);
            _infoButton.onClick.AddListener(OnInfoButtonClick);
            _settingsButton.onClick.AddListener(OnSettingsButtonClick);

            _globalEventProvider.StockChangedEvent.AddListener(SetupStock);
            _upgradeEventProvider.MiningUpgradeEvent.AddListener(UpdateMoneyInfo);
        }

        protected void OnDestroy()
        {
            _stockButton.onClick.RemoveListener(OnLevelButtonClick);
            _infoButton.onClick.RemoveListener(OnInfoButtonClick);
            _settingsButton.onClick.RemoveListener(OnSettingsButtonClick);

            _globalEventProvider.StockChangedEvent.RemoveListener(SetupStock);
            _upgradeEventProvider.MiningUpgradeEvent.RemoveListener(UpdateMoneyInfo);
        }

        private void UpdateMoneyInfo() =>
            SetupMoney(_cardsDataService.MoneyPerHour);

        private void SetupStock(string stockId)
        {
            StockConfig stockConfig = _globalConfig.GetStockConfig(stockId);
            _stockIcon.sprite = stockConfig.StockIcon;
        }

        private void SetupMoney(long money)
        {
            _moneyGain.text = $"+{MoneyConverter.ConvertToShortValue(money)}";
        }

        private void OnLevelButtonClick() =>
            OnStockClick?.Invoke();

        private void OnInfoButtonClick() =>
            OnInfoClick?.Invoke();

        private void OnSettingsButtonClick() =>
            OnSettingsClick?.Invoke();
    }
}