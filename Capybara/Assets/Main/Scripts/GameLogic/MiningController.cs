using System.Collections.Generic;
using Main.Scripts.Configs.Cards;
using Main.Scripts.Configs.Global;
using Main.Scripts.Core.Utilities;
using Main.Scripts.Data.DataObjects;
using Main.Scripts.Data.Services;
using Main.Scripts.GameLogic.External;
using Main.Scripts.Infrastructure.Providers.Configs;
using Main.Scripts.Infrastructure.Providers.Events;

namespace Main.Scripts.GameLogic
{
    public interface IMiningController
    {
        long CalculateMoneyFromLastPlay();
        void UpdateMoneyPerHour();
        long MoneyFromLastPlay { get; }
        IReadOnlyCollection<CardConfig> CardsCombo { get; }
        long MoneyPerSecond { get; }
        long MoneyPerHour { get; }
        void UpdateCardsCombo();
        void AddMoneyFromMining(float seconds);
    }

    public class MiningController : IMiningController
    {
        private readonly GlobalConfig _globalConfig;
        private readonly IPlayerDataService _playerDataService;
        private readonly ICardsDataService _cardsDataService;
        private readonly IRemoteDataService _remoteDataService;
        private readonly CardsGroupConfig _cardsConfig;
        private readonly UpgradeEventProvider _upgradeEventProvider;

        private long _moneyPerSecond;
        private long _moneyPerHour;
        private long _moneyFromLastPlay;
        private IReadOnlyCollection<CardConfig> _cardsCombo;

        public long MoneyPerSecond => _moneyPerSecond;
        public long MoneyPerHour => _moneyPerHour;
        public long MoneyFromLastPlay => _moneyFromLastPlay;
        public IReadOnlyCollection<CardConfig> CardsCombo => _cardsCombo;

        public MiningController(IGlobalConfigProvider globalConfigProvider, IPlayerDataService playerDataService,
            ICardsDataService cardsDataService, ICardsConfigProvider cardsConfigProvider,
            IRemoteDataService remoteDataService, UpgradeEventProvider upgradeEventProvider)
        {
            _upgradeEventProvider = upgradeEventProvider;
            _remoteDataService = remoteDataService;
            _globalConfig = globalConfigProvider.Config;
            _playerDataService = playerDataService;
            _cardsDataService = cardsDataService;
            _cardsConfig = cardsConfigProvider.Config;
        }

        public void UpdateMoneyPerHour()
        {
            _moneyPerHour = 0;
            foreach (CardData cardData in _cardsDataService.Cards)
            {
                CardConfig cardConfig = _cardsConfig.GetCardConfig(cardData.CardId);
                _moneyPerHour += cardConfig.GetMiningOnLevel(cardData.CardLevel);
            }

            UpdateMoneyPerSecond();

            _cardsDataService.SetMoneyPerHour(_moneyPerHour);
            _upgradeEventProvider.MiningUpgradeEvent.Invoke();
        }

        public void UpdateCardsCombo()
        {
            _cardsCombo = _remoteDataService.GetCardCombo();
            if (_cardsCombo == null)
                _cardsCombo = _cardsConfig.ComboConfig.TestCombo;
        }

        public long CalculateMoneyFromLastPlay()
        {
            int now = _remoteDataService.GetTimeNow();
            int lastGameTime = _playerDataService.LastGameTime;
            int timePassed = now - lastGameTime;
            if (timePassed <= 0)
                return 0;

            int maxMiningTime = _globalConfig.MaxMiningTime * UnixTime.SECONDS_IN_HOUR;
            if (timePassed >= maxMiningTime)
                timePassed = maxMiningTime;

            float hoursPassed = 1f * timePassed / UnixTime.SECONDS_IN_HOUR;

            _moneyFromLastPlay = (long) (hoursPassed * _moneyPerHour);
            return _moneyFromLastPlay;
        }

        public void AddMoneyFromMining(float seconds)
        {
            long money = (long) (seconds * _moneyPerSecond);
            _playerDataService.AddMoney(money);
        }

        private void UpdateMoneyPerSecond()
        {
            if (_moneyPerHour <= 0)
            {
                _moneyPerSecond = 0;
                return;
            }

            _moneyPerSecond = _moneyPerHour / UnixTime.SECONDS_IN_HOUR;
            if (_moneyPerSecond < 1)
                _moneyPerSecond = 1;
        }
    }
}