using System;
using System.Collections.Generic;
using System.Linq;
using Main.Scripts.Configs.Accounts;
using Main.Scripts.Data.Services;
using Main.Scripts.GameLogic.External;
using Main.Scripts.Infrastructure.Providers.Configs;
using Main.Scripts.UI.Base;
using UnityEngine;
using UnityEngine.UI;
using Zenject;
using Random = UnityEngine.Random;

namespace Main.Scripts.UI.MainMenu.LeaderBoard
{
    public class LeaderBordPanel : UIPanel
    {
        [SerializeField]
        private AccountLevelInfo _accountLevelInfo;

        [SerializeField]
        private Button _closeButton;

        [SerializeField]
        private Button _previousButton;

        [SerializeField]
        private Button _nextButton;

        [SerializeField]
        private LeaderBoardVirtualList _topList;

        [SerializeField]
        private LeaderBoardItem _unTopItem;

        private IPlayerDataService _playerDataService;
        private IRemoteDataService _remoteDataService;
        private AccountConfig _accountConfig;
        private int _accountLevel;
        private int _maxAccountLevel;
        private LeaderBoardItemData _currentPlayerData;

        private Func<string, Sprite> GetStockIcon;
        private Func<Sprite> GetRandomStockIcon;

        [Inject]
        public void Construct(IAccountConfigProvider accountConfigProvider, IPlayerDataService playerDataService,
            IRemoteDataService remoteDataService, IGlobalConfigProvider globalConfigProvider)
        {
            _remoteDataService = remoteDataService;
            _playerDataService = playerDataService;
            _accountConfig = accountConfigProvider.Config;
            _maxAccountLevel = _accountConfig.AccountLevels.Count - 1;

            GetStockIcon = (string stockId) => globalConfigProvider.Config.GetStockConfig(stockId).StockIcon;
            GetRandomStockIcon = () =>
            {
                int index = Random.Range(0, globalConfigProvider.Config.Stocks.Count);
                return globalConfigProvider.Config.Stocks.ElementAt(index).StockIcon;
            };

            _currentPlayerData = new LeaderBoardItemData(remoteDataService.GetPlayerIcon(),
                remoteDataService.GetPlayerName(),
                GetStockIcon(_playerDataService.StockId),
                _playerDataService.Money, true, -1);
        }

        public override void Hide()
        {
            base.Hide();
            DestroySelfDelayed();
        }

        private void Start()
        {
            _closeButton.onClick.AddListener(OnCloseButtonClick);
            _previousButton.onClick.AddListener(OnPreviousButtonClick);
            _nextButton.onClick.AddListener(OnNextButtonClick);

            _accountLevel = _playerDataService.AccountLevel;
            UpdateAccountInfo(_accountLevel);
            UpdateTop(_accountLevel);
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();
            _closeButton.onClick.RemoveListener(OnCloseButtonClick);
            _previousButton.onClick.RemoveListener(OnPreviousButtonClick);
            _nextButton.onClick.RemoveListener(OnNextButtonClick);
        }

        private void UpdateAccountInfo(int accountLevel)
        {
            AccountLevelConfig accountLevelConfig = _accountConfig.GetAccountLevelConfig(accountLevel);
            AccountLevelConfig nextAccountLevelConfig = _accountConfig.GetAccountLevelConfig(accountLevel + 1);
            long currentMoney = _playerDataService.Money;
            bool isLevelEqualsCurrent = accountLevel == _playerDataService.AccountLevel;

            _accountLevelInfo.UpdateInfo(accountLevelConfig, nextAccountLevelConfig, isLevelEqualsCurrent,
                currentMoney);
        }

        private void UpdateTop(int accountLevel)
        {
            bool isLevelEqualsCurrent = accountLevel == _playerDataService.AccountLevel;
            List<LeaderBoardItemData> leaderBoardItemsData = BuildPlayersTop(accountLevel);

            _currentPlayerData.SetPlace(-1);

            if (isLevelEqualsCurrent)
                leaderBoardItemsData.Add(_currentPlayerData);
            else
                SetupUnTopContainer(false);

            leaderBoardItemsData = leaderBoardItemsData.OrderByDescending(item => item.Money)
                .Select((item, index) =>
                {
                    item.SetPlace(index + 1);
                    return item;
                }).Take(100).ToList();

            _topList.InitItems(leaderBoardItemsData);

            if (_currentPlayerData.Place < 0 && isLevelEqualsCurrent)
                _currentPlayerData.SetPlace(Random.Range(101, 999999));

            if (_currentPlayerData.Place <= 100)
            {
                SetupUnTopContainer(false);
                _topList.ScrollTo(_currentPlayerData.Place - 1);
            }
            else
            {
                SetupUnTopContainer(true);
                _unTopItem.UpdateInfo(_currentPlayerData);
            }
        }

        private List<LeaderBoardItemData> BuildPlayersTop(int accountLevel)
        {
            List<LeaderBoardItemData> leaderBoardItemsData = _remoteDataService.GetLeaderBoard(accountLevel);

            if (leaderBoardItemsData == null || leaderBoardItemsData.Count == 0)
                return BuildFakePlayersTop(accountLevel);

            return leaderBoardItemsData;
        }

        private List<LeaderBoardItemData> BuildFakePlayersTop(int accountLevel)
        {
            List<LeaderBoardItemData> leaderBoardItemsData = new List<LeaderBoardItemData>();
            IReadOnlyCollection<FakeAccountData> fakeDataConfig = _accountConfig.FakeAccountsData;
            AccountLevelConfig accountLevelConfig = _accountConfig.GetAccountLevelConfig(accountLevel);
            AccountLevelConfig nextAccountLevelConfig = _accountConfig.GetAccountLevelConfig(accountLevel + 1);

            long maxMoney = accountLevelConfig.NeedMoney;
            if (nextAccountLevelConfig != null)
                maxMoney = accountLevelConfig.NeedMoney - 1;

            for (int i = 0; i < 100; i++)
            {
                FakeAccountData accountData = fakeDataConfig.ElementAt(Random.Range(0, fakeDataConfig.Count));
                LeaderBoardItemData itemData = new LeaderBoardItemData(accountData.AccountIcon,
                    $"{accountData.AccountName}_{Random.Range(0, i)}", GetRandomStockIcon(),
                    (long) Random.Range(0, maxMoney), false, i);

                leaderBoardItemsData.Add(itemData);
            }

            return leaderBoardItemsData;
        }

        private void SetupSwitchButtons()
        {
            _previousButton.interactable = _accountLevel > 0;
            _nextButton.interactable = _accountLevel < _maxAccountLevel;
        }

        void SetupUnTopContainer(bool isActive)
        {
            _unTopItem.gameObject.SetActive(isActive);
            _topList.SetBottomOffset(isActive);
        }

        private void OnCloseButtonClick() =>
            Hide();

        private void OnNextButtonClick()
        {
            _accountLevel++;
            if (_accountLevel > _maxAccountLevel)
            {
                _accountLevel = _maxAccountLevel;
                return;
            }

            SetupSwitchButtons();
            UpdateAccountInfo(_accountLevel);
            UpdateTop(_accountLevel);
        }

        private void OnPreviousButtonClick()
        {
            _accountLevel--;
            if (_accountLevel < 0)
            {
                _accountLevel = 0;
                return;
            }

            SetupSwitchButtons();
            UpdateAccountInfo(_accountLevel);
            UpdateTop(_accountLevel);
        }
    }
}