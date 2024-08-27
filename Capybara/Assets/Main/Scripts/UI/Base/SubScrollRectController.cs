using UnityEngine;
using UnityEngine.UI;

namespace Main.Scripts.UI.Base
{
    public class SubScrollRectController : MonoBehaviour
    {
        [SerializeField]
        public ScrollRect _mainScrollRect;

        [SerializeField]
        public ScrollRect _childScrollRect;

        [SerializeField]
        public float _scrollStep = 100f;

        private float _previousValue = 1;

        private void Start()
        {
            _childScrollRect.onValueChanged.AddListener(OnChildScrollValueChanged);
        }

        private void OnChildScrollValueChanged(Vector2 value)
        {
            float currentValue = value.y;
            if (_childScrollRect.verticalNormalizedPosition >= 0.99f)
            {
                float newVerticalPosition = _mainScrollRect.verticalNormalizedPosition +
                                            _scrollStep / _mainScrollRect.content.rect.height;
                _mainScrollRect.verticalNormalizedPosition = Mathf.Clamp(newVerticalPosition, 0f, 1f);

                currentValue = (currentValue + _previousValue + 0.99f) / 3f;
                _childScrollRect.verticalNormalizedPosition = currentValue;
            }
            else if (_childScrollRect.verticalNormalizedPosition <= 0.01f)
            {
                float newVerticalPosition = _mainScrollRect.verticalNormalizedPosition -
                                            _scrollStep / _mainScrollRect.content.rect.height;
                _mainScrollRect.verticalNormalizedPosition = Mathf.Clamp(newVerticalPosition, 0f, 1f);

                currentValue = (currentValue + _previousValue + 0.01f) / 3f;
                _childScrollRect.verticalNormalizedPosition = currentValue;
            }

            _previousValue = currentValue;
        }
    }
}