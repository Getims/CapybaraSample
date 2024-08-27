using System.Collections.Generic;
using System.Linq;
using Coffee.UIExtensions;
using Main.Scripts.Configs.Tasks;
using Main.Scripts.Core.Constants;
using Main.Scripts.Core.Enums;
using Main.Scripts.Data.Services;
using Main.Scripts.GameLogic.External;
using Main.Scripts.GameLogic.Sound;
using Main.Scripts.Infrastructure.Providers.Configs;
using Main.Scripts.UI.Base;
using Main.Scripts.UI.Common;
using Main.Scripts.UI.TasksMenu.Tasks;
using UnityEngine;
using Zenject;

namespace Main.Scripts.UI.TasksMenu
{
    public class TasksMenuPanel : MenuPanel
    {
        [SerializeField]
        private TaskButton _taskButtonPrefab;

        [SerializeField]
        private TaskButton _dailyTaskButton;

        [SerializeField]
        private Transform _baseTaskPosition;

        [SerializeField]
        private Transform _watchTaskPosition;

        [SerializeField]
        private UIParticle _rewardParticles;

        private IPlayerDataService _playerDataService;
        private IRemoteDataService _remoteDataService;
        private TasksGroupConfig _tasksConfig;
        private ITasksDataService _tasksDataService;
        private ISoundService _soundService;

        private bool _isFirstSetup = true;
        private List<TaskButton> _taskButtons = new List<TaskButton>();

        [Inject]
        public void Construct(ITasksConfigProvider tasksConfigProvider, ITasksDataService tasksDataService,
            IPlayerDataService playerDataService, IRemoteDataService remoteDataService, ISoundService soundService)
        {
            _soundService = soundService;
            _tasksDataService = tasksDataService;
            _tasksConfig = tasksConfigProvider.Config;
            _remoteDataService = remoteDataService;
            _playerDataService = playerDataService;
        }

        public override void Initialize(IUIMenuFactory uiMenuFactory, UIContainerProvider containerProvider)
        {
            base.Initialize(uiMenuFactory, containerProvider);

            if (_isFirstSetup)
            {
                CreateTasksButtons();
                SetupDailyTaskButton();
                _isFirstSetup = false;
            }
            else
                UpdateTasksButtons();
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
            _dailyTaskButton.OnClick += OpenDailyTaskPopup;
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();
            _dailyTaskButton.OnClick -= OpenDailyTaskPopup;
        }

        private void SetupDailyTaskButton()
        {
            _dailyTaskButton.SetTitle(Phrases.DAILY_TASK);

            long fullReward = 0;
            foreach (long reward in _tasksConfig.DailyRewards)
                fullReward += reward;

            _dailyTaskButton.SetReward(fullReward);
            _dailyTaskButton.SetState(_tasksDataService.IsDailyTaskClaimed);
        }

        private void CreateTasksButtons()
        {
            TaskConfig[] configs = _tasksConfig.Tasks.ToArray();
            Transform buttonsContainer = _baseTaskPosition.parent;
            int watchTaskIndex = _watchTaskPosition.GetSiblingIndex() + 1;
            int baseTaskIndex = _baseTaskPosition.GetSiblingIndex() + 1;

            for (int i = configs.Length - 1; i >= 0; i--)
            {
                TaskConfig taskConfig = configs.ElementAt(i);

                if (taskConfig.TaskType == TaskType.NULL)
                    continue;

                if (taskConfig.TaskType == TaskType.Watch)
                    CreateTaskButton(buttonsContainer, taskConfig, watchTaskIndex);
                else
                    CreateTaskButton(buttonsContainer, taskConfig, baseTaskIndex);
            }
        }

        private void CreateTaskButton(Transform buttonsContainer, TaskConfig taskConfig, int baseTaskIndex)
        {
            TaskButton taskButton = Instantiate(_taskButtonPrefab, buttonsContainer);
            taskButton.Setup(taskConfig, OnTaskClick);
            taskButton.transform.SetSiblingIndex(baseTaskIndex);
            taskButton.SetState(_tasksDataService.IsTaskComplete(taskConfig.TaskId));
            _taskButtons.Add(taskButton);
        }

        private void UpdateTasksButtons()
        {
            _dailyTaskButton.SetState(_tasksDataService.IsDailyTaskClaimed);

            foreach (TaskButton taskButton in _taskButtons)
            {
                bool isComplete = _tasksDataService.IsTaskComplete(taskButton.GetTaskId());
                taskButton.SetState(isComplete);
            }
        }

        private void OpenDailyTaskPopup()
        {
            var popup = OpenPopup<DailyTaskPopup>(null, TryToClaimDailyReward);
            popup.Initialize();
        }

        private void OpenUrlLinkTaskPopup(TaskConfig taskConfig)
        {
            var popup = OpenPopup<UrlLinkTaskPopup>(null, CloseAllPopups);
            popup.Initialize(taskConfig, OnTaskComplete);
        }

        private void OpenFriendsAndStockTaskPopup(TaskConfig taskConfig)
        {
            var popup = OpenPopup<FriendsAndStockTaskPopup>(null, CloseAllPopups);
            popup.Initialize(taskConfig, OnTaskComplete);
        }

        private void OpenWalletTaskPopup(TaskConfig taskConfig)
        {
            var popup = OpenPopup<WalletTaskPopup>(null, CloseAllPopups);
            popup.Initialize(taskConfig, OnTaskComplete);
        }

        private void ShowMoneyReward() =>
            _rewardParticles.Play();

        private void OnTaskClick(TaskConfig taskConfig)
        {
            switch (taskConfig.TaskType)
            {
                case TaskType.NULL:
                    break;
                case TaskType.Stock:
                    OpenFriendsAndStockTaskPopup(taskConfig);
                    break;
                case TaskType.Friends:
                    OpenFriendsAndStockTaskPopup(taskConfig);
                    break;
                case TaskType.Subscribe:
                    OpenUrlLinkTaskPopup(taskConfig);
                    break;
                case TaskType.Watch:
                    OpenUrlLinkTaskPopup(taskConfig);
                    break;
                case TaskType.Wallet:
                    OpenWalletTaskPopup(taskConfig);
                    break;
            }
        }

        private void OnTaskComplete(TaskConfig taskConfig)
        {
            if (taskConfig == null)
                return;

            _tasksDataService.AddCompletedTaskId(taskConfig.TaskId);
            _playerDataService.AddMoney(taskConfig.Reward);
            ShowMoneyReward();
            UpdateTasksButtons();
            _soundService.PlayTaskCompleteSound();
        }

        private void TryToClaimDailyReward()
        {
            if (_tasksDataService.IsDailyTaskClaimed)
                return;

            int currentDay = _tasksDataService.CurrentDailyTask;
            long reward = _tasksConfig.GetDayReward(currentDay);

            _playerDataService.AddMoney(reward);
            _tasksDataService.SetDailyTaskClaimed(true);
            _tasksDataService.SetDailyTaskClaimTime(_remoteDataService.GetTimeNow());

            ShowMoneyReward();
            UpdateTasksButtons();
            _soundService.PlayTaskCompleteSound();
        }
    }
}