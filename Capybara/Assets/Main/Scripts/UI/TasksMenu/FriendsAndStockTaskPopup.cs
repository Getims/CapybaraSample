using System;
using Main.Scripts.Configs.Tasks;
using Main.Scripts.Core.Constants;
using Main.Scripts.Core.Enums;
using Main.Scripts.Core.Utilities;
using Main.Scripts.Data.Services;
using Main.Scripts.GameLogic;
using Main.Scripts.UI.Common;
using Sirenix.Utilities;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Main.Scripts.UI.TasksMenu
{
    public class FriendsAndStockTaskPopup : PopupPanel
    {
        [SerializeField]
        private Image _icon;

        [SerializeField]
        private TextMeshProUGUI _title;

        [SerializeField]
        private TextMeshProUGUI _reward;

        [SerializeField]
        private TextMeshProUGUI _claimButtonTitle;

        private ITasksDataService _tasksDataService;
        private IFriendsDataService _friendsDataService;
        private IPlayerDataService _playerDataService;

        private Action<TaskConfig> _onClaimTask;
        private TaskConfig _taskConfig;
        private bool _completed = false;

        [Inject]
        public void Construct(ITasksDataService tasksDataService, IFriendsDataService friendsDataService,
            IPlayerDataService playerDataService)
        {
            _friendsDataService = friendsDataService;
            _tasksDataService = tasksDataService;
            _playerDataService = playerDataService;
        }

        public void Initialize(TaskConfig taskConfig, Action<TaskConfig> onClaimReward)
        {
            _onClaimTask = onClaimReward;
            _taskConfig = taskConfig;

            _title.text = taskConfig.TaskName;
            _icon.sprite = taskConfig.TaskIcon;
            _reward.text = MoneyConverter.ConvertToSpaceValue(taskConfig.Reward);

            _completed = _tasksDataService.IsTaskComplete(taskConfig.TaskId);

            Utils.ReworkPoint("Need localization for claim button");
            if (_completed)
                _claimButtonTitle.text = Phrases.COMPLETE;
            else
                _claimButtonTitle.text = Phrases.CHECK;
        }

        protected override void OnClaimButtonClick()
        {
            base.OnClaimButtonClick();
            if (_completed)
                return;

            if (_taskConfig.TaskType == TaskType.Friends)
            {
                if (_friendsDataService.FriendList.Count >= _taskConfig.FriendsCount)
                    _onClaimTask?.Invoke(_taskConfig);
            }
            else
            {
                if (!_playerDataService.StockId.IsNullOrWhitespace())
                    _onClaimTask?.Invoke(_taskConfig);
            }
        }
    }
}