using System;
using DG.Tweening;
using Main.Scripts.Core.Enums;
using UnityEngine;

namespace Main.Scripts.UI.TapGame.Core
{
    [Serializable]
    public class TapAnimator
    {
        [SerializeField]
        private RectTransform _rectTransform;

        [SerializeField]
        private float _animationTime = 0.15f;

        [SerializeField]
        private float _scale = 0.85f;

        [SerializeField]
        private float _rotation = 15;

        private Vector3 _startScale;
        private Vector3 _finalScale;
        private Vector3 _startRotation;
        private Vector3 _finalRotation;

        private Tweener _scaleTW;
        private Tweener _rotateTW;

        public void Start()
        {
            _startScale = _rectTransform.localScale;
            _startRotation = _rectTransform.localEulerAngles;
        }

        public void OnDestroy()
        {
            _scaleTW?.Kill();
            _rotateTW?.Kill();
        }

        public void AnimateTapDown(TapZone tapZone)
        {
            _scaleTW?.Complete();
            _rotateTW?.Complete();

            _finalScale = tapZone == TapZone.Center ? Vector3.one * _scale : Vector3.one * .95f;
            _scaleTW = _rectTransform.DOScale(_finalScale, _animationTime)
                .SetEase(Ease.InSine)
                .SetUpdate(true);

            switch (tapZone)
            {
                case TapZone.Center:
                    break;
                case TapZone.TopLeft:
                    _finalRotation = new Vector3(_rotation, _rotation, 0);
                    Rotate(_finalRotation, Ease.OutCubic);
                    break;
                case TapZone.TopRight:
                    _finalRotation = new Vector3(_rotation, -_rotation, 0);
                    Rotate(_finalRotation, Ease.OutCubic);
                    break;
                case TapZone.BottomLeft:
                    _finalRotation = new Vector3(-_rotation, _rotation, 0);
                    Rotate(_finalRotation, Ease.OutCubic);
                    break;
                case TapZone.BottomRight:
                    _finalRotation = new Vector3(-_rotation, -_rotation, 0);
                    Rotate(_finalRotation, Ease.OutCubic);
                    break;
            }
        }

        public void AnimateTapUp(TapZone tapZone)
        {
            _scaleTW?.Complete();
            _rotateTW?.Complete();

            _scaleTW = _rectTransform.DOScale(_startScale, _animationTime)
                .SetEase(Ease.OutSine)
                .SetUpdate(true);

            switch (tapZone)
            {
                case TapZone.Center:
                    break;
                case TapZone.TopLeft:
                case TapZone.TopRight:
                case TapZone.BottomLeft:
                case TapZone.BottomRight:
                    Rotate(_startRotation, Ease.OutSine);
                    break;
            }
        }

        private void Rotate(Vector3 finalRotation, Ease ease)
        {
            _rotateTW = _rectTransform.transform.DORotate(finalRotation, _animationTime, RotateMode.Fast)
                .SetEase(ease)
                .SetUpdate(true);
        }
    }
}