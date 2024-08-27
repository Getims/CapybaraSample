using Main.Scripts.Core.Enums;
using Main.Scripts.Data.Services;
using Main.Scripts.GameLogic;
using Main.Scripts.GameLogic.External;
using Main.Scripts.Infrastructure.Providers.Events;
using Main.Scripts.UI.Base;
using Main.Scripts.UI.Common;
using Main.Scripts.UI.Common.FlyIcons;
using Main.Scripts.UI.MainMenu.Boosters;
using Main.Scripts.UI.MainMenu.Info;
using Main.Scripts.UI.MainMenu.LeaderBoard;
using Main.Scripts.UI.MainMenu.Settings;
using Main.Scripts.UI.MainMenu.Stock;
using Main.Scripts.UI.TapGame;
using UnityEngine;
using Zenject;

namespace Main.Scripts.UI.MainMenu
{
    public class MainMenuPanel : MenuPanel
    {
        [SerializeField]
        private AccountInfoPanel _accountInfoPanel;

        [SerializeField]
        private LevelInfoPanel _levelInfoPanel;

        [SerializeField]
        private CommonInfoPanel _commonInfoPanel;

        [SerializeField]
        private MoneyCounter _moneyCounter;

        [SerializeField]
        private TapGamePanel _tapGamePanel;

        [SerializeField]
        private TapInfoPanel _tapInfoPanel;

        [SerializeField]
        private FlyIconsSpawner _flyIconsSpawner;

        private GlobalEventProvider _globalEventProvider;
        private IPlayerDataService _playerDataService;
        private IMiningController _miningController;
        private IRemoteDataService _remoteDataService;

        private bool _miningChecked = false;

        [Inject]
        public void Construct(GlobalEventProvider globalEventProvider, IPlayerDataService playerDataService,
            IMiningController miningController, IRemoteDataService remoteDataService)
        {
            _remoteDataService = remoteDataService;
            _miningController = miningController;
            _playerDataService = playerDataService;
            _globalEventProvider = globalEventProvider;
        }

        public override void Initialize(IUIMenuFactory uiMenuFactory, UIContainerProvider containerProvider)
        {
            base.Initialize(uiMenuFactory, containerProvider);

            _tapGamePanel.Initialize(containerProvider);
            _commonInfoPanel.Initialize();
            _accountInfoPanel.Initialize(_remoteDataService.GetPlayerIcon(), _remoteDataService.GetPlayerName());
            _moneyCounter.UpdateInfo();
            _levelInfoPanel.UpdateInfo();
        }

        public override void Show()
        {
            base.Show();

            if (_playerDataService.StockId == string.Empty)
            {
                OpenStocksWindow();
                return;
            }

            if (_miningChecked == false)
            {
                _miningChecked = true;
                long miningMoney = _miningController.MoneyFromLastPlay;
                if (miningMoney > 0)
                    OpenMiningPopup(miningMoney);
            }
        }

        public override void Hide()
        {
            base.Hide();
            CloseAllPopups();
        }

        private void Start()
        {
            _levelInfoPanel.OnLevelClick += OpenLeaderBoardWindow;
            _commonInfoPanel.OnStockClick += OpenStocksWindow;
            _commonInfoPanel.OnInfoClick += OpenInfoPopup;
            _commonInfoPanel.OnSettingsClick += OpenSettingsWindow;
            _tapInfoPanel.OnBoosterClick += OpenBoosterWindow;
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();
            _levelInfoPanel.OnLevelClick -= OpenLeaderBoardWindow;
            _commonInfoPanel.OnStockClick -= OpenStocksWindow;
            _commonInfoPanel.OnInfoClick -= OpenInfoPopup;
            _commonInfoPanel.OnSettingsClick -= OpenSettingsWindow;
            _tapInfoPanel.OnBoosterClick -= OpenBoosterWindow;
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

        private void OpenBoosterWindow()
        {
            BoostersPanel window = OpenWindow<BoostersPanel>();
            window.OnFreeEnergyClick += OpenFreeEnergyPopup;
            window.OnUpgradeEnergyClick += OpenUpgradeEnergyPopup;
            window.OnUpgradeTapClick += OpenUpgradeTapPopup;
        }

        private void OpenInfoPopup() =>
            OpenPopup<MiningInfoPopup>(null, TryToEnableMiningState);

        private void OpenMiningPopup(long miningMoney)
        {
            MiningWorkPopup popup = OpenPopup<MiningWorkPopup>(null, ShowMoneyReward);
            popup.Initialize(_playerDataService.StockId, miningMoney);
            _playerDataService.AddMoney(miningMoney, false);
        }

        private void OpenFreeEnergyPopup() =>
            OpenPopup<FreeEnergyPopup>(null, CloseAllPopups);

        private void OpenUpgradeTapPopup() =>
            OpenPopup<UpgradeTapPopup>(null, CloseAllPopups);

        private void OpenUpgradeEnergyPopup() =>
            OpenPopup<UpgradeEnergyPopup>(null, CloseAllPopups);

        private void OpenStockChangedPopup() =>
            OpenPopup<StockChangedPopup>(null, CloseAllPopups);

        private void ShowMoneyReward()
        {
            _flyIconsSpawner.StartAnimation(_flyIconsSpawner.transform.position, _moneyCounter.IconPosition);
            _globalEventProvider.MoneyChangedEvent.Invoke(_playerDataService.Money);
        }

        private void TryToEnableMiningState() =>
            _globalEventProvider.TryToSwitchGameState.Invoke(GameState.Mining);
    }
}