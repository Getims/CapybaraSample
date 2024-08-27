using Main.Scripts.Data.Services;
using Main.Scripts.Infrastructure.Providers.Events;
using TMPro;
using UnityEngine;
using Zenject;

namespace Main.Scripts.UI.Common
{
    public class LevelTracker : MonoBehaviour
    {
        [SerializeField]
        private TextMeshProUGUI _valueTMP;

        private GlobalEventProvider _globalEventProvider;
        private IPlayerDataService _playerDataService;

        [Inject]
        public void Construct(IPlayerDataService playerDataService, GlobalEventProvider globalEventProvider)
        {
            _playerDataService = playerDataService;
            _globalEventProvider = globalEventProvider;
            _globalEventProvider.AccountLevelSwitchEvent.AddListener(UpdateInfo);
        }

        public void UpdateInfo()
        {
            if (_playerDataService == null)
                return;

            UpdateInfo(_playerDataService.AccountLevel);
        }

        private void Start() =>
            UpdateInfo();

        private void OnDestroy() =>
            _globalEventProvider?.AccountLevelSwitchEvent.RemoveListener(UpdateInfo);

        private void UpdateInfo(int levelNumber) =>
            _valueTMP.text = $"Level {levelNumber + 1}";
    }
}