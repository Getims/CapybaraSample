using System;
using Main.Scripts.Configs.Accounts;
using Main.Scripts.Data.Services;
using Main.Scripts.Infrastructure.Providers.Configs;
using Main.Scripts.Infrastructure.Providers.Events;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Main.Scripts.UI.MainMenu.Info
{
    public class LevelInfoPanel : MonoBehaviour
    {
        [SerializeField]
        private TextMeshProUGUI _levelName;

        [SerializeField]
        private TextMeshProUGUI _levelCounter;

        [SerializeField]
        private Image _levelProgress;

        [SerializeField]
        private Button _levelButton;

        [SerializeField, Min(0)]
        private float _checkProgressTime = 2f;

        private GlobalEventProvider _globalEventProvider;
        private AccountConfig _accountConfig;
        private IPlayerDataService _playerDataService;

        private long _lastMoneyCount;
        private float _lastCheckTime;
        private int _currentAccountLevel;

        public event Action OnLevelClick;

        [Inject]
        public void Construct(IAccountConfigProvider accountConfigProvider, IPlayerDataService playerDataService,
            GlobalEventProvider globalEventProvider)
        {
            _playerDataService = playerDataService;
            _globalEventProvider = globalEventProvider;
            _globalEventProvider.AccountLevelSwitchEvent.AddListener(UpdateInfo);
            _accountConfig = accountConfigProvider.Config;
        }

        public void UpdateInfo()
        {
            if (_playerDataService == null)
                return;

            UpdateInfo(_playerDataService.AccountLevel);
        }

        private void Start()
        {
            _levelButton.onClick.AddListener(OnLevelButtonClick);
            UpdateInfo();
        }

        protected void OnDestroy()
        {
            _levelButton.onClick.RemoveListener(OnLevelButtonClick);
            _globalEventProvider?.AccountLevelSwitchEvent.RemoveListener(UpdateInfo);
        }

        private void FixedUpdate()
        {
            if (_lastCheckTime + _checkProgressTime >= Time.time)
                return;

            _lastCheckTime = Time.time;
            long currentMoney = _playerDataService.Money;
            if (currentMoney.Equals(_lastMoneyCount))
                return;

            _lastMoneyCount = currentMoney;
            AccountLevelConfig nextAccountLevel = _accountConfig.GetAccountLevelConfig(_currentAccountLevel + 1);
            if (nextAccountLevel == null)
                return;

            long needMoney = nextAccountLevel.NeedMoney;
            float progress = 1f * currentMoney / needMoney;
            SetLevelProgress(progress);
        }

        private void UpdateInfo(int accountLevel)
        {
            _currentAccountLevel = accountLevel;
            AccountLevelConfig currentAccountLevel = _accountConfig.GetAccountLevelConfig(_currentAccountLevel);
            AccountLevelConfig nextAccountLevel = _accountConfig.GetAccountLevelConfig(_currentAccountLevel + 1);

            SetName(currentAccountLevel.LevelName);
            if (nextAccountLevel == null)
            {
                SetCounter(_currentAccountLevel + 1, _currentAccountLevel + 1);
                SetLevelProgress(0);
            }
            else
            {
                SetCounter(_currentAccountLevel + 1, _accountConfig.AccountLevels.Count);

                long currentMoney = _playerDataService.Money;
                _lastMoneyCount = currentMoney;
                long needMoney = nextAccountLevel.NeedMoney;
                float progress = 1f * currentMoney / needMoney;
                SetLevelProgress(progress);
            }

            _lastCheckTime = Time.time;
        }

        private void SetName(string name) =>
            _levelName.text = name;

        private void SetCounter(int currentId, int levelsCount)
        {
            if (currentId > levelsCount)
                Debug.LogWarning("Wrong level id!");

            _levelCounter.text = $"{currentId}/{levelsCount}";
        }

        private void SetLevelProgress(float progress) =>
            _levelProgress.fillAmount = progress;

        private void OnLevelButtonClick() =>
            OnLevelClick?.Invoke();
    }
}