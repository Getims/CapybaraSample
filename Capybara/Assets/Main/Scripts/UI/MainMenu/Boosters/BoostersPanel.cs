using System;
using Main.Scripts.Configs.Global;
using Main.Scripts.Configs.TapGame;
using Main.Scripts.Core.Constants;
using Main.Scripts.Core.Utilities;
using Main.Scripts.Data.Services;
using Main.Scripts.GameLogic;
using Main.Scripts.GameLogic.External;
using Main.Scripts.GameLogic.Timers;
using Main.Scripts.Infrastructure.Providers.Configs;
using Main.Scripts.UI.Base;
using Main.Scripts.UI.Common;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Main.Scripts.UI.MainMenu.Boosters
{
    public class BoostersPanel : UIPanel
    {
        [SerializeField]
        private Button _closeButton;

        [SerializeField]
        private Button _boostersInfoButton;

        [SerializeField]
        private BoostersButton _freeEnergyButton;

        [SerializeField]
        private BoostersButton _turboButton;

        [SerializeField]
        private BoostersButton _energyUpgradeButton;

        [SerializeField]
        private BoostersButton _tapUpgradeButton;

        [SerializeField]
        private MoneyCounter _moneyCounter;

        private string _boostersInfoLink;
        private ITapGameDataService _tapGameDataService;
        private TapGameConfig _tapGameConfig;
        private IPlayerDataService _playerDataService;
        private Func<int> GetTimeNow;
        private VirtualTimer _energyTimer;
        private ITimersController _timersController;

        public event Action OnFreeEnergyClick;
        public event Action OnUpgradeEnergyClick;
        public event Action OnUpgradeTapClick;

        [Inject]
        public void Construct(IGlobalConfigProvider globalConfigProvider, IPlayerDataService playerDataService,
            ITapGameConfigProvider tapGameConfigProvider, ITapGameDataService tapGameDataService,
            IRemoteDataService remoteDataService, ITimersController timersController)
        {
            _timersController = timersController;
            _playerDataService = playerDataService;
            _tapGameDataService = tapGameDataService;
            _tapGameConfig = tapGameConfigProvider.Config;
            GetTimeNow = remoteDataService.GetTimeNow;

            GlobalConfig globalConfig = globalConfigProvider.Config;
            _boostersInfoLink = globalConfig.LinksConfig.BoostersInfoLink;
        }

        public override void Show()
        {
            base.Show();
            _moneyCounter.UpdateInfo();
        }

        public override void Hide()
        {
            base.Hide();
            DestroySelfDelayed();
        }

        protected virtual void Start()
        {
            _closeButton.onClick.AddListener(OnCloseClick);
            _boostersInfoButton.onClick.AddListener(OnBoostersInfoButtonClick);

            _freeEnergyButton.OnClick += OnFreeEnergyButtonClick;
            _turboButton.OnClick += OnTurboButtonClick;
            _energyUpgradeButton.OnClick += OnEnergyUpgradeButtonClick;
            _tapUpgradeButton.OnClick += OnTapUpgradeButtonClick;

            UpdateFreeEnergyInfo();
            UpdateTurboInfo();
            UpdateEnergyUpgradeInfo();
            UpdateTapUpgradeInfo();
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();
            _closeButton.onClick.RemoveListener(OnCloseClick);
            _boostersInfoButton.onClick.RemoveListener(OnBoostersInfoButtonClick);

            _freeEnergyButton.OnClick -= OnFreeEnergyButtonClick;
            _turboButton.OnClick -= OnTurboButtonClick;
            _energyUpgradeButton.OnClick -= OnEnergyUpgradeButtonClick;
            _tapUpgradeButton.OnClick -= OnTapUpgradeButtonClick;

            if (_energyTimer != null && _energyTimer.IsRunning)
                _timersController.RemoveTimer(_energyTimer);
        }

        private void UpdateFreeEnergyInfo()
        {
            int fullRecoveryMax = _tapGameConfig.EnergyConfig.FullRecoveryCount +
                                  _tapGameConfig.EnergyConfig.FullRecoveryPerAccountLevel *
                                  _playerDataService.AccountLevel;
            int fullRecoveryCurrent = _tapGameDataService.FullEnergyRecoveryCount;

            string available = $"{fullRecoveryMax - fullRecoveryCurrent}/{fullRecoveryMax} {Phrases.AVAILABLE}";
            _freeEnergyButton.SetInfo1(available);

            UpdateFreeEnergyTimer();
        }

        private void UpdateFreeEnergyTimer()
        {
            string timer = string.Empty;
            int timeNow = GetTimeNow();
            int lastRecoveryTime = _tapGameDataService.LastFullEnergyRecoveryTime;
            int delay = UnixTime.HoursToSeconds(_tapGameConfig.EnergyConfig.HoursBetweenFullRecoveryUse);

            if (timeNow > lastRecoveryTime + delay)
                _freeEnergyButton.SetState(true);
            else
            {
                _freeEnergyButton.SetState(false, true);
                int remain = lastRecoveryTime + delay - timeNow;
                timer = $"{UnixTime.SecondsToMinutes(remain)} {Phrases.MINUTES} {Phrases.REMAIN}";

                _energyTimer?.Stop();
                _energyTimer = new VirtualTimer(15, () => { }, UpdateFreeEnergyTimer);
                _timersController.AddTimer(_energyTimer);
            }

            _freeEnergyButton.SetInfo2(timer);
        }

        private void UpdateTurboInfo()
        {
            if (_tapGameConfig.TurboConfig.TurboEnabled)
                _turboButton.SetActive(true);
            else
            {
                _turboButton.SetActive(false);
                return;
            }

            int turboMax = _tapGameConfig.TurboConfig.TurboCountPerDay +
                           _tapGameConfig.TurboConfig.TurboCountPerAccountLevel *
                           _playerDataService.AccountLevel;
            int turboCurrent = _tapGameDataService.TurboUsedCount;

            string available = $"{turboMax - turboCurrent}/{turboMax} {Phrases.AVAILABLE}";
            _turboButton.SetInfo1(available);

            bool enabled = turboMax - turboCurrent > 0;
            _turboButton.SetState(false, !enabled);
        }

        private void UpdateEnergyUpgradeInfo()
        {
            int upgradeLevel = _tapGameDataService.EnergyUpgradeLevel;
            long moneyToUpgrade = _tapGameConfig.EnergyConfig.GetUpgradeCost(upgradeLevel);

            string level = $" {Phrases.DOT} {upgradeLevel + 1} {Phrases.LEVEL}";

            if (moneyToUpgrade < 0)
            {
                _energyUpgradeButton.SetInfo1("wip");
                _energyUpgradeButton.SetState(false, true);
            }
            else
            {
                _energyUpgradeButton.SetInfo1(MoneyConverter.ConvertToShortValue(moneyToUpgrade));
                _energyUpgradeButton.SetState(true);
            }

            _energyUpgradeButton.SetInfo2(level);
        }

        private void UpdateTapUpgradeInfo()
        {
            int upgradeLevel = _tapGameDataService.TapUpgradeLevel;
            long moneyToUpgrade = _tapGameConfig.MoneyConfig.GetUpgradeCost(upgradeLevel);

            string level = $" {Phrases.DOT} {upgradeLevel + 1} {Phrases.LEVEL}";

            if (moneyToUpgrade < 0)
            {
                _tapUpgradeButton.SetInfo1("wip");
                _tapUpgradeButton.SetState(false, true);
            }
            else
            {
                _tapUpgradeButton.SetInfo1(MoneyConverter.ConvertToShortValue(moneyToUpgrade));
                _tapUpgradeButton.SetState(true);
            }

            _tapUpgradeButton.SetInfo2(level);
        }

        private void OnCloseClick()
        {
            Hide();
            DestroySelfDelayed();
        }

        private void OnBoostersInfoButtonClick()
        {
            Application.OpenURL(_boostersInfoLink);
        }

        private void OnFreeEnergyButtonClick() =>
            OnFreeEnergyClick?.Invoke();

        private void OnTurboButtonClick()
        {
        }

        private void OnEnergyUpgradeButtonClick() =>
            OnUpgradeEnergyClick?.Invoke();

        private void OnTapUpgradeButtonClick() =>
            OnUpgradeTapClick?.Invoke();
    }
}