using Main.Scripts.Configs.Global;
using Main.Scripts.Core.Constants;
using Main.Scripts.Core.Utilities;
using Main.Scripts.Data.Services;
using Main.Scripts.Infrastructure.Providers.Configs;
using Main.Scripts.UI.Common;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Main.Scripts.UI.MainMenu.Stock
{
    public class StockChangedPopup : PopupPanel
    {
        [SerializeField]
        private Image _stockIcon;

        [SerializeField]
        private TextMeshProUGUI _infoText;

        private StockConfig _stockConfig;

        [Inject]
        public void Construct(IGlobalConfigProvider globalConfigProvider, IPlayerDataService playerDataService)
        {
            GlobalConfig globalConfig = globalConfigProvider.Config;
            _stockConfig = globalConfig.GetStockConfig(playerDataService.StockId);
        }

        public override void Initialize()
        {
            _stockIcon.sprite = _stockConfig.StockIcon;
            Utils.ReworkPoint("Need localization for stock message");
            _infoText.text = $"{Phrases.STOCK_CONTRACT_INFO} {_stockConfig.StockName}";
        }
    }
}