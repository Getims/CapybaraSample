using System.Collections.Generic;
using System.Linq;
using Main.Scripts.Configs.Core;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Main.Scripts.Configs.Tasks
{
    public class TasksGroupConfig : ScriptableConfig
    {
        [SerializeField, ListDrawerSettings(ShowIndexLabels = true), LabelWidth(80)]
        private List<long> _dailyRewards = new List<long>();

        [SerializeField]
        private List<TaskConfig> _tasks = new List<TaskConfig>();

        public IReadOnlyCollection<long> DailyRewards => _dailyRewards;
        public IReadOnlyCollection<TaskConfig> Tasks => _tasks;

        public TaskConfig GetTaskConfig(string taskId) =>
            _tasks.FirstOrDefault(task => task.TaskId.Equals(taskId));

        public long GetDayReward(int day)
        {
            if (day >= _dailyRewards.Count)
                return 0;

            return _dailyRewards[day];
        }

#if UNITY_EDITOR
        public override string GetConfigCategory() =>
            ConfigsCategories.TASKS_CATEGORY;
#endif
    }
}