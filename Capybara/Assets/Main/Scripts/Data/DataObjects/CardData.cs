using System;
using UnityEngine;

namespace Main.Scripts.Data.DataObjects
{
    [Serializable]
    public class CardData
    {
        [field: SerializeField]
        public string CardId { get; private set; }

        [field: SerializeField]
        public int CardLevel { get; private set; }

        public CardData(string cardId, int cardLevel)
        {
            CardId = cardId;
            CardLevel = cardLevel;
        }

        internal void SetLevel(int cardLevel)
        {
            CardLevel = cardLevel;
        }
    }
}