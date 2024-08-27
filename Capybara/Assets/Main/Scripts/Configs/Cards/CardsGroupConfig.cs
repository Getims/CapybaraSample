using System.Collections.Generic;
using Main.Scripts.Configs.Core;
using Main.Scripts.Core.Utilities;
using Sirenix.OdinInspector;
using Sirenix.Utilities;
using UnityEngine;

namespace Main.Scripts.Configs.Cards
{
    public class CardsGroupConfig : ScriptableConfig
    {
        [SerializeField,
         ListDrawerSettings(Expanded = false, ShowIndexLabels = true, ListElementLabelName = "GroupInfo")]
        private List<CardGroup> _cardsGroups = new List<CardGroup>();

        [SerializeField, HideLabel, InlineProperty]
        [Title("Cards Combo")]
        private CardsComboConfig _cardsComboConfig;

        public IReadOnlyCollection<CardGroup> CardsGroups => _cardsGroups;
        public int GroupsCount => _cardsGroups.Count;
        public CardsComboConfig ComboConfig => _cardsComboConfig;

        public bool HasCards(int groupId) =>
            groupId < GroupsCount && _cardsGroups[groupId].Cards.Count > 0;

        public CardGroup GetCardGroup(int groupId) =>
            groupId < GroupsCount ? _cardsGroups[groupId] : null;

        public CardConfig GetCardConfig(string cardId)
        {
            CardConfig result = null;
            foreach (CardGroup cardGroup in _cardsGroups)
            {
                result = cardGroup.GetCardConfig(cardId);
                if (result != null)
                    return result;
            }

            return result;
        }


#if UNITY_EDITOR
        public override string GetConfigCategory() =>
            ConfigsCategories.CARDS_CATEGORY;
#endif

#if UNITY_EDITOR
        [HorizontalGroup("Helpers"), Title("Help functions")]
        [Button]
        private void CheckCard(CardConfig cardConfig, bool logIfFoundOneTime = true)
        {
            string result = string.Empty;
            string checkId = cardConfig.CardId;
            int foundCount = 0;

            foreach (CardGroup cardsGroup in _cardsGroups)
            {
                string groupName = cardsGroup.GroupName;
                string cards = string.Empty;
                int i = -1;
                foreach (CardConfig card in cardsGroup.Cards)
                {
                    i++;
                    if (card.CardId.Equals(checkId))
                    {
                        cards += $"[{i}],";
                        foundCount++;
                    }
                }

                if (!cards.IsNullOrWhitespace())
                    result += $" Group [{groupName}] positions: {cards} \n";
            }

            if (result.IsNullOrWhitespace())
                Utils.Log(this, $"Card [{cardConfig.name}] not found in groups");
            else
            {
                if (foundCount > 1 || (foundCount == 1 && logIfFoundOneTime))
                    Utils.Log(this, $"Card [{cardConfig.name}] found in: {result}");
            }
        }

        [Button]
        private void CheckAllCards()
        {
            foreach (CardGroup cardsGroup in _cardsGroups)
            {
                foreach (CardConfig card in cardsGroup.Cards)
                    CheckCard(card, false);
            }
        }

#endif
    }
}