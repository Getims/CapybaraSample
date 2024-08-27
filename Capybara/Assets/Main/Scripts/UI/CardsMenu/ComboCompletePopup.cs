using Main.Scripts.GameLogic;
using Main.Scripts.UI.Common;
using TMPro;
using UnityEngine;

namespace Main.Scripts.UI.CardsMenu
{
    public class ComboCompletePopup : PopupPanel
    {
        [SerializeField]
        private TextMeshProUGUI _reward;

        public void Initialize(long reward)
        {
            _reward.text = MoneyConverter.ConvertToSpaceValue(reward);
        }
    }
}