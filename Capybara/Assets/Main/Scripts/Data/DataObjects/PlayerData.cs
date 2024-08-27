using System;
using Main.Scripts.Core.Utilities;
using Main.Scripts.Data.Core;
using Sirenix.OdinInspector;

namespace Main.Scripts.Data.DataObjects
{
    [Serializable]
    public class PlayerData : GameData
    {
        public int LastGameTime = 0;
        public bool IsSoundOn = true;
        public bool IsMusicOn = true;
        public long Money;
        public string StockId = string.Empty;
        public int AccountLevel = 0;

#if UNITY_EDITOR
        [Title("Utils")]
        [Button]
        private void RewindTimeByDay() =>
            LastGameTime = LastGameTime - UnixTime.HOURS_AT_DAY * UnixTime.SECONDS_IN_HOUR;

        [Button]
        private void RewindTimeByHour() =>
            LastGameTime = LastGameTime - UnixTime.SECONDS_IN_HOUR;

        [Button]
        private void SetTimeNow() =>
            LastGameTime = UnixTime.Now;
#endif
    }
}