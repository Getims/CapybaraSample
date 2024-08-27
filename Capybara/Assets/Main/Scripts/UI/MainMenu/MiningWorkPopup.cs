using System;
using Main.Scripts.Configs.Global;
using Main.Scripts.Core.Utilities;
using Main.Scripts.Data.Services;
using Main.Scripts.GameLogic;
using Main.Scripts.Infrastructure.Providers.Configs;
using Main.Scripts.UI.Common;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Main.Scripts.UI.MainMenu
{
    public class MiningWorkPopup : PopupPanel
    {
        private const string INFO_TEXT = "Спасибо,";

        [SerializeField]
        private Image _stockIcon;

        [SerializeField]
        private TextMeshProUGUI _buttonText;

        [SerializeField]
        private TextMeshProUGUI _moneyGain;

        private Func<string, StockConfig> GetConfig;

        [Inject]
        public void Construct(IGlobalConfigProvider globalConfigProvider, IPlayerDataService playerDataService)
        {
            GlobalConfig globalConfig = globalConfigProvider.Config;
            GetConfig = (string stockId) => globalConfig.GetStockConfig(stockId);
        }

        public void Initialize(string stockId, long money)
        {
            StockConfig stockConfig = GetConfig(stockId);
            _stockIcon.sprite = stockConfig.StockIcon;

            Utils.ReworkPoint("Need localization for stock message");
            _buttonText.text = $"{INFO_TEXT} {stockConfig.StockName}♥";
            _moneyGain.text = $"+{MoneyConverter.ConvertToShortValue(money)}";
        }
    }
}