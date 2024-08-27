using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;

namespace Main.Scripts.UI.Common
{
    [ExecuteAlways]
    [RequireComponent(typeof(RectTransform))]
    public class PopupResizer : MonoBehaviour
    {
        [SerializeField, Range(0, 1)]
        private float _maxHeight = 0.5f;

        [SerializeField, Min(0)]
        private float _top = 0;

        [SerializeField, Min(0)]
        private float _bottom = 0;

        [SerializeField, Required]
        private RectTransform _content;

        [SerializeField]
        private bool _autoResize;

        [SerializeField]
        [ShowIf(nameof(_autoResize))]
        private float _customUpdateTime = -1;

        private RectTransform _rect;
        private RectTransform _parentRectTransform;
        private float _targetHeight;
        private float _lastResizeTime;

        public float TargetHeight => _targetHeight;

        private RectTransform RectTransform => _rect == null
            ? _rect = GetComponent<RectTransform>()
            : _rect;

        private RectTransform ParentRectTransform => _parentRectTransform == null
            ? _parentRectTransform = transform.parent.GetComponent<RectTransform>()
            : _parentRectTransform;

        [Button]
        public void Resize()
        {
            _targetHeight = CalculateSize();
            RectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, _targetHeight);
        }

        private void FixedUpdate()
        {
            if (!_autoResize)
                return;

            if (_customUpdateTime <= 0)
            {
                Resize();
                return;
            }

            float currentTime = Time.time;
            if (currentTime > _lastResizeTime + _customUpdateTime)
            {
                _lastResizeTime = currentTime;
                Resize();
            }
        }

        private float CalculateSize()
        {
            float preferredSize = LayoutUtility.GetPreferredSize(_content, 1) + _top + _bottom;
            float maxHeight = ParentRectTransform.rect.height * _maxHeight;
            float targetHeight = Mathf.Min(preferredSize, maxHeight);

            return targetHeight;
        }
    }
}