using System;
using Main.Scripts.Data.Services;
using Main.Scripts.Infrastructure.Providers.Events;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Main.Scripts.UI.TapGame
{
    public class TapInfoPanel : MonoBehaviour
    {
        [SerializeField]
        private TextMeshProUGUI _energyCounter;

        [SerializeField]
        private Button _boosterButton;

        private GlobalEventProvider _globalEventProvider;
        private ITapGameDataService _tapGameDataService;

        public event Action OnBoosterClick;

        [Inject]
        public void Construct(ITapGameDataService tapGameDataService, GlobalEventProvider globalEventProvider)
        {
            _tapGameDataService = tapGameDataService;
            _globalEventProvider = globalEventProvider;
            _globalEventProvider.EnergyChangedEvent.AddListener(UpdateEnergy);
        }

        private void Start()
        {
            _boosterButton.onClick.AddListener(OnBoosterButtonClick);
            SetEnergy(_tapGameDataService.Energy, _tapGameDataService.MaxEnergy);
        }

        protected void OnDestroy()
        {
            _boosterButton.onClick.RemoveListener(OnBoosterButtonClick);
            _globalEventProvider.EnergyChangedEvent.RemoveListener(UpdateEnergy);
        }

        private void OnBoosterButtonClick() =>
            OnBoosterClick?.Invoke();

        private void UpdateEnergy(int currentEnergy) =>
            SetEnergy(currentEnergy, _tapGameDataService.MaxEnergy);

        private void SetEnergy(int current, int max)
        {
            _energyCounter.text = $"{current} / {max}";
        }
    }
}