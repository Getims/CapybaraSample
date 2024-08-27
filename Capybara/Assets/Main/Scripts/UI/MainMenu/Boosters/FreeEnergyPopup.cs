using Main.Scripts.GameLogic;
using Main.Scripts.UI.Common;
using Zenject;

namespace Main.Scripts.UI.MainMenu.Boosters
{
    public class FreeEnergyPopup : PopupPanel
    {
        private IEnergyController _energyController;

        [Inject]
        public void Construct(IEnergyController energyController)
        {
            _energyController = energyController;
        }

        protected override void OnClaimButtonClick()
        {
            _energyController.FullEnergyRecovery();
            base.OnClaimButtonClick();
        }
    }
}