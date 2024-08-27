using System;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Main.Scripts.GameLogic.External
{
    [Serializable]
    public class AccountData
    {
        [SerializeField]
        protected string _accountName;

        [SerializeField]
        protected long _money;

        [SerializeField]
        protected string _stockId;

        [SerializeField]
        protected int _accountLevel = 0;

        [SerializeField, PreviewField(Height = 80, Alignment = ObjectFieldAlignment.Left)]
        protected Sprite _accountIcon;

        public Sprite AccountIcon => _accountIcon;
        public string AccountName => _accountName;
        public long Money => _money;
        public int AccountLevel => _accountLevel;
        public string StockId => _stockId;

        public AccountData(string accountName, long money, string stockId, int accountLevel)
        {
            _accountName = accountName;
            _money = money;
            _stockId = stockId;
            _accountLevel = accountLevel;
        }
    }
}