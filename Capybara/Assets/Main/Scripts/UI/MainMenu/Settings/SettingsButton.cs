using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Main.Scripts.UI.MainMenu.Settings
{
    public class SettingsButton : MonoBehaviour
    {
        [SerializeField]
        private Button _button;

        [SerializeField]
        private TextMeshProUGUI _title;

        [SerializeField]
        private TextMeshProUGUI _info;

        public event Action OnClick;

        public void SetTitle(string title)
        {
            _title.text = title;
        }

        public void SetInfo(string info)
        {
            _info.text = info;
        }

        private void Start()
        {
            _button.onClick.AddListener(OnButtonClick);
        }

        private void OnDestroy()
        {
            _button.onClick.RemoveListener(OnButtonClick);
        }

        private void OnButtonClick() => OnClick?.Invoke();
    }
}