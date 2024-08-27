using System;
using Main.Scripts.Configs.TapGame;
using Main.Scripts.Core.Utilities;
using Main.Scripts.Data.Services;
using Main.Scripts.GameLogic.External;
using Main.Scripts.GameLogic.Timers;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Main.Scripts.UI.TapGame.Core
{
    [Serializable]
    public class TurboLogic
    {
        [SerializeField]
        private TurboButton _turboButton;

        [SerializeField]
        private Vector2 _turboPositionRadius;

        private ITapGameDataService _tapGameDataService;
        private IRemoteDataService _remoteDataService;
        private ITimersController _timersController;
        private TurboConfig _turboConfig;

        private Action<bool> _setTurboState;
        private int _turboUsedCount;
        private int _nextUseTime;
        private bool _isWorking = false;
        private VirtualTimer _showButtonTimer;
        private VirtualTimer _workTimer;
        private Vector3 _turboButtonStartPosition;

        public void Initialize(TurboConfig turboConfig, IRemoteDataService remoteDataService,
            ITapGameDataService tapGameDataService, Action<bool> setTurboState, ITimersController timersController)
        {
            _timersController = timersController;
            _remoteDataService = remoteDataService;
            _tapGameDataService = tapGameDataService;
            _setTurboState = setTurboState;
            _turboConfig = turboConfig;
            _turboUsedCount = _tapGameDataService.TurboUsedCount;
            _nextUseTime = CalculateTime(tapGameDataService.LastTurboUsedTime);

            _turboButtonStartPosition = _turboButton.transform.localPosition;
            _turboButton.OnClick -= OnTurboButtonClick;
            _turboButton.OnClick += OnTurboButtonClick;
        }

        public void TryToSpawnTurbo()
        {
            if (!CanSpawnTurbo())
                return;

            ShowButton();
            _isWorking = true;
            _showButtonTimer = new VirtualTimer(_turboConfig.TurboOnScreenTime, () => { }, HideButton);
            _timersController.AddTimer(_showButtonTimer);
        }

        private void ShowButton()
        {
            Vector2 radius = Random.insideUnitCircle;
            Vector2 offset = _turboPositionRadius * radius;
            _turboButton.transform.localPosition = _turboButtonStartPosition + (Vector3) offset;
            _turboButton.Show();
        }

        private void HideButton()
        {
            _turboButton.Hide(true);
            _nextUseTime = CalculateTime(UnixTime.Now);
            _tapGameDataService.SetTurboUsedTime(_remoteDataService.GetTimeNow());
            _isWorking = false;
        }

        private bool CanSpawnTurbo()
        {
            if (!_turboConfig.TurboEnabled)
                return false;

            if (_isWorking)
                return false;

            if (_turboUsedCount >= _turboConfig.TurboCountPerDay + _turboConfig.TurboCountPerAccountLevel)
            {
                //Utils.Log(this, "Turbo blocked by use count");
                return false;
            }

            if (UnixTime.Now < _nextUseTime)
            {
                //Utils.Log(this, $"Turbo blocked by time:{_nextUseTime - UnixTime.Now}");
                return false;
            }

            if (!Utils.RandomChance(_turboConfig.TurboSpawnChance))
            {
                //Utils.Log(this, "Turbo blocked by chance");
                return false;
            }

            return true;
        }

        private void OnTurboButtonClick()
        {
            _showButtonTimer.Stop();
            _workTimer = new VirtualTimer(_turboConfig.TurboWorkTime, () => { }, DisableTurbo);
            _timersController.AddTimer(_workTimer);

            _turboUsedCount++;
            _nextUseTime = CalculateTime(UnixTime.Now);

            _tapGameDataService.SetTurboUsedCount(_turboUsedCount);
            _tapGameDataService.SetTurboUsedTime(_remoteDataService.GetTimeNow());
            _setTurboState?.Invoke(true);
        }

        private void DisableTurbo()
        {
            _isWorking = false;
            _workTimer.Stop();
            _setTurboState?.Invoke(false);
        }

        private int CalculateTime(int now)
        {
            int delay = Random.Range(_turboConfig.TimeBetweenTurboSpawn.x, _turboConfig.TimeBetweenTurboSpawn.y);
            return now + delay + _turboConfig.TurboWorkTime;
        }
    }
}