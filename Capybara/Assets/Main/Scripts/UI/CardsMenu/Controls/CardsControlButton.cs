using System;
using Main.Scripts.UI.Common;

namespace Main.Scripts.UI.CardsMenu.Controls
{
    public class CardsControlButton : ControlButtonBase
    {
        private Action<int> _onButtonClick;
        private int _cardGroupId;

        public void Initialize(int cardGroupId, string groupName, Action<int> onButtonClick)
        {
            _cardGroupId = cardGroupId;
            _onButtonClick = onButtonClick;
            _buttonTitle.text = groupName;
            SetSelected(false);
        }

        protected override void OnButtonClick()
        {
            _onButtonClick?.Invoke(_cardGroupId);
        }
    }
}