using System;
using Main.Scripts.Configs.Global;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Main.Scripts.UI.MainMenu.Stock
{
    public class StockButton : MonoBehaviour
    {
        [SerializeField]
        private Button _button;

        [SerializeField]
        private GameObject _selectedBackground;

        [SerializeField]
        private Image _icon;

        [SerializeField]
        private TextMeshProUGUI _name;

        [SerializeField]
        private Image _baseIcon;

        [SerializeField]
        private Image _selectedIcon;

        private Action<StockConfig> _onButtonClick;
        private StockConfig _stockConfig;

        public void Initialize(Action<StockConfig> onButtonClick, StockConfig stockConfig)
        {
            _stockConfig = stockConfig;
            _onButtonClick = onButtonClick;

            _icon.sprite = stockConfig.StockIcon;
            _name.text = stockConfig.StockName;
        }

        public void SetSelected(string stockId)
        {
            SetSelected(stockId.Equals(_stockConfig.StockId));
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
            _onButtonClick?.Invoke(_stockConfig);
    }
}