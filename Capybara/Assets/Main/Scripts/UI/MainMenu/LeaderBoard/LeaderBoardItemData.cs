using System;
using Main.Scripts.GameLogic.External;
using UnityEngine;

namespace Main.Scripts.UI.MainMenu.LeaderBoard
{
    [Serializable]
    public class LeaderBoardItemData : AccountData
    {
        private bool _isCurrentPlayer = false;
        private Sprite _stockIcon;
        private int _place;

        public bool IsCurrentPlayer => _isCurrentPlayer;
        public Sprite StockIcon => _stockIcon;
        public int Place => _place;

        public LeaderBoardItemData(Sprite accountIcon, string accountName, Sprite stockIcon, long money,
            bool isCurrentPlayer = false, int place = 101, string stockId = "", int accountLevel = 0) : base(
            accountName, money, stockId, accountLevel)
        {
            _isCurrentPlayer = isCurrentPlayer;
            _accountIcon = accountIcon;
            _accountName = accountName;
            _stockIcon = stockIcon;
            _money = money;
            _place = place;
        }

        public void SetPlace(int place = 101) =>
            _place = place;
    }
}