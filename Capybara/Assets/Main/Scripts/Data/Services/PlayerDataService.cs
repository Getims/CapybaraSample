using System;
using Main.Scripts.Data.Core;
using Main.Scripts.Data.DataObjects;
using Main.Scripts.Infrastructure.Providers.Events;

namespace Main.Scripts.Data.Services
{
    public interface IPlayerDataService
    {
        bool IsSoundOn { get; }
        bool IsMusicOn { get; }
        long Money { get; }
        string StockId { get; }
        int LastGameTime { get; }
        int AccountLevel { get; }

        void SetSoundState(bool isSoundOn, bool autosave = true);
        void SetMusicState(bool isMusicOn, bool autosave = true);

        void AddMoney(long amount, bool sendEvent = true, bool autosave = true);
        void SetMoney(long amount, bool autosave = true);
        void SpendMoney(long amount, bool autosave = true);

        void SetStock(string stockId, bool autosave = true);
        void SaveTime(int time, bool autosave = true);
        void SetAccountLevel(int levelId, bool autosave = true);
    }

    public class PlayerDataService : DataService, IPlayerDataService
    {
        private readonly PlayerData _playerData;
        private readonly GlobalEventProvider _globalEventProvider;

        public bool IsSoundOn => _playerData.IsSoundOn;
        public bool IsMusicOn => _playerData.IsMusicOn;
        public long Money => _playerData.Money;
        public string StockId => _playerData.StockId;
        public int LastGameTime => _playerData.LastGameTime;
        public int AccountLevel => _playerData.AccountLevel;

        public PlayerDataService(IDatabase database, GlobalEventProvider globalEventProvider) : base(database)
        {
            _globalEventProvider = globalEventProvider;
            _playerData = database.GetData<PlayerData>();
        }

        public void SetSoundState(bool isSoundOn, bool autosave = true)
        {
            if (_playerData.IsSoundOn == isSoundOn)
                return;

            _playerData.IsSoundOn = isSoundOn;
            TryToSave(autosave);
        }

        public void SetMusicState(bool isMusicOn, bool autosave = true)
        {
            if (_playerData.IsMusicOn == isMusicOn)
                return;

            _playerData.IsMusicOn = isMusicOn;
            TryToSave(autosave);
        }

        public void AddMoney(long amount, bool sendEvent = true, bool autosave = true)
        {
            if (amount <= 0)
                return;

            _playerData.Money += amount;
            TryToSave(autosave);

            if (sendEvent)
                _globalEventProvider.MoneyChangedEvent.Invoke(_playerData.Money);
        }

        public void SetMoney(long amount, bool autosave = true)
        {
            if (amount == _playerData.Money)
                return;

            _playerData.Money = Math.Max(amount, 0);
            TryToSave(autosave);

            _globalEventProvider.MoneyChangedEvent.Invoke(_playerData.Money);
        }

        public void SpendMoney(long amount, bool autosave = true)
        {
            if (amount <= 0)
                return;

            _playerData.Money = Math.Max(_playerData.Money - amount, 0);
            TryToSave(autosave);

            _globalEventProvider.MoneyChangedEvent.Invoke(_playerData.Money);
        }

        public void SetStock(string stockId, bool autosave = true)
        {
            _playerData.StockId = stockId;
            TryToSave(autosave);
            _globalEventProvider.StockChangedEvent.Invoke(stockId);
        }

        public void SetAccountLevel(int levelId, bool autosave = true)
        {
            if (levelId < 0)
                levelId = 0;

            _playerData.AccountLevel = levelId;
            _globalEventProvider.AccountLevelSwitchEvent.Invoke(levelId);
            TryToSave(autosave);
        }

        public void SaveTime(int time, bool autosave = true)
        {
            _playerData.LastGameTime = time;
            TryToSave(autosave);
        }
    }
}