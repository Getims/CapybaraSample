using System;
using Main.Scripts.Data.Core;
using Main.Scripts.Data.DataObjects;
using Main.Scripts.Infrastructure.Providers.Events;

namespace Main.Scripts.Data.Services
{
    public interface ITapGameDataService
    {
        int Energy { get; }
        int MaxEnergy { get; }
        int TapUpgradeLevel { get; }
        int EnergyUpgradeLevel { get; }
        int FullEnergyRecoveryCount { get; }
        int LastFullEnergyRecoveryTime { get; }
        int TurboUsedCount { get; }
        int LastTurboUsedTime { get; }

        void AddEnergy(int amount, bool sendEvent = true, bool autosave = true);
        void SetEnergy(int amount, bool sendEvent = true, bool autosave = true);
        void SetMaxEnergy(int amount, bool autosave = true);
        void SpendEnergy(int amount, bool sendEvent = true, bool autosave = true);

        void SetTapUpgradeLevel(int level, bool sendEvent = true, bool autosave = true);
        void SetEnergyUpgradeLevel(int level, bool autosave = true);
        void SetFullEnergyRecoveryCount(int count, bool autosave = true);
        void SetFullEnergyRecoveryTime(int time, bool autosave = true);
        void SetTurboUsedCount(int count, bool autosave = true);
        void SetTurboUsedTime(int time, bool autosave = true);
    }

    public class TapGameDataService : DataService, ITapGameDataService
    {
        private readonly TapGameData _tapGameData;
        private readonly GlobalEventProvider _globalEventProvider;
        private readonly UpgradeEventProvider _upgradeEventProvider;

        public int Energy => _tapGameData.Energy;
        public int MaxEnergy => _tapGameData.MaxEnergy;
        public int TapUpgradeLevel => _tapGameData.TapUpgradeLevel;
        public int EnergyUpgradeLevel => _tapGameData.EnergyUpgradeLevel;
        public int FullEnergyRecoveryCount => _tapGameData.FullEnergyRecoveryCount;
        public int LastFullEnergyRecoveryTime => _tapGameData.LastFullEnergyRecoveryTime;
        public int TurboUsedCount => _tapGameData.TurboUsedCount;
        public int LastTurboUsedTime => _tapGameData.LastTurboUsedTime;

        protected TapGameDataService(IDatabase database, GlobalEventProvider globalEventProvider,
            UpgradeEventProvider upgradeEventProvider) : base(database)
        {
            _upgradeEventProvider = upgradeEventProvider;
            _globalEventProvider = globalEventProvider;
            _tapGameData = database.GetData<TapGameData>();
        }

        public void AddEnergy(int amount, bool sendEvent = true, bool autosave = true)
        {
            if (amount <= 0)
                return;

            int energy = _tapGameData.Energy + amount;
            int maxEnergy = _tapGameData.MaxEnergy;

            _tapGameData.Energy = Math.Min(energy, maxEnergy);
            TryToSave(autosave);

            if (sendEvent)
                _globalEventProvider.EnergyChangedEvent.Invoke(_tapGameData.Energy);
        }

        public void SetEnergy(int amount, bool sendEvent = true, bool autosave = true)
        {
            if (amount == _tapGameData.Energy)
                return;

            _tapGameData.Energy = Math.Max(amount, 0);
            TryToSave(autosave);

            if (sendEvent)
                _globalEventProvider.EnergyChangedEvent.Invoke(_tapGameData.Energy);
        }

        public void SetMaxEnergy(int amount, bool autosave = true)
        {
            _tapGameData.MaxEnergy = Math.Max(amount, 0);
            TryToSave(autosave);
        }

        public void SpendEnergy(int amount, bool sendEvent = true, bool autosave = true)
        {
            if (amount <= 0)
                return;

            _tapGameData.Energy = Math.Max(_tapGameData.Energy - amount, 0);
            TryToSave(autosave);

            if (sendEvent)
                _globalEventProvider.EnergyChangedEvent.Invoke(_tapGameData.Energy);
        }

        public void SetTapUpgradeLevel(int level, bool sendEvent = true, bool autosave = true)
        {
            if (level < 0)
                level = 0;

            _tapGameData.TapUpgradeLevel = level;
            TryToSave(autosave);

            if (sendEvent)
                _upgradeEventProvider.TapUpgradeEvent.Invoke();
        }

        public void SetEnergyUpgradeLevel(int level, bool autosave = true)
        {
            if (level < 0)
                level = 0;

            _tapGameData.EnergyUpgradeLevel = level;
            TryToSave(autosave);
        }

        public void SetFullEnergyRecoveryCount(int count, bool autosave = true)
        {
            if (count < 0)
                count = 0;

            _tapGameData.FullEnergyRecoveryCount = count;
            TryToSave(autosave);
        }

        public void SetFullEnergyRecoveryTime(int time, bool autosave = true)
        {
            _tapGameData.LastFullEnergyRecoveryTime = time;
            TryToSave(autosave);
        }

        public void SetTurboUsedCount(int count, bool autosave = true)
        {
            if (count < 0)
                count = 0;

            _tapGameData.TurboUsedCount = count;
            TryToSave(autosave);
        }

        public void SetTurboUsedTime(int time, bool autosave = true)
        {
            _tapGameData.LastTurboUsedTime = time;
            TryToSave(autosave);
        }

        public void SaveData() => TryToSave(true);
    }
}