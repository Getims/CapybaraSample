using System;
using System.Collections.Generic;
using Main.Scripts.Data.Core;

namespace Main.Scripts.Data.DataObjects
{
    [Serializable]
    public class CardsData : GameData
    {
        public List<CardData> Cards = new List<CardData>();
        public List<string> ComboCardsId = new List<string>();
        public bool ComboClaimed = false;
        public long MoneyPerHour = 0;
    }
}