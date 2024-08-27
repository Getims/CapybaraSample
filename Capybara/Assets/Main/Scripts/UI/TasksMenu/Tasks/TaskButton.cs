using System;
using Main.Scripts.Configs.Tasks;
using Main.Scripts.Core.Constants;
using Main.Scripts.GameLogic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Main.Scripts.UI.TasksMenu.Tasks
{
    public class TaskButton : MonoBehaviour
    {
        [SerializeField]
        private Button _button;

        [SerializeField]
        private Image _icon;

        [SerializeField]
        private TextMeshProUGUI _title;

        [SerializeField]
        private TextMeshProUGUI _reward;

        [SerializeField]
        private GameObject _checkmark;

        [SerializeField]
        private CanvasGroup _backgroundCanvasGroup;

        private TaskConfig _taskConfig;
        private Action<TaskConfig> _onClick;

        public event Action OnClick;

        public void Setup(TaskConfig taskConfig, Action<TaskConfig> onClick)
        {
            _taskConfig = taskConfig;
            SetTitle(_taskConfig.TaskName);
            SetIcon(_taskConfig.TaskIcon);
            SetReward(_taskConfig.Reward);
            _onClick = onClick;
        }

        public void SetTitle(string title) =>
            _title.text = title;

        public void SetReward(long reward) =>
            _reward.text = $"+{MoneyConverter.ConvertToSpaceValue(reward)}";

        public void SetIcon(Sprite icon) =>
            _icon.sprite = icon;

        public void SetState(bool completed)
        {
            _checkmark.SetActive(completed);
            _backgroundCanvasGroup.alpha = completed ? Numbers.NOT_ACTIVE_BUTTON_ALPHA : 1;
        }

        public string GetTaskId()
        {
            if (_taskConfig != null)
                return _taskConfig.TaskId;
            return string.Empty;
        }

        private void Start() =>
            _button.onClick.AddListener(OnButtonClick);

        private void OnDestroy() =>
            _button.onClick.RemoveListener(OnButtonClick);

        private void OnButtonClick()
        {
            OnClick?.Invoke();

            if (_taskConfig != null)
                _onClick?.Invoke(_taskConfig);
        }
    }
}