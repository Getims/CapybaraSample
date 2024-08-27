using Main.Scripts.Configs.Core;
using Main.Scripts.Core.Enums;
using Main.Scripts.Core.Utilities;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Main.Scripts.Configs.Tasks
{
    public class TaskConfig : ScriptableConfig
    {
        [SerializeField, ReadOnly]
        private string _taskID = Utils.GetUniqueID(8);

        [SerializeField]
        private TaskType _taskType = TaskType.NULL;

        [SerializeField]
        private string _taskName;

        [SerializeField]
        private Sprite _taskIcon;

        [SerializeField]
        [ShowIf(nameof(_taskType), TaskType.Watch)]
        private string _taskDescription;

        [SerializeField]
        [ShowIf(nameof(NeedLink))]
        private string _taskLink;

        [SerializeField]
        private long _reward;

        [SerializeField, MinValue(1)]
        [ShowIf(nameof(_taskType), TaskType.Friends)]
        private int _friendsCount = 1;

        public string TaskId => _taskID;
        public TaskType TaskType => _taskType;
        public string TaskName => _taskName;
        public Sprite TaskIcon => _taskIcon;
        public string TaskDescription => _taskDescription;
        public string TaskLink => _taskLink;
        public long Reward => _reward;
        public int FriendsCount => _friendsCount;

        private bool NeedLink => _taskType == TaskType.Watch ||
                                 _taskType == TaskType.Subscribe;

        [Button]
        private void GenerateTaskID() =>
            _taskID = Utils.GetUniqueID(8);


#if UNITY_EDITOR
        public override string GetConfigCategory() =>
            ConfigsCategories.TASKS_CATEGORY;
#endif
    }
}