using System;

namespace Main.Scripts.GameLogic.External
{
    public static class AdsManager
    {
        private static Action<bool> _onRewarded;

        public static void ShowRewarded(Action<bool> onRewarded)
        {
            _onRewarded = onRewarded;
            OnRewarded(true);
        }

        public static bool CanShowRewarded() => true;

        private static void OnRewarded(bool giveReward) =>
            _onRewarded?.Invoke(giveReward);
    }
}