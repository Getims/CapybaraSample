using DG.Tweening;
using Main.Scripts.Data.Services;
using Main.Scripts.GameLogic;
using Main.Scripts.Infrastructure.Providers.Events;
using TMPro;
using UnityEngine;
using Zenject;

namespace Main.Scripts.UI.Common
{
    public class MoneyCounter : MonoBehaviour
    {
        [SerializeField]
        private Transform _moneyIcon;

        [SerializeField]
        private TextMeshProUGUI _valueTMP;

        [SerializeField]
        private float _updateDuration = 0.2f;

        private IPlayerDataService _playerDataService;
        private GlobalEventProvider _globalEventProvider;
        private long _currentValue = -1;
        private Tweener _updateTW;

        public Vector3 IconPosition => _moneyIcon.position;

        [Inject]
        public void Construct(IPlayerDataService playerDataService, GlobalEventProvider globalEventProvider)
        {
            _globalEventProvider = globalEventProvider;
            _playerDataService = playerDataService;
            _globalEventProvider.MoneyChangedEvent.AddListener(UpdateInfo);
        }

        public void UpdateInfo()
        {
            if (_playerDataService == null)
                return;

            UpdateInfo(_playerDataService.Money);
        }

        private void Start() =>
            UpdateInfo();

        private void OnDestroy() =>
            _globalEventProvider?.MoneyChangedEvent.RemoveListener(UpdateInfo);

        private void UpdateInfo(long moneyCount)
        {
            _updateTW?.Kill();
            if (_updateDuration <= 0.01 || _currentValue < 0)
            {
                _valueTMP.text = moneyCount.ToString();
                _currentValue = moneyCount;
                return;
            }

            long startValue = _currentValue;
            long difference = moneyCount - _currentValue;
            float lerp = 0;
            _updateTW = DOTween.To(() => lerp, x => lerp = x, 1, _updateDuration)
                .OnUpdate(() =>
                {
                    long lerpValue = (long) (difference * lerp);
                    _currentValue = startValue + lerpValue;
                    if (_currentValue > moneyCount)
                        _currentValue = moneyCount;

                    _valueTMP.text = MoneyConverter.ConvertToSpaceValue(_currentValue);
                })
                .SetEase(Ease.Linear)
                .SetUpdate(UpdateType.Fixed)
                .OnComplete(() =>
                {
                    _currentValue = moneyCount;
                    _valueTMP.text = MoneyConverter.ConvertToSpaceValue(_currentValue);
                });
        }
    }
}