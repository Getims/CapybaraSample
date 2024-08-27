using System.Collections.Generic;
using System.Linq;
using Main.Scripts.Configs.Accounts;
using Main.Scripts.Configs.Global;
using Main.Scripts.Configs.Tasks;
using Main.Scripts.Core.Constants;
using Main.Scripts.Core.Utilities;
using Main.Scripts.Data.Services;
using Main.Scripts.GameLogic;
using Main.Scripts.GameLogic.External;
using Main.Scripts.GameLogic.Timers;
using Main.Scripts.Infrastructure.Providers.Configs;

namespace Main.Scripts.Infrastructure.Bootstrap
{
    public class GameSceneBootstrapper
    {
        private readonly IPlayerDataService _playerDataService;
        private readonly ITapGameDataService _tapGameDataService;
        private readonly IMiningController _miningController;
        private readonly IRemoteDataService _remoteDataService;
        private readonly IEnergyController _energyController;
        private readonly ITimersController _timersController;
        private readonly ITasksDataService _tasksDataService;
        private readonly IFriendsDataService _friendsDataService;
        private readonly ICardsDataService _cardsDataService;

        private readonly GlobalConfig _globalConfig;
        private readonly AccountConfig _accountConfig;
        private readonly TasksGroupConfig _tasksConfig;

        public GameSceneBootstrapper(IPlayerDataService playerDataService, IGlobalConfigProvider globalConfigProvider,
            ITapGameDataService tapGameDataService, IMiningController miningController,
            IRemoteDataService remoteDataService, IEnergyController energyController,
            IAccountConfigProvider accountConfigProvider, ITimersController timersController,
            ITasksDataService tasksDataService, ITasksConfigProvider tasksConfigProvider,
            IFriendsDataService friendsDataService, ICardsDataService cardsDataService)
        {
            _cardsDataService = cardsDataService;
            _friendsDataService = friendsDataService;
            _timersController = timersController;
            _energyController = energyController;
            _remoteDataService = remoteDataService;
            _miningController = miningController;
            _tapGameDataService = tapGameDataService;
            _playerDataService = playerDataService;
            _tasksDataService = tasksDataService;

            _globalConfig = globalConfigProvider.Config;
            _accountConfig = accountConfigProvider.Config;
            _tasksConfig = tasksConfigProvider.Config;
        }

        public void Initialize()
        {
            //SetupStock();

            CheckDate();
            CheckTime();
            CheckFriends();
        }

        private void SetupStock()
        {
            string stockId = _playerDataService.StockId;

            if (stockId == string.Empty)
            {
                stockId = _globalConfig.Stocks.ElementAt(0).StockId;
                _playerDataService.SetStock(stockId);
            }
        }

        private void CheckDate()
        {
            int lastEnter = _playerDataService.LastGameTime;
            int now = _remoteDataService.GetTimeNow();

            if (lastEnter > now)
                return;

            bool isNewDay = UnixTime.IsDayPassed(lastEnter, now);
            if (!isNewDay)
                return;

            ResetTapGame();
            ResetDailyTask(now);
            ResetCombo();

            _friendsDataService.SetLevelsCheckedState(false);
        }

        private void CheckTime()
        {
            int timeNow = _remoteDataService.GetTimeNow();
            if (timeNow <= _playerDataService.LastGameTime)
                return;

            SetupMiningController();
            CheckAccountLevel();
            CreateAccountTimer();
            _energyController.UpdateEnergyFromLastPlay();
            CreateEnergyTimer();
            CreateTimeUpdateTimer(timeNow);
        }

        private void CheckAccountLevel()
        {
            long currentMoney = _playerDataService.Money;
            int accountLevel = _playerDataService.AccountLevel;
            long needMoney = 0;

            int i = 0;
            while (currentMoney >= needMoney)
            {
                i += 1;
                AccountLevelConfig LevelConfig = _accountConfig.GetAccountLevelConfig(accountLevel + i);
                if (LevelConfig == null)
                    break;

                needMoney = LevelConfig.NeedMoney;
            }

            i -= 1;
            if (i > 0)
                _playerDataService.SetAccountLevel(accountLevel + i);
        }

        private void CheckFriends()
        {
            if (_friendsDataService.HasNotification)
                return;

            if (_remoteDataService.GetFriendsCount() != _friendsDataService.FriendList.Count)
            {
                _friendsDataService.SetNotificationState(true, false);
                _friendsDataService.SetLevelsCheckedState(false, true);
                return;
            }

            if (_friendsDataService.LevelsCheckedToday)
                return;

            List<AccountData> friendsAccounts = _remoteDataService.GetFriendsData();
            _friendsDataService.SetLevelsCheckedState(true);
            if (friendsAccounts == null)
                return;

            long levelsSum = 0;
            foreach (AccountData accountData in friendsAccounts)
                levelsSum += accountData.AccountLevel;

            if (levelsSum != _friendsDataService.GetFriendsLevelsSum())
                _friendsDataService.SetNotificationState(true);
        }

        private void SetupMiningController()
        {
            _miningController.UpdateMoneyPerHour();
            _miningController.CalculateMoneyFromLastPlay();
            _miningController.UpdateCardsCombo();
            CreateMiningTimer();
        }

        private void ResetTapGame()
        {
            _tapGameDataService.SetFullEnergyRecoveryCount(0, false);
            _tapGameDataService.SetFullEnergyRecoveryTime(0, false);
            _tapGameDataService.SetTurboUsedCount(0, false);
            _tapGameDataService.SetTurboUsedTime(0);
        }

        private void ResetDailyTask(int today)
        {
            int taskClaimTime = _tasksDataService.LastDailyTaskClaimTime;
            bool taskClaimedYesterday = UnixTime.IsYesterday(taskClaimTime, today);
            int currentDay = _tasksDataService.CurrentDailyTask;

            if (taskClaimedYesterday)
            {
                currentDay += 1;
                if (currentDay >= _tasksConfig.DailyRewards.Count)
                    currentDay = 0;
            }
            else
                currentDay = 0;

            _tasksDataService.SetDailyTaskClaimed(false, false);
            _tasksDataService.SetDailyTaskClaimTime(0, false);
            _tasksDataService.SetDailyTask(currentDay);
        }

        private void ResetCombo() =>
            _cardsDataService.ClearComboCards();

        private void CreateAccountTimer()
        {
            VirtualTimer accountTimer = new VirtualTimerEndless(CheckAccountLevel, TimersTick.ACCOUNT_CHECK_INTERVAL);
            _timersController.AddTimer(accountTimer);
        }

        private void CreateEnergyTimer()
        {
            VirtualTimer energyTimer =
                new VirtualTimerEndless(_energyController.RecoveryEnergy, TimersTick.RECOVERY_ENERGY_INTERVAL);
            _timersController.AddTimer(energyTimer);
        }

        private void CreateMiningTimer()
        {
            VirtualTimer miningTimer =
                new VirtualTimerEndless(_miningController.AddMoneyFromMining, TimersTick.MINING_INTERVAL);
            _timersController.AddTimer(miningTimer);
        }

        private void CreateTimeUpdateTimer(int timeNow)
        {
            _playerDataService.SaveTime(timeNow);

            VirtualTimer timeUpdateTimer = new VirtualTimerEndless(() =>
            {
                int now = _remoteDataService.GetTimeNow();
                _playerDataService.SaveTime(now);
            }, TimersTick.GAME_TIME_SAVE_INTERVAL);
            _timersController.AddTimer(timeUpdateTimer);
        }
    }
}