using System.Collections.Generic;
using System.Linq;
using Coffee.UIExtensions;
using Main.Scripts.Configs.Accounts;
using Main.Scripts.Configs.Friends;
using Main.Scripts.Core.Utilities;
using Main.Scripts.Data.DataObjects;
using Main.Scripts.Data.Services;
using Main.Scripts.GameLogic;
using Main.Scripts.GameLogic.External;
using Main.Scripts.GameLogic.Sound;
using Main.Scripts.Infrastructure.Providers.Configs;
using Main.Scripts.UI.Base;
using Main.Scripts.UI.Common;
using Main.Scripts.UI.FriendsMenu.BonusInfo;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Main.Scripts.UI.FriendsMenu
{
    public class FriendsMenuPanel : MenuPanel
    {
        [SerializeField]
        private TextMeshProUGUI _friendReward;

        [SerializeField]
        private TextMeshProUGUI _friendsCounter;

        [SerializeField]
        private BonusInfoContainer _bonusInfoContainer;

        [SerializeField]
        private FriendsVirtualList _friendsVirtualList;

        [SerializeField]
        private Button _moreInfoButton;

        [SerializeField]
        private Button _refreshListButton;

        [SerializeField]
        private Button _inviteButton;

        [SerializeField]
        private Button _linkButton;

        [SerializeField]
        private UIParticle _rewardParticles;

        private FriendsConfig _friendsConfig;
        private AccountConfig _accountConfig;
        private IFriendsDataService _friendsDataService;
        private IRemoteDataService _remoteDataService;
        private IPlayerDataService _playerDataService;
        private ISoundService _soundService;

        private bool _isFirstSetup = true;
        private List<AccountData> _friendsAccounts;

        [Inject]
        public void Construct(IFriendsConfigProvider friendsConfigProvider, IFriendsDataService friendsDataService,
            IAccountConfigProvider accountConfigProvider, IRemoteDataService remoteDataService,
            IPlayerDataService playerDataService, ISoundService soundService)
        {
            _soundService = soundService;
            _playerDataService = playerDataService;
            _friendsDataService = friendsDataService;
            _friendsConfig = friendsConfigProvider.Config;
            _accountConfig = accountConfigProvider.Config;
            _remoteDataService = remoteDataService;
        }

        public override void Initialize(IUIMenuFactory uiMenuFactory, UIContainerProvider containerProvider)
        {
            base.Initialize(uiMenuFactory, containerProvider);

            if (_isFirstSetup)
            {
                _isFirstSetup = false;
                _friendReward.text = $"+{MoneyConverter.ConvertToSpaceValue(_friendsConfig.InviteReward)}";
                BuildFriendsList();
                CheckFriendsRewards();
            }

            _friendsDataService.SetNotificationState(false);
        }

        public override void Hide()
        {
            base.Hide();
            CloseAllPopups();
        }

        private void Start()
        {
            _inviteButton.onClick.AddListener(OnInviteClick);
            _linkButton.onClick.AddListener(OnLinkClick);
            _moreInfoButton.onClick.AddListener(OnMoreInfoClick);
            _refreshListButton.onClick.AddListener(OnRefreshFriendsList);
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();
            _inviteButton.onClick.RemoveListener(OnLinkClick);
            _linkButton.onClick.RemoveListener(OnLinkClick);
            _moreInfoButton.onClick.RemoveListener(OnMoreInfoClick);
            _refreshListButton.onClick.RemoveListener(BuildFriendsList);
        }

        private void BuildFriendsList()
        {
            _friendsAccounts = _remoteDataService.GetFriendsData();
            if (_friendsAccounts == null)
                _friendsAccounts = _friendsConfig.FakeFriends.ToList();

            List<FriendListInfo> friendsData = new List<FriendListInfo>();
            foreach (AccountData accountData in _friendsAccounts)
            {
                FriendListInfo friendInfo = new FriendListInfo(accountData.AccountIcon,
                    accountData.AccountName, GetAccountLevel(accountData.AccountLevel), accountData.Money,
                    GetAccountReward(accountData.AccountLevel));
                friendsData.Add(friendInfo);
            }

            friendsData.Sort((friend1, friend2) => friend2.Reward.CompareTo(friend1.Reward));
            _friendsCounter.text = $"({friendsData.Count})";
            _friendsVirtualList.InitItems(friendsData);
        }

        private void CheckFriendsRewards()
        {
            if (_friendsAccounts == null)
                return;

            List<FriendAccountData> friendAccountDatas = _friendsDataService.FriendList.ToList();
            long reward = 0;

            foreach (AccountData account in _friendsAccounts)
            {
                FriendAccountData savedAccount =
                    friendAccountDatas.FirstOrDefault(fr => fr.AccountName == account.AccountName);

                if (savedAccount == null)
                {
                    _friendsDataService.AddFriendAccount(
                        new FriendAccountData(account.AccountName, account.AccountLevel), false);

                    reward += GetAccountReward(account.AccountLevel);
                }
                else
                {
                    if (account.AccountLevel == savedAccount.AccountLevel)
                        continue;

                    reward += GetAccountReward(account.AccountLevel, savedAccount.AccountLevel, false);

                    _friendsDataService.UpdateFriendAccount(
                        new FriendAccountData(account.AccountName, account.AccountLevel), false);
                }
            }

            _friendsDataService.SaveData();

            if (reward > 0)
                AddMoneyReward(reward);
        }

        private void AddMoneyReward(long reward)
        {
            _playerDataService.AddMoney(reward);
            _rewardParticles.Play();
            _soundService.PlayTaskCompleteSound();
        }

        private string GetAccountLevel(int accountLevel)
        {
            AccountLevelConfig accountLevelConfig = _accountConfig.GetAccountLevelConfig(accountLevel);
            return accountLevelConfig.LevelName;
        }

        private long GetAccountReward(int currentLevel, int startLevel = 0, bool includeInvite = true)
        {
            long reward = 0;
            if (includeInvite)
                reward += _friendsConfig.InviteReward;

            for (int i = startLevel; i <= currentLevel; i++)
                reward += _friendsConfig.GetFriendLevelReward(i);

            return reward;
        }

        private void OnMoreInfoClick()
        {
            _moreInfoButton.gameObject.SetActive(false);
            _bonusInfoContainer.Initialize(_accountConfig, _friendsConfig.FriendLevelReward, OnMoreInfoCloseClick);
            _bonusInfoContainer.SetActive(true);
        }

        private void OnMoreInfoCloseClick()
        {
            _moreInfoButton.gameObject.SetActive(true);
            _bonusInfoContainer.SetActive(false);
        }

        private void OnInviteClick()
        {
            Utils.ReworkPoint("Add invite logic");
        }

        private void OnLinkClick()
        {
            Utils.ReworkPoint("Add link logic");
        }

        private void OnRefreshFriendsList()
        {
            BuildFriendsList();
            CheckFriendsRewards();
        }
    }
}