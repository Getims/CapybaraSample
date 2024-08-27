using Main.Scripts.Configs.Core;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Main.Scripts.Configs.TapGame
{
    public class TapGameConfig : ScriptableConfig
    {
        [Title("Money config")]
        [SerializeField, InlineProperty, HideLabel]
        private MoneyConfig _moneyConfig;

        [Title("Energy config")]
        [SerializeField, InlineProperty, HideLabel]
        private EnergyConfig _energyConfig;

        [Title("Turbo config")]
        [SerializeField, InlineProperty, HideLabel]
        private TurboConfig _turboConfig;

        public MoneyConfig MoneyConfig => _moneyConfig;
        public EnergyConfig EnergyConfig => _energyConfig;
        public TurboConfig TurboConfig => _turboConfig;

#if UNITY_EDITOR
        public override string GetConfigCategory() =>
            ConfigsCategories.TAP_GAME_CATEGORY;
#endif
    }
}