using Main.Scripts.Core.Constants;
using Main.Scripts.GameLogic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Main.Scripts.UI.TasksMenu.Tasks
{
    public class DayInfo : MonoBehaviour
    {
        [SerializeField]
        private TextMeshProUGUI _title;

        [SerializeField]
        private TextMeshProUGUI _reward;

        [SerializeField]
        private Image _currentBackground;

        [SerializeField]
        private Image _completeBackground;

        public void Initialize(int dayNumber, long money, bool isCurrent, bool isComplete)
        {
            _title.text = $"{Phrases.DAY} {dayNumber}";
            _reward.text = MoneyConverter.ConvertToShortValue(money);
            _completeBackground.enabled = isComplete;
            _currentBackground.enabled = isCurrent && !isComplete;
        }
    }
}