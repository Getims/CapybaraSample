using System;
using Main.Scripts.Configs.Tasks;
using Main.Scripts.Core.Constants;
using Main.Scripts.Core.Enums;
using Main.Scripts.Core.Utilities;
using Main.Scripts.Data.Services;
using Main.Scripts.GameLogic;
using Main.Scripts.GameLogic.External;
using Main.Scripts.UI.Common;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Main.Scripts.UI.TasksMenu
{
    public class UrlLinkTaskPopup : PopupPanel
    {
        [SerializeField]
        private Image _icon;

        [SerializeField]
        private TextMeshProUGUI _title;

        [SerializeField]
        private TextMeshProUGUI _description;

        [SerializeField]
        private GameObject _descriptionContainer;

        [SerializeField]
        private TextMeshProUGUI _reward;

        [SerializeField]
        private TextMeshProUGUI _claimButtonTitle;

        private ITasksDataService _tasksDataService;
        private IRemoteDataService _remoteDataService;

        private Action<TaskConfig> _onClaimTask;
        private TaskConfig _taskConfig;
        private bool _completed = false;

        [Inject]
        public void Construct(ITasksDataService tasksDataService, IRemoteDataService remoteDataService)
        {
            _remoteDataService = remoteDataService;
            _tasksDataService = tasksDataService;
        }

        public void Initialize(TaskConfig taskConfig, Action<TaskConfig> onClaimReward)
        {
            _onClaimTask = onClaimReward;
            _taskConfig = taskConfig;

            _title.text = taskConfig.TaskName;
            _icon.sprite = taskConfig.TaskIcon;
            _description.text = taskConfig.TaskDescription;
            _reward.text = MoneyConverter.ConvertToSpaceValue(taskConfig.Reward);

            _completed = _tasksDataService.IsTaskComplete(taskConfig.TaskId);

            Utils.ReworkPoint("Need localization for claim button");
            if (taskConfig.TaskType == TaskType.Watch)
            {
                _claimButtonTitle.text = Phrases.WATCH_VIDEO;
                _descriptionContainer.SetActive(true);
            }
            else
            {
                _claimButtonTitle.text = Phrases.SUBSCRIBE;
                _descriptionContainer.SetActive(false);
            }
        }

        protected override void OnClaimButtonClick()
        {
            base.OnClaimButtonClick();
            Application.OpenURL(_taskConfig.TaskLink);

            if (!_completed && _remoteDataService.TryToCompleteTask(_taskConfig.TaskId))
                _onClaimTask?.Invoke(_taskConfig);
        }
    }
}