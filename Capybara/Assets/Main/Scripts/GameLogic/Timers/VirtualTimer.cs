using System;

namespace Main.Scripts.GameLogic.Timers
{
    public class VirtualTimer
    {
        protected float _duration;
        protected float _elapsedTime;
        protected Action<float> _onTick;
        protected Action _onComplete;
        protected bool _isRunning;

        public bool IsRunning => _isRunning;

        public VirtualTimer(float duration, Action<float> onTick, Action onComplete)
        {
            _duration = duration;
            _elapsedTime = 0f;
            _onTick = onTick;

            _onComplete = onComplete;
            _isRunning = true;
        }

        public VirtualTimer(float duration, Action onTick, Action onComplete)
        {
            _duration = duration;
            _elapsedTime = 0f;
            _onTick = (time) => onTick?.Invoke();
            _onComplete = onComplete;
            _isRunning = true;
        }

        public void Stop() => _isRunning = false;

        public virtual void Update(float deltaTime)
        {
            if (!_isRunning)
                return;

            _elapsedTime += deltaTime;
            _onTick?.Invoke(_elapsedTime);

            if (_elapsedTime >= _duration)
            {
                _onComplete?.Invoke();
                Stop();
            }
        }
    }
}