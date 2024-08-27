using System;
using Main.Scripts.Core.Enums;
using Main.Scripts.UI.Common;
using UnityEngine;
using UnityEngine.UI;

namespace Main.Scripts.UI.Controls
{
    public class ControlButton : ControlButtonBase
    {
        [SerializeField]
        private GameState _buttonType = GameState.MainMenu;

        [SerializeField]
        private Image _notificationIcon;

        private Action<GameState> _onButtonClick;

        public GameState ButtonType => _buttonType;

        public void Initialize(Action<GameState> onButtonClick)
        {
            _onButtonClick = onButtonClick;
            SetSelected(false);
        }

        public void SetNotification(bool enabled) =>
            _notificationIcon.enabled = enabled;

        protected override void OnButtonClick()
        {
            _onButtonClick?.Invoke(_buttonType);
            SetNotification(false);
        }
    }
}