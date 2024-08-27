using System.Collections.Generic;
using System.Linq;
using Main.Scripts.Configs.Tasks;
using Main.Scripts.Core.Constants;
using Main.Scripts.Core.Utilities;
using Main.Scripts.Data.Services;
using Main.Scripts.Infrastructure.Providers.Configs;
using Main.Scripts.UI.Common;
using TMPro;
using UnityEngine;
using Zenject;

namespace Main.Scripts.UI.TasksMenu.Tasks
{
    public class DailyTaskPopup : PopupPanel
    {
        [SerializeField]
        private DayInfo _dayInfoPrefab;

        [SerializeField]
        private Transform _daysContainer;
        
        [SerializeField]
        private TextMeshProUGUI _claimButtonTitle;

        private ITasksDataService _tasksDataService;
        private TasksGroupConfig _tasksConfig;

        private bool _completed = false;

        [Inject]
        public void Construct(ITasksConfigProvider tasksConfigProvider, ITasksDataService tasksDataService)
        {
            _tasksConfig = tasksConfigProvider.Config;
            _tasksDataService = tasksDataService;
        }

        public void Initialize()
        {
            _completed = _tasksDataService.IsDailyTaskClaimed;
            CreateDaysInfo();


            Utils.ReworkPoint("Need localization for claim button");
            if (_completed)
                _claimButtonTitle.text = Phrases.RETURN_NEXT_DAY;
            else
                _claimButtonTitle.text = Phrases.PICK;
        }

        private void CreateDaysInfo()
        {
            _daysContainer.gameObject.SetActive(false);

            int currentDailyTask = _tasksDataService.CurrentDailyTask;

            List<long> rewards = _tasksConfig.DailyRewards.ToList();
            for (int i = 0; i < rewards.Count; i++)
            {
                bool isComplete = i < currentDailyTask;
                bool isCurrent = false;
                if (i == currentDailyTask)
                {
                    isComplete = _completed;
                    isCurrent = true;
                }

                DayInfo dayInfo = Instantiate(_dayInfoPrefab, _daysContainer);
                dayInfo.Initialize(i + 1, rewards[i], isCurrent, isComplete);
            }

            _daysContainer.gameObject.SetActive(true);
        }
    }
}