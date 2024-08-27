using System.Collections.Generic;
using Main.Scripts.Core.Constants;
using UnityEngine;

namespace Main.Scripts.GameLogic.Timers
{
    public interface ITimersController
    {
        void AddTimer(VirtualTimer virtualTimer);
        void RemoveTimer(VirtualTimer virtualTimer);
    }

    public class TimersController : MonoBehaviour, ITimersController
    {
        private readonly List<VirtualTimer> _virtualTimers = new List<VirtualTimer>();
        private float _elapsedTime = 0f;

        public void AddTimer(VirtualTimer virtualTimer)
        {
            _virtualTimers.Add(virtualTimer);
        }

        public void RemoveTimer(VirtualTimer virtualTimer)
        {
            virtualTimer.Stop();
            //_virtualTimers.Remove(virtualTimer);
        }

        private void Update()
        {
            _elapsedTime += Time.deltaTime;
            if (_elapsedTime < TimersTick.BASE_TICK_INTERVAL)
                return;

            int timersCount = _virtualTimers.Count;
            for (int i = 0; i < timersCount; i++)
            {
                if (_virtualTimers[i] != null)
                    _virtualTimers[i].Update(_elapsedTime);
            }

            _elapsedTime = 0;
            _virtualTimers.RemoveAll(timer => timer == null || !timer.IsRunning);
        }
    }
}