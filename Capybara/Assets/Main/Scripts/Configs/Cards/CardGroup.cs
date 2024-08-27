using System;
using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Main.Scripts.Configs.Cards
{
    [Serializable]
    public class CardGroup
    {
        [SerializeField]
        private string _groupName;

        [SerializeField, ListDrawerSettings(ShowIndexLabels = true, ListElementLabelName = "ShortName")]
        private List<CardConfig> _cards = new List<CardConfig>();

        public string GroupName => _groupName;
        public IReadOnlyCollection<CardConfig> Cards => _cards;

        protected string GroupInfo => $"{_groupName}, cards: {_cards.Count}";

        public CardConfig GetCardConfig(string _cardID) =>
            _cards.FirstOrDefault(card => card.CardId.Equals(_cardID));
    }
}