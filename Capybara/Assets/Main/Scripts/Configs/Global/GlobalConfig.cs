using System;
using System.Collections.Generic;
using System.Linq;
using Main.Scripts.Configs.Core;
using UnityEngine;

namespace Main.Scripts.Configs.Global
{
    [Serializable]
    public class GlobalConfig : ScriptableConfig
    {
        [SerializeField]
        private VolumeConfig _volumeConfig;

        [SerializeField]
        private AudioClipsListConfig _audioClipsListConfig;

        [SerializeField]
        private List<StockConfig> _stocks = new List<StockConfig>();

        [SerializeField]
        private LinksConfig _linksConfig;

        [SerializeField, Range(1, 24)]
        [Tooltip("How long, in hours, can money be mined")]
        private int _maxMiningTime = 3;

        public VolumeConfig VolumeConfig => _volumeConfig;
        public AudioClipsListConfig AudioClipsListConfig => _audioClipsListConfig;
        public IReadOnlyCollection<StockConfig> Stocks => _stocks;
        public LinksConfig LinksConfig => _linksConfig;
        public int MaxMiningTime => _maxMiningTime;

        public StockConfig GetStockConfig(string stockId)
        {
            StockConfig stockConfig = _stocks.FirstOrDefault(stock => stock.StockId.Equals(stockId));
            if (stockConfig == null)
            {
                Debug.LogWarning("Stock not found " + stockId);
                stockConfig = _stocks[0];
            }

            return stockConfig;
        }
    }
}