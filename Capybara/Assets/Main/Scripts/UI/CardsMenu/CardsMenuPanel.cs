using Coffee.UIExtensions;
using Main.Scripts.Configs.Cards;
using Main.Scripts.Data.Services;
using Main.Scripts.GameLogic.Sound;
using Main.Scripts.Infrastructure.Providers.Configs;
using Main.Scripts.Infrastructure.Providers.Events;
using Main.Scripts.UI.Base;
using Main.Scripts.UI.CardsMenu.Cards;
using Main.Scripts.UI.CardsMenu.Combo;
using Main.Scripts.UI.CardsMenu.Controls;
using Main.Scripts.UI.Common;
using Main.Scripts.UI.MainMenu;
using Main.Scripts.UI.MainMenu.Info;
using Main.Scripts.UI.MainMenu.LeaderBoard;
using Main.Scripts.UI.MainMenu.Settings;
using Main.Scripts.UI.MainMenu.Stock;
using UnityEngine;
using Zenject;

namespace Main.Scripts.UI.CardsMenu
{
    public class CardsMenuPanel : MenuPanel
    {
        [SerializeField]
        private LevelInfoPanel _levelInfoPanel;

        [SerializeField]
        private CommonInfoPanel _commonInfoPanel;

        [SerializeField]
        private MoneyCounter _moneyCounter;

        [SerializeField]
        private CardsControlPanel _controlPanel;

        [SerializeField]
        private ComboPanel _comboPanel;

        [SerializeField]
        private CardsPanel _cardsPanel;

        [SerializeField]
        private UIParticle _rewardParticles;

        private IPlayerDataService _playerDataService;
        private ISoundService _soundService;
        private ICardsDataService _cardsDataService;
        private CardsGroupConfig _cardsConfig;
        private UpgradeEventProvider _upgradeEventProvider;

        [Inject]
        public void Construct(ICardsConfigProvider cardsConfigProvider, ICardsDataService cardsDataService,
            IPlayerDataService playerDataService, ISoundService soundService, UpgradeEventProvider upgradeEventProvider)
        {
            _upgradeEventProvider = upgradeEventProvider;
            _cardsDataService = cardsDataService;
            _cardsConfig = cardsConfigProvider.Config;
            _soundService = soundService;
            _playerDataService = playerDataService;
        }

        public override void Initialize(IUIMenuFactory uiMenuFactory, UIContainerProvider containerProvider)
        {
            base.Initialize(uiMenuFactory, containerProvider);

            _commonInfoPanel.Initialize();
            _moneyCounter.UpdateInfo();
            _levelInfoPanel.UpdateInfo();
            _cardsPanel.Initialize();
            _controlPanel.Initialize();
            _comboPanel.Initialize();
        }

        public override void Show()
        {
            base.Show();
        }

        public override void Hide()
        {
            base.Hide();
            CloseAllPopups();
        }

        private void Start()
        {
            _upgradeEventProvider.ComboCardBoughtEvent.AddListener(OnComboCardBought);

            _levelInfoPanel.OnLevelClick += OpenLeaderBoardWindow;
            _commonInfoPanel.OnStockClick += OpenStocksWindow;
            _commonInfoPanel.OnInfoClick += OpenInfoPopup;
            _commonInfoPanel.OnSettingsClick += OpenSettingsWindow;
            _comboPanel.OnInfoClick += OpenComboInfoPopup;
            _comboPanel.OnRewardClick += OpenComboRewardPopup;
            _controlPanel.OnControlButtonClick += OnControlButtonClick;
            _cardsPanel.OnCardClick += OpenCardPopup;
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();

            _upgradeEventProvider.ComboCardBoughtEvent.RemoveListener(OnComboCardBought);

            _levelInfoPanel.OnLevelClick -= OpenLeaderBoardWindow;
            _commonInfoPanel.OnStockClick -= OpenStocksWindow;
            _commonInfoPanel.OnInfoClick -= OpenInfoPopup;
            _commonInfoPanel.OnSettingsClick -= OpenSettingsWindow;
            _comboPanel.OnInfoClick -= OpenComboInfoPopup;
            _comboPanel.OnRewardClick -= OpenComboRewardPopup;
            _controlPanel.OnControlButtonClick -= OnControlButtonClick;
            _cardsPanel.OnCardClick -= OpenCardPopup;
        }

        private void OpenLeaderBoardWindow() =>
            OpenWindow<LeaderBordPanel>();

        private void OpenSettingsWindow()
        {
            SettingsPanel window = OpenWindow<SettingsPanel>();
            window.OnStockClick += OpenStocksWindow;
            window.OnLanguageClick += OpenLanguageWindow;
        }

        private void OpenStocksWindow()
        {
            StockChangePanel window = OpenWindow<StockChangePanel>();
            window.OnStockClick += OpenStockChangedPopup;
        }

        private void OpenLanguageWindow() =>
            OpenWindow<LanguageChangePanel>();

        private void OpenInfoPopup() =>
            OpenPopup<MiningInfoPopup>(null, null);

        private void OpenStockChangedPopup() =>
            OpenPopup<StockChangedPopup>(null, null);

        private void OpenComboRewardPopup()
        {
            long reward = _cardsConfig.ComboConfig.ComboReward;
            var popup = OpenPopup<ComboCompletePopup>(null, () => AddMoneyReward(reward));
            popup.Initialize(reward);
        }

        private void OpenComboFindPopup()
        {
            ComboFindPopup popup = _uiMenuFactory.Create<ComboFindPopup>(_uiContainerProvider.WindowsContainer);
            _popups.Add(popup);
            popup.Show();
        }

        private void OpenComboInfoPopup() =>
            OpenPopup<ComboInfoPopup>(null, null);

        public void OpenCardPopup(CardConfig cardConfig)
        {
            var popup = OpenPopup<CardUpgradePopup>(null, () =>
            {
                _cardsPanel.Refresh();
                ShowRewardParticles();
                _soundService.PlayCardBoughtSound();
            });
            popup.Initialize(cardConfig);
        }

        private void AddMoneyReward(long reward)
        {
            _playerDataService.AddMoney(reward);
            _cardsDataService.SetComboClaimed();
            ShowRewardParticles();
            _soundService.PlayComboCompleteSound();
            _comboPanel.SetupRewardButton();
        }

        private void ShowRewardParticles()
        {
            _rewardParticles.Play();
        }

        private void OnControlButtonClick(int cardsGroupId)
        {
            _cardsPanel.ShowCardsFromGroup(cardsGroupId);
        }

        private void OnComboCardBought()
        {
            if (_cardsDataService.ComboCards.Count < 3)
                OpenComboFindPopup();
            else
                OpenComboRewardPopup();
        }
    }
}