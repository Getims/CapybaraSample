using Main.Scripts.Configs.Core;
using Main.Scripts.Core.Utilities;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Main.Scripts.Configs.Global
{
    public class StockConfig : ScriptableConfig
    {
        [SerializeField]
        [Tooltip("Display name at game")]
        private string _stockName;

        [SerializeField, PreviewField(ObjectFieldAlignment.Left, Height = 100)]
        private Sprite _stockIcon;

        [SerializeField, ReadOnly]
        private string _stockID = Utils.GetUniqueID(8);

        public string StockId => _stockID;
        public Sprite StockIcon => _stockIcon;
        public string StockName => _stockName;

        [Button]
        private void GenerateStockID() =>
            _stockID = Utils.GetUniqueID(8);
    }
}