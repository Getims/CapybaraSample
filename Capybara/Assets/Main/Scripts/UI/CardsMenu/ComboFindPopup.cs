using System;
using Main.Scripts.GameLogic.Timers;
using Main.Scripts.UI.Base;
using Main.Scripts.UI.Common;
using Sirenix.OdinInspector;
using UnityEngine;
using Zenject;

namespace Main.Scripts.UI.CardsMenu
{
    internal class ComboFindPopup : UIPanel
    {
        [SerializeField]
        private SunRays _sunRays;

        [SerializeField, MinValue(0)]
        private float _onScreenTime;

        private ITimersController _timersController;

        [Inject]
        public void Construct(ITimersController timersController)
        {
            _timersController = timersController;
        }

        public override void Show()
        {
            base.Show();
            _sunRays.PlayAnimation();

            float closeDelay = FadeTime + _onScreenTime;
            _timersController.AddTimer(new VirtualTimer(closeDelay, (Action) null, Hide));
        }

        public override void Hide()
        {
            base.Hide();
            _sunRays.StopAnimation();
        }
    }
}