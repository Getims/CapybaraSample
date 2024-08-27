using Main.Scripts.Configs.TapGame;
using Main.Scripts.Core.Constants;
using Main.Scripts.Core.Utilities;
using Main.Scripts.Data.Services;
using Main.Scripts.GameLogic;
using Main.Scripts.Infrastructure.Providers.Configs;
using Main.Scripts.UI.Common;
using TMPro;
using UnityEngine;
using Zenject;

namespace Main.Scripts.UI.MainMenu.Boosters
{
    public class UpgradeEnergyPopup : PopupPanel
    {
        [SerializeField]
        private TextMeshProUGUI _upgrade;

        [SerializeField]
        private TextMeshProUGUI _cost;

        [SerializeField]
        private TextMeshProUGUI _level;

        [SerializeField]
        private CanvasGroup _claimButtonCG;

        private ITapGameDataService _tapGameDataService;
        private TapGameConfig _tapGameConfig;
        private IPlayerDataService _playerDataService;
        private IEnergyController _energyController;

        private long _upgradeCost = -1;

        [Inject]
        public void Construct(IPlayerDataService playerDataService, ITapGameConfigProvider tapGameConfigProvider,
            ITapGameDataService tapGameDataService, IEnergyController energyController)
        {
            _energyController = energyController;
            _playerDataService = playerDataService;
            _tapGameDataService = tapGameDataService;
            _tapGameConfig = tapGameConfigProvider.Config;
        }

        public override void Initialize()
        {
            base.Initialize();

            Utils.ReworkPoint("Need localization energy");
            _upgrade.text = $"+{_tapGameConfig.EnergyConfig.EnergyPerUpgrade} {Phrases.ENERGY_UPGRADE}";

            int upgradeLevel = _tapGameDataService.EnergyUpgradeLevel;
            _upgradeCost = _tapGameConfig.EnergyConfig.GetUpgradeCost(upgradeLevel);

            _cost.text = MoneyConverter.ConvertToSpaceValue(_upgradeCost);
            _level.text = $" {Phrases.DOT} {upgradeLevel + 1} {Phrases.LEVEL}";

            SetClaimState(_upgradeCost <= _playerDataService.Money);
        }

        protected override void OnClaimButtonClick()
        {
            if (_upgradeCost >= 0 && _upgradeCost <= _playerDataService.Money)
            {
                _playerDataService.SpendMoney(_upgradeCost);

                int energyUpgradeLevel = _tapGameDataService.EnergyUpgradeLevel + 1;
                _tapGameDataService.SetEnergyUpgradeLevel(energyUpgradeLevel);

                _energyController.CalculateMaxEnergy();
                _energyController.FullEnergyRecovery(false);

                base.OnClaimButtonClick();
            }
            else
                OnCloseButtonClick();
        }

        private void SetClaimState(bool enabled)
        {
            _claimButtonCG.alpha = enabled ? 1 : Numbers.NOT_ACTIVE_BUTTON_ALPHA;
            _claimButtonCG.interactable = enabled;
            _claimButtonCG.blocksRaycasts = enabled;
        }
    }
}