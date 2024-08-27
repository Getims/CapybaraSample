using System;
using System.Collections.Generic;
using Main.Scripts.Core.Utilities;
using Main.Scripts.UI.Base;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Main.Scripts.UI.MainMenu.Settings
{
    public class LanguageChangePanel : UIPanel
    {
        [SerializeField]
        private Button _closeButton;

        [SerializeField]
        private LanguageButton _languageButtonPrefab;

        [SerializeField]
        private Transform _buttonsContainer;

        private List<LanguageButton> _languageButtons = new List<LanguageButton>();

        private IUIElementFactory _uiElementFactory;

        public event Action<string> OnLanguageClick;

        [Inject]
        public void Construct(IUIElementFactory uiElementFactory)
        {
            _uiElementFactory = uiElementFactory;
        }

        public override void Initialize()
        {
            Utils.ReworkPoint("Rework language panel");

            string[] test = new string[]
            {
                "Русский (Ру)",
                "English (Eng)",
                "Français (Fr)",
                "Deutsch (De)",
                "Español (Es)"
            };
            int id = 0;

            foreach (string language in test)
            {
                LanguageButton languageButton = _uiElementFactory.Create(_languageButtonPrefab, _buttonsContainer);
                languageButton.Initialize(OnLanguageButtonClick, id, language);
                _languageButtons.Add(languageButton);
                languageButton.SetSelected(0);
                id++;
            }
        }

        public override void Hide()
        {
            base.Hide();
            DestroySelfDelayed();
        }

        private void Start()
        {
            _closeButton.onClick.AddListener(OnCloseButtonClick);
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();
            _closeButton.onClick.RemoveListener(OnCloseButtonClick);
        }

        private void OnCloseButtonClick()
        {
            Hide();
        }

        private void OnLanguageButtonClick(int id)
        {
            OnLanguageClick?.Invoke(id.ToString());
            Hide();
        }
    }
}