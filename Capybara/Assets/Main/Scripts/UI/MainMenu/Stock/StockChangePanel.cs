using System;
using System.Collections.Generic;
using Main.Scripts.Configs.Global;
using Main.Scripts.Core.Constants;
using Main.Scripts.Core.Utilities;
using Main.Scripts.Data.Services;
using Main.Scripts.Infrastructure.Providers.Configs;
using Main.Scripts.UI.Base;
using Sirenix.Utilities;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Main.Scripts.UI.MainMenu.Stock
{
    public class StockChangePanel : UIPanel
    {
        [SerializeField]
        private Button _closeButton;

        [SerializeField]
        private StockButton _stockButtonPrefab;

        [SerializeField]
        private Transform _buttonsContainer;

        [SerializeField]
        private TextMeshProUGUI _title;

        private List<StockButton> _stockButtons = new List<StockButton>();

        private GlobalConfig _globalConfig;
        private IUIElementFactory _uiElementFactory;
        private IPlayerDataService _playerDataService;

        public event Action OnStockClick;

        [Inject]
        public void Construct(IGlobalConfigProvider globalConfigProvider, IPlayerDataService playerDataService,
            IUIElementFactory uiElementFactory)
        {
            _playerDataService = playerDataService;
            _uiElementFactory = uiElementFactory;
            _globalConfig = globalConfigProvider.Config;
        }

        public override void Initialize()
        {
            IReadOnlyCollection<StockConfig> stocks = _globalConfig.Stocks;
            string selectedStockId = _playerDataService.StockId;
            SelectTitle(selectedStockId);

            foreach (StockConfig stockConfig in stocks)
            {
                StockButton stockButton = _uiElementFactory.Create(_stockButtonPrefab, _buttonsContainer);
                stockButton.Initialize(OnStockButtonClick, stockConfig);
                _stockButtons.Add(stockButton);
                stockButton.SetSelected(selectedStockId);
            }
        }

        private void SelectTitle(string selectedStockId)
        {
            Utils.ReworkPoint("Need localization for title");
            if (selectedStockId.IsNullOrWhitespace())
                _title.text = Phrases.STOCK_PANEL_FIRST_ENTER_TITLE;
            else
                _title.text = Phrases.STOCK_PANEL_BASE_TITLE;
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

        private void OnStockButtonClick(StockConfig stockConfig)
        {
            _playerDataService.SetStock(stockConfig.StockId);
            OnStockClick?.Invoke();
            Hide();
        }
    }
}