using System;
using System.Collections;
using DG.Tweening;
using Main.Scripts.UI.Base;
using UnityEngine;
using UnityEngine.UI;

namespace Main.Scripts.UI.Common
{
    public class PopupPanel : UIPanel
    {
        [SerializeField]
        private PopupResizer _popupResizer;

        [SerializeField]
        private Button _closeButton;

        [SerializeField]
        private Button _claimButton;

        private Coroutine _showCO;
        private Coroutine _hideCO;

        public event Action OnCloseClick;
        public event Action OnClaimClick;

        public override void Show()
        {
            if (_showCO != null)
                StopCoroutine(_showCO);
            _showCO = StartCoroutine(ShowCO());
        }

        public override void Hide()
        {
            if (_showCO != null)
                StopCoroutine(_showCO);
            if (_hideCO != null)
                StopCoroutine(_hideCO);
            _hideCO = StartCoroutine(HideCO());
        }

        private void Start()
        {
            if (_closeButton != null)
                _closeButton.onClick.AddListener(OnCloseButtonClick);

            if (_claimButton != null)
                _claimButton.onClick.AddListener(OnClaimButtonClick);
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();
            StopAllCoroutines();

            if (_closeButton != null)
                _closeButton.onClick.RemoveListener(OnCloseButtonClick);

            if (_claimButton != null)
                _claimButton.onClick.RemoveListener(OnClaimButtonClick);
        }

        protected virtual void OnCloseButtonClick()
        {
            Hide();
            OnCloseClick?.Invoke();
        }

        protected virtual void OnClaimButtonClick()
        {
            Hide();
            OnClaimClick?.Invoke();
        }

        private IEnumerator ShowCO()
        {
            yield return null;

            _popupResizer.Resize();
            yield return null;

            Vector3 localPosition = _popupResizer.transform.localPosition;
            float currentY = localPosition.y;
            localPosition.y -= _popupResizer.TargetHeight;
            _popupResizer.transform.localPosition = localPosition;

            yield return null;

            _popupResizer.transform.DOLocalMoveY(currentY, FadeTime)
                .SetEase(Ease.InSine)
                .SetDelay(FadeTime * .5f);
            base.Show();
        }

        private IEnumerator HideCO()
        {
            yield return null;
            Vector3 localPosition = _popupResizer.transform.localPosition;
            localPosition.y -= _popupResizer.TargetHeight;
            _popupResizer.transform.DOLocalMoveY(localPosition.y, FadeTime)
                .SetEase(Ease.OutSine);
            yield return new WaitForSeconds(FadeTime * .5f);
            base.Hide();
            DestroySelfDelayed();
        }
    }
}