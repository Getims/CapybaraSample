using Main.Scripts.Configs.TapGame;
using Main.Scripts.Data.Services;
using Main.Scripts.GameLogic.External;
using Main.Scripts.Infrastructure.Providers.Configs;
using UnityEngine;

namespace Main.Scripts.GameLogic
{
    public interface IEnergyController
    {
        void UpdateEnergyFromLastPlay();
        void RecoveryEnergy(float seconds);
        void FullEnergyRecovery(bool useRecoveryBooster = true);
        int CalculateMaxEnergy();
    }

    public class EnergyController : IEnergyController
    {
        private readonly IRemoteDataService _remoteDataService;
        private readonly IPlayerDataService _playerDataService;
        private readonly ITapGameDataService _tapGameDataService;
        private readonly EnergyConfig _energyConfig;

        public EnergyController(ITapGameConfigProvider tapGameConfigProvider, IPlayerDataService playerDataService,
            ITapGameDataService tapGameDataService, IRemoteDataService remoteDataService)
        {
            _energyConfig = tapGameConfigProvider.Config.EnergyConfig;
            _tapGameDataService = tapGameDataService;
            _playerDataService = playerDataService;
            _remoteDataService = remoteDataService;
        }

        public void UpdateEnergyFromLastPlay()
        {
            int timeNow = _remoteDataService.GetTimeNow();
            int lastGameTime = _playerDataService.LastGameTime;
            int timePassed = timeNow - lastGameTime;
            if (timePassed <= 0)
                return;

            CalculateMaxEnergy();
            RecoveryEnergy(timePassed);
        }

        public void RecoveryEnergy(float seconds)
        {
            int energy = Mathf.CeilToInt(seconds *
                                         (_energyConfig.RecoveryPerSecond +
                                          _playerDataService.AccountLevel * _energyConfig.RecoveryPerAccountLevel));
            _tapGameDataService.AddEnergy(energy);
        }

        public void FullEnergyRecovery(bool useRecoveryBooster = true)
        {
            if (useRecoveryBooster)
            {
                int recoveryCount = _tapGameDataService.FullEnergyRecoveryCount + 1;
                _tapGameDataService.SetFullEnergyRecoveryCount(recoveryCount, false);

                int timeNow = _remoteDataService.GetTimeNow();
                _tapGameDataService.SetFullEnergyRecoveryTime(timeNow, false);
            }

            int energy = _tapGameDataService.MaxEnergy;
            _tapGameDataService.SetEnergy(energy);
        }

        public int CalculateMaxEnergy()
        {
            int startEnergy = _energyConfig.StartEnergy;
            int upgradeEnergy = _energyConfig.EnergyPerUpgrade * _tapGameDataService.EnergyUpgradeLevel;
            int accountEnergy = _energyConfig.EnergyPerAccountLevel * _playerDataService.AccountLevel;

            int maxEnergy = startEnergy + upgradeEnergy + accountEnergy;
            _tapGameDataService.SetMaxEnergy(maxEnergy);

            return maxEnergy;
        }
    }
}