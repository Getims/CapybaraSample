using System.Collections.Generic;
using System.Linq;
using Main.Scripts.Data.Core;
using Main.Scripts.Data.DataObjects;
using Main.Scripts.Infrastructure.Providers.Events;

namespace Main.Scripts.Data.Services
{
    public interface ICardsDataService
    {
        public int CardsCount { get; }
        IReadOnlyCollection<CardData> Cards { get; }
        IReadOnlyCollection<string> ComboCards { get; }
        bool ComboComplete { get; }
        bool ComboClaimed { get; }
        long MoneyPerHour { get; }

        void AddCard(string cardId, int level = -1, bool autosave = true);
        void UpdateCardLevel(string cardId, int cardLevel, bool autosave = true);
        CardData GetCardData(string cardId);
        bool WasCardBought(string cardId);

        bool WasComboCardBought(string cardId);
        void ClearComboCards(bool autosave = true);
        void SaveData();
        void SetComboClaimed(bool autosave = true);
        void SetMoneyPerHour(long money, bool autosave = true);
        void AddComboCard(string cardId, bool autosave = true);
    }

    public class CardsDataService : DataService, ICardsDataService
    {
        private readonly CardsData _cardsData;
        private readonly UpgradeEventProvider _upgradeEventProvider;

        public int CardsCount => _cardsData.Cards.Count;
        public IReadOnlyCollection<CardData> Cards => _cardsData.Cards;
        public IReadOnlyCollection<string> ComboCards => _cardsData.ComboCardsId;
        public bool ComboComplete => _cardsData.ComboCardsId.Count >= 3;
        public bool ComboClaimed => _cardsData.ComboClaimed;
        public long MoneyPerHour => _cardsData.MoneyPerHour;

        protected CardsDataService(IDatabase database, UpgradeEventProvider upgradeEventProvider) : base(database)
        {
            _cardsData = database.GetData<CardsData>();
            _upgradeEventProvider = upgradeEventProvider;
        }

        public void AddCard(string cardId, int level = -1, bool autosave = true)
        {
            CardData cardData = GetCardData(cardId);

            if (cardData != null)
            {
                int newLevel = level < 0 ? cardData.CardLevel + 1 : level;
                cardData.SetLevel(newLevel);
            }
            else
            {
                int newLevel = level < 0 ? 0 : level;
                _cardsData.Cards.Add(new CardData(cardId, newLevel));
            }

            TryToSave(autosave);
            _upgradeEventProvider.CardBoughtEvent.Invoke();
        }

        public void UpdateCardLevel(string cardId, int cardLevel, bool autosave = true)
        {
            CardData cardData = GetCardData(cardId);
            if (cardData == null)
                return;

            cardData.SetLevel(cardLevel);
            TryToSave(autosave);
        }

        public CardData GetCardData(string cardId)
        {
            return _cardsData.Cards.FirstOrDefault(card => card.CardId.Equals(cardId));
        }

        public bool WasCardBought(string cardId)
        {
            return GetCardData(cardId) != null;
        }

        public void AddComboCard(string cardId, bool autosave = true)
        {
            if (WasComboCardBought(cardId))
                return;

            _cardsData.ComboCardsId.Add(cardId);
            TryToSave(autosave);
            _upgradeEventProvider.ComboCardBoughtEvent.Invoke();
        }

        public void SetComboClaimed(bool autosave = true)
        {
            _cardsData.ComboClaimed = true;
            TryToSave(autosave);
        }

        public bool WasComboCardBought(string cardId) =>
            _cardsData.ComboCardsId.Contains(cardId);

        public void ClearComboCards(bool autosave = true)
        {
            _cardsData.ComboCardsId.Clear();
            _cardsData.ComboClaimed = false;
            TryToSave(autosave);
        }

        public void SetMoneyPerHour(long money, bool autosave = true)
        {
            if (money < 0)
                money = 0;

            _cardsData.MoneyPerHour = money;
            TryToSave(autosave);
        }

        public void SaveData() => TryToSave(true);
    }
}