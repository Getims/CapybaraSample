using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Main.Scripts.UI.MainMenu.Settings
{
    public class LanguageButton : MonoBehaviour
    {
        [SerializeField]
        private Button _button;

        [SerializeField]
        private GameObject _selectedBackground;

        [SerializeField]
        private TextMeshProUGUI _name;

        [SerializeField]
        private Image _baseIcon;

        [SerializeField]
        private Image _selectedIcon;

        private Action<int> _onButtonClick;
        private int _languageId;

        public void Initialize(Action<int> onButtonClick, int languageId, string name)
        {
            _languageId = languageId;
            _onButtonClick = onButtonClick;
            _name.text = name;
        }

        public void SetSelected(int languageId)
        {
            SetSelected(languageId == _languageId);
        }

        public void SetSelected(bool isSelected)
        {
            _selectedBackground.SetActive(isSelected);
            _baseIcon.enabled = !isSelected;
            _selectedIcon.enabled = isSelected;
        }

        private void Start()
        {
            _button.onClick.AddListener(OnButtonClick);
        }

        private void OnDestroy()
        {
            _button.onClick.RemoveListener(OnButtonClick);
        }

        private void OnButtonClick() =>
            _onButtonClick?.Invoke(_languageId);
    }
}