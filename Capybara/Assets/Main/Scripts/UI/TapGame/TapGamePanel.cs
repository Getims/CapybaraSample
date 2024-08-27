using System.Collections.Generic;
using Lean.Pool;
using Main.Scripts.Configs.Accounts;
using Main.Scripts.Configs.TapGame;
using Main.Scripts.Core.Enums;
using Main.Scripts.Data.Services;
using Main.Scripts.GameLogic.External;
using Main.Scripts.GameLogic.Sound;
using Main.Scripts.GameLogic.Timers;
using Main.Scripts.Infrastructure.Providers.Configs;
using Main.Scripts.Infrastructure.Providers.Events;
using Main.Scripts.UI.TapGame.Core;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Main.Scripts.UI.TapGame
{
    public class TapGamePanel : MonoBehaviour
    {
        [SerializeField]
        private Image _tapIcon;

        [SerializeField]
        private List<TapTracker> _tapTrackers = new List<TapTracker>();

        [SerializeField]
        private TapAnimator _tapAnimator = new TapAnimator();

        [SerializeField]
        private TurboLogic _turboLogic = new TurboLogic();

        [Title("Tap hint")]
        [SerializeField]
        private LeanGameObjectPool _moneyPool;

        [SerializeField]
        private LeanGameObjectPool _boostPool;

        [SerializeField]
        private Transform _moneyContainer;

        [SerializeField, Min(0)]
        private float _despawnTime = 1f;

        private IPlayerDataService _playerDataService;
        private TapGameConfig _tapGameConfig;
        private AccountConfig _accountConfig;
        private ITapGameDataService _tapGameDataService;
        private GlobalEventProvider _globalEventProvider;
        private UpgradeEventProvider _upgradeEventProvider;
        private ISoundService _soundService;

        private bool _boostMode = false;
        private int _moneyPerTapBase = 0;
        private int _moneyPerTap = 0;
        private int _energyPerTap = 0;
        private LeanGameObjectPool _currentMoneyPool;

        [Inject]
        public void Construct(IPlayerDataService playerDataService, ITapGameDataService tapGameDataService,
            ITapGameConfigProvider tapGameConfigProvider, IAccountConfigProvider accountConfigProvider,
            GlobalEventProvider globalEventProvider, UpgradeEventProvider upgradeEventProvider,
            ISoundService soundService, IRemoteDataService remoteDataService, ITimersController timersController)
        {
            _globalEventProvider = globalEventProvider;
            _upgradeEventProvider = upgradeEventProvider;
            _tapGameDataService = tapGameDataService;
            _playerDataService = playerDataService;
            _tapGameConfig = tapGameConfigProvider.Config;
            _accountConfig = accountConfigProvider.Config;
            _soundService = soundService;

            _turboLogic.Initialize(_tapGameConfig.TurboConfig, remoteDataService, _tapGameDataService, SetTurboMode,
                timersController);

            _globalEventProvider.AccountLevelSwitchEvent.AddListener(UpdateInfo);
            _upgradeEventProvider.TapUpgradeEvent.AddListener(UpdateTapLevel);
        }

        public void Initialize(UIContainerProvider containerProvider) =>
            _moneyContainer = containerProvider.CoinsContainer;

        private void Start()
        {
            foreach (TapTracker tapTracker in _tapTrackers)
            {
                tapTracker.OnTapStart += OnTapStart;
                tapTracker.OnTapEnd += OnTapEnd;
            }

            _tapAnimator.Start();
            UpdateInfo(_playerDataService.AccountLevel);
        }

        private void OnDestroy()
        {
            foreach (TapTracker tapTracker in _tapTrackers)
            {
                tapTracker.OnTapStart -= OnTapStart;
                tapTracker.OnTapEnd -= OnTapEnd;
            }

            _tapAnimator.OnDestroy();
            _globalEventProvider.AccountLevelSwitchEvent.RemoveListener(UpdateInfo);
            _upgradeEventProvider.TapUpgradeEvent.RemoveListener(UpdateTapLevel);
        }

        private bool CanTap() =>
            _tapGameDataService.Energy >= _energyPerTap;

        private void PlaySound()
        {
            if (_boostMode)
                _soundService.PlayTurboTapSound();
            else
                _soundService.PlayTapSound();
        }

        private void ShowMoneyPlate(Vector2 tapPosition, float value)
        {
            GameObject instance = _currentMoneyPool.Spawn(tapPosition, Quaternion.identity, _moneyContainer);
            if (instance == null)
                return;

            MoneyPlate moneyPlate = instance.GetComponent<MoneyPlate>();
            moneyPlate.Initialize(value);
            _currentMoneyPool.Despawn(instance, _despawnTime);
        }

        private void UpdateInfo(int accountLevel)
        {
            Sprite icon = _accountConfig.GetAccountLevelConfig(accountLevel).Icon;
            _tapIcon.sprite = icon;

            _moneyPerTapBase = _tapGameConfig.MoneyConfig.MoneyPerTap +
                               accountLevel * _tapGameConfig.MoneyConfig.MoneyTapPerAccountLevel +
                               _tapGameDataService.TapUpgradeLevel * _tapGameConfig.MoneyConfig.MoneyTapPerUpgradeLevel;

            CalculateTapInfo();
        }

        private void UpdateTapLevel()
        {
            _moneyPerTapBase = _tapGameConfig.MoneyConfig.MoneyPerTap +
                               _playerDataService.AccountLevel * _tapGameConfig.MoneyConfig.MoneyTapPerAccountLevel +
                               _tapGameDataService.TapUpgradeLevel * _tapGameConfig.MoneyConfig.MoneyTapPerUpgradeLevel;
            CalculateTapInfo();
        }

        private void CalculateTapInfo()
        {
            if (_boostMode)
            {
                int maxBoost = _playerDataService.AccountLevel + 2;
                float moneyBoost = _tapGameConfig.TurboConfig.MoneyBoostMultiplier;
                if (maxBoost > moneyBoost)
                    moneyBoost = Random.Range(moneyBoost, maxBoost);

                _moneyPerTap = (int) (_moneyPerTapBase * moneyBoost);
                _energyPerTap = (int) (_moneyPerTapBase * _tapGameConfig.TurboConfig.EnergyBoostMultiplier);
                _currentMoneyPool = _boostPool;
            }
            else
            {
                _moneyPerTap = _moneyPerTapBase;
                _energyPerTap = _moneyPerTapBase;
                _currentMoneyPool = _moneyPool;
            }
        }

        [Button]
        private void SetTurboMode(bool isActive)
        {
            _boostMode = isActive;
            CalculateTapInfo();
        }

        private void OnTapStart(TapZone tapZone)
        {
            if (CanTap())
                _tapAnimator.AnimateTapDown(tapZone);
        }

        private void OnTapEnd(TapZone tapZone, Vector2 tapPosition)
        {
            if (!CanTap())
                return;

            _tapAnimator.AnimateTapUp(tapZone);
            _turboLogic.TryToSpawnTurbo();
            _tapGameDataService.SpendEnergy(_energyPerTap);
            _playerDataService.AddMoney(_moneyPerTap);

            PlaySound();
            ShowMoneyPlate(tapPosition, _moneyPerTap);
        }
    }
}