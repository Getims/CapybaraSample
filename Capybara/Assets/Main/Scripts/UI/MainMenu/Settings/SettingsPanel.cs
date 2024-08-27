using System;
using Main.Scripts.Configs.Global;
using Main.Scripts.Core.Utilities;
using Main.Scripts.Data.Services;
using Main.Scripts.Infrastructure.Providers.Configs;
using Main.Scripts.UI.Base;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Main.Scripts.UI.MainMenu.Settings
{
    public class SettingsPanel : UIPanel
    {
        [SerializeField]
        private Button _closeButton;

        [SerializeField]
        private SettingsButton _languageButton;

        [SerializeField]
        private SettingsButton _stockButton;

        [SerializeField]
        private SettingsButton _deleteButton;

        [SerializeField]
        private Button _privacyButton;

        [SerializeField]
        private SoundButton _soundButton;

        [SerializeField]
        private SoundButton _musicButton;

        private string _privacyPolicyLink;

        public event Action OnStockClick;
        public event Action OnLanguageClick;

        [Inject]
        public void Construct(IGlobalConfigProvider globalConfigProvider, IPlayerDataService playerDataService)
        {
            GlobalConfig globalConfig = globalConfigProvider.Config;
            _privacyPolicyLink = globalConfig.LinksConfig.PrivacyPolicyLink;

            StockConfig stockConfig = globalConfig.GetStockConfig(playerDataService.StockId);
            _stockButton.SetInfo(stockConfig.StockName);
        }

        public override void Show()
        {
            base.Show();
            UpdateInfo();
        }

        public override void Hide()
        {
            base.Hide();
            DestroySelfDelayed();
        }

        protected void Start()
        {
            _closeButton.onClick.AddListener(OnCloseClick);
            _languageButton.OnClick += OnLanguageButtonClick;
            _stockButton.OnClick += OnStockButtonClick;
            _deleteButton.OnClick += OnDeleteButtonClick;
            _privacyButton.onClick.AddListener(OnPrivacyButtonClick);
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();
            _closeButton.onClick.RemoveListener(OnCloseClick);
            _languageButton.OnClick -= OnLanguageButtonClick;
            _stockButton.OnClick -= OnStockButtonClick;
            _deleteButton.OnClick -= OnDeleteButtonClick;
            _privacyButton.onClick.RemoveListener(OnPrivacyButtonClick);
        }

        private void UpdateInfo()
        {
            _soundButton.UpdateInfo();
            _musicButton.UpdateInfo();
        }

        private void OnCloseClick() =>
            Hide();

        private void OnPrivacyButtonClick()
        {
            Application.OpenURL(_privacyPolicyLink);
        }

        private void OnDeleteButtonClick()
        {
            Utils.ReworkPoint("Add delete account logic");
        }

        private void OnStockButtonClick()
        {
            OnStockClick?.Invoke();
            Hide();
        }

        private void OnLanguageButtonClick()
        {
            OnLanguageClick?.Invoke();
            Hide();
        }
    }
}