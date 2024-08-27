using Main.Scripts.Infrastructure.Providers.Events;
using Main.Scripts.Infrastructure.StateMachines.BaseStates;
using Main.Scripts.Infrastructure.StateMachines.States.Load;

namespace Main.Scripts.Infrastructure.StateMachines.States.Global
{
    public class GameLoaderState : IEnterState
    {
        private readonly IGameStateMachine _stateMachine;
        private readonly GlobalEventProvider _globalEventsProvider;

        public GameLoaderState(IGameStateMachine stateMachine, GlobalEventProvider globalEventsProvider)
        {
            _globalEventsProvider = globalEventsProvider;
            _stateMachine = stateMachine;
            _globalEventsProvider.GameLoadCompleteEvent.AddListener(MoveToNextState);
        }

        public void Enter()
        {
        }

        private void MoveToNextState()
        {
            _globalEventsProvider.GameLoadCompleteEvent.RemoveListener(MoveToNextState);
            _stateMachine.Enter<LoadGameSceneState>();
        }
    }
}