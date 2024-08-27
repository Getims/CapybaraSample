using System;
using System.Collections.Generic;
using Main.Scripts.Data.Core;
using Main.Scripts.Data.DataObjects;

namespace Main.Scripts.Data.Services
{
    public interface ITasksDataService
    {
        int CurrentDailyTask { get; }
        int LastDailyTaskClaimTime { get; }
        IReadOnlyCollection<string> CompletedTasksId { get; }
        bool IsDailyTaskClaimed { get; }

        void SetDailyTask(int number, bool autosave = true);
        void SetDailyTaskClaimTime(int time, bool autosave = true);
        void SetDailyTaskClaimed(bool isClaimed, bool autosave = true);
        void AddCompletedTaskId(string taskId, bool autosave = true);
        bool IsTaskComplete(string taskId);
        void SaveData();
    }

    public class TasksDataService : DataService, ITasksDataService
    {
        private readonly TasksData _tasksData;

        public int CurrentDailyTask => _tasksData.CurrentDailyTask;
        public int LastDailyTaskClaimTime => _tasksData.LastDailyTaskClaimTime;
        public IReadOnlyCollection<string> CompletedTasksId => _tasksData.CompletedTasksId;
        public bool IsDailyTaskClaimed => _tasksData.DailyTaskClaimed;

        protected TasksDataService(IDatabase database) : base(database)
        {
            _tasksData = database.GetData<TasksData>();
        }

        public void SetDailyTask(int number, bool autosave = true)
        {
            _tasksData.CurrentDailyTask = Math.Max(number, 0);
            TryToSave(autosave);
        }

        public void SetDailyTaskClaimTime(int time, bool autosave = true)
        {
            _tasksData.LastDailyTaskClaimTime = time;
            TryToSave(autosave);
        }

        public void SetDailyTaskClaimed(bool isClaimed, bool autosave = true)
        {
            _tasksData.DailyTaskClaimed = isClaimed;
            TryToSave(autosave);
        }

        public void AddCompletedTaskId(string taskId, bool autosave = true)
        {
            if (IsTaskComplete(taskId))
                return;

            _tasksData.CompletedTasksId.Add(taskId);
            TryToSave(autosave);
        }

        public bool IsTaskComplete(string taskId)
        {
            return _tasksData.CompletedTasksId.Exists(task => task.Equals(taskId));
        }

        public void SaveData() => TryToSave(true);
    }
}