using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Main.Scripts.UI.Common
{
    public abstract class ControlButtonBase : MonoBehaviour
    {
        [SerializeField]
        protected Button _button;

        [SerializeField]
        protected Image _selectedBackground;

        [SerializeField]
        protected TextMeshProUGUI _buttonTitle;

        [SerializeField]
        protected Color _selectTitleColor = Color.white;

        [SerializeField]
        protected Color _baseTitleColor = Color.white;

        public bool IsActive => gameObject.activeSelf || gameObject.activeInHierarchy;

        public virtual void SetSelected(bool isSelected)
        {
            _selectedBackground.enabled = isSelected;
            _buttonTitle.color = isSelected ? _selectTitleColor : _baseTitleColor;
        }

        public virtual void SetActive(bool isActive) =>
            gameObject.SetActive(isActive);

        protected void Start()
        {
            _button.onClick.AddListener(OnButtonClick);
        }

        protected void OnDestroy()
        {
            _button.onClick.RemoveListener(OnButtonClick);
        }

        protected abstract void OnButtonClick();
    }
}