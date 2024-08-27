using System;
using System.Collections.Generic;
using Main.Scripts.Data.Core;

namespace Main.Scripts.Data.DataObjects
{
    [Serializable]
    public class TasksData : GameData
    {
        public int CurrentDailyTask;
        public int LastDailyTaskClaimTime;
        public bool DailyTaskClaimed;

        public List<string> CompletedTasksId = new List<string>();
    }
}