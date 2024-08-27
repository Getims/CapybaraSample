using System;
using Main.Scripts.GameLogic.Sound;
using Main.Scripts.UI.Base;
using Main.Scripts.UI.Base.UIAnimator;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Main.Scripts.UI.TapGame
{
    public class TurboButton : UIPanel
    {
        [SerializeField]
        private Button _button;

        [SerializeField]
        private UIAnimator _uiAnimator;

        private ISoundService _soundService;

        public event Action OnClick;

        [Inject]
        public void Construct(ISoundService soundService)
        {
            _soundService = soundService;
        }

        public override void Show()
        {
            base.Show();
            _uiAnimator.Play();
        }

        public override void Hide(bool instant)
        {
            base.Hide(instant);
            _targetCG.interactable = false;
            _uiAnimator.Stop();
        }

        private void Start()
        {
            _button.onClick.AddListener(OnButtonClick);
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();
            _button.onClick.RemoveListener(OnButtonClick);
        }

        private void OnButtonClick()
        {
            Hide(true);
            OnClick?.Invoke();
            _soundService.PlayTurboPickSound();
        }
    }
}