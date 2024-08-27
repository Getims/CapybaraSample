using System;
using Main.Scripts.Configs.Tasks;
using Main.Scripts.Core.Constants;
using Main.Scripts.Core.Utilities;
using Main.Scripts.Data.Services;
using Main.Scripts.GameLogic;
using Main.Scripts.UI.Common;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Main.Scripts.UI.TasksMenu
{
    public class WalletTaskPopup : PopupPanel
    {
        [SerializeField]
        private Image _icon;

        [SerializeField]
        private TextMeshProUGUI _title;

        [SerializeField]
        private TextMeshProUGUI _reward;

        [SerializeField]
        private GameObject _rewardContainer;

        [SerializeField]
        private TextMeshProUGUI _claimButtonTitle;

        private ITasksDataService _tasksDataService;

        private Action<TaskConfig> _onClaimTask;
        private TaskConfig _taskConfig;
        private bool _completed = false;

        [Inject]
        public void Construct(ITasksDataService tasksDataService)
        {
            _tasksDataService = tasksDataService;
        }

        public void Initialize(TaskConfig taskConfig, Action<TaskConfig> onClaimReward)
        {
            _onClaimTask = onClaimReward;
            _taskConfig = taskConfig;

            _title.text = taskConfig.TaskName;
            _icon.sprite = taskConfig.TaskIcon;

            _completed = _tasksDataService.IsTaskComplete(taskConfig.TaskId);

            Utils.ReworkPoint("Need localization for claim button");
            if (_completed)
            {
                _claimButtonTitle.text = Phrases.CONNECTED;
                _rewardContainer.SetActive(false);
            }
            else
            {
                _claimButtonTitle.text = Phrases.CONNECT;
                _reward.text = MoneyConverter.ConvertToSpaceValue(taskConfig.Reward);
                _rewardContainer.SetActive(true);
            }
        }

        protected override void OnClaimButtonClick()
        {
            base.OnClaimButtonClick();
            if (_completed)
                return;

            _onClaimTask?.Invoke(_taskConfig);
        }
    }
}