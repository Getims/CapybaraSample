using Main.Scripts.Configs.Cards;
using Main.Scripts.Core.Enums;
using Main.Scripts.Data.DataObjects;
using Main.Scripts.Data.Services;

namespace Main.Scripts.GameLogic.Cards
{
    public interface ICardsConditionChecker
    {
        bool CanBuyCard(CardConfig cardConfig);
    }

    public class CardsConditionChecker : ICardsConditionChecker
    {
        private ICardsDataService _cardsDataService;

        CardsConditionChecker(ICardsDataService cardsDataService)
        {
            _cardsDataService = cardsDataService;
        }

        public bool CanBuyCard(CardConfig cardConfig)
        {
            switch (cardConfig.BuyCondition)
            {
                case CardBuyCondition.NoCondition:
                    return true;
                case CardBuyCondition.UpgradeCard:
                    return CheckCardUpgrade(cardConfig.UpgradeCardCondition);
            }

            return true;
        }

        private bool CheckCardUpgrade(UpgradeCardCondition upgradeCardCondition)
        {
            CardData cardData = _cardsDataService.GetCardData(upgradeCardCondition.CardToUpgradeId);
            if (cardData == null)
                return false;

            if (cardData.CardLevel >= upgradeCardCondition.TargetLevel)
                return true;

            return false;
        }
    }
}