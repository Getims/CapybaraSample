using System;
using Main.Scripts.Data.Core;

namespace Main.Scripts.Data.DataObjects
{
    [Serializable]
    public class TapGameData : GameData
    {
        public int Energy;
        public int MaxEnergy;
        public int TapUpgradeLevel;
        public int EnergyUpgradeLevel;
        public int FullEnergyRecoveryCount;
        public int LastFullEnergyRecoveryTime;
        public int TurboUsedCount;
        public int LastTurboUsedTime;
    }
}