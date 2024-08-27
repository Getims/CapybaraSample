using System;

namespace Main.Scripts.GameLogic.Timers
{
    public class VirtualTimerEndless : VirtualTimer
    {
        private readonly float _tickInterval;
        private float _tickElapsedTime;

        public VirtualTimerEndless(Action<float> onTick, float tickInterval = 0f) : base(0, onTick, null)
        {
            _elapsedTime = 0f;
            _onTick = onTick;

            _isRunning = true;
            _tickElapsedTime = 0f;

            _tickInterval = tickInterval;
            if (_tickInterval < 0)
                _tickInterval = 0;
        }

        public VirtualTimerEndless(Action onTick, float tickInterval = 0f) : base(0, onTick, null)
        {
            _elapsedTime = 0f;
            _onTick = (tick) => onTick?.Invoke();

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

            _tickElapsedTime += deltaTime;

            if (_tickElapsedTime >= _tickInterval)
            {
                _onTick?.Invoke(_tickElapsedTime);
                _tickElapsedTime -= _tickInterval;
            }
        }
    }
}