using System;

namespace Main.Scripts.GameLogic.Timers
{
    public class VirtualTimerWithInterval : VirtualTimer
    {
        private float _tickInterval;
        private float _tickElapsedTime;

        public VirtualTimerWithInterval(float duration, Action onTick, Action onComplete,
            float tickInterval = 0f) : base(duration, onTick, onComplete)
        {
            _duration = duration;
            _elapsedTime = 0f;
            _onTick = (time) => onTick?.Invoke();

            _onComplete = onComplete;
            _isRunning = true;
            _tickElapsedTime = 0f;

            _tickInterval = tickInterval;
            if (_tickInterval < 0)
                _tickInterval = 0;
        }

        public VirtualTimerWithInterval(float duration, Action<float> onTick, Action onComplete,
            float tickInterval = 0f) : base(duration, onTick, onComplete)
        {
            _duration = duration;
            _elapsedTime = 0f;
            _onTick = onTick;

            _onComplete = onComplete;
            _isRunning = true;
            _tickElapsedTime = 0f;

            _tickInterval = tickInterval;
            if (_tickInterval < 0)
                _tickInterval = 0;
        }

        public override void Update(float deltaTime)
        {
            if (!_isRunning)
                return;

            _elapsedTime += deltaTime;
            _tickElapsedTime += deltaTime;

            if (_tickElapsedTime >= _tickInterval)
            {
                _onTick?.Invoke(_tickInterval);
                _tickElapsedTime -= _tickInterval;
            }

            if (_elapsedTime >= _duration)
            {
                _onComplete?.Invoke();
                Stop();
            }
        }
    }
}