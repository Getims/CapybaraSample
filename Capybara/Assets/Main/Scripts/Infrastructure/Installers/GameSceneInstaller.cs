using Main.Scripts.GameLogic;
using Main.Scripts.GameLogic.Cards;
using Main.Scripts.GameLogic.Timers;
using Main.Scripts.Infrastructure.Bootstrap;
using Main.Scripts.Infrastructure.Providers.Configs;
using Main.Scripts.UI;
using UnityEngine;
using Zenject;

namespace Main.Scripts.Infrastructure.Installers
{
    public class GameSceneInstaller : MonoInstaller
    {
        [SerializeField]
        private UIController _uiController;

        [SerializeField]
        private TimersController _timersController;

        public override void InstallBindings()
        {
            BindConfigProviders();
            BindSceneObjects();
            BindGameControllers();

            CreateSceneBootstrapper();
        }

        private void BindGameControllers()
        {
            Container.BindInterfacesTo<MiningController>().AsSingle().NonLazy();
            Container.BindInterfacesTo<EnergyController>().AsSingle().NonLazy();
            Container.BindInterfacesTo<CardsConditionChecker>().AsSingle().NonLazy();
        }

        private void BindConfigProviders()
        {
            Container.BindInterfacesTo<AccountConfigProvider>().AsSingle().NonLazy();
            Container.BindInterfacesTo<UIConfigProvider>().AsSingle().NonLazy();
            Container.BindInterfacesTo<TapGameConfigProvider>().AsSingle().NonLazy();
            Container.BindInterfacesTo<TasksConfigProvider>().AsSingle().NonLazy();
            Container.BindInterfacesTo<FriendsConfigProvider>().AsSingle().NonLazy();
            Container.BindInterfacesTo<CardsConfigProvider>().AsSingle().NonLazy();
        }

        private void BindSceneObjects()
        {
            Container.BindInstance(_uiController).AsSingle().NonLazy();
            Container.Bind<ITimersController>().FromInstance(_timersController).AsSingle().NonLazy();
        }

        private void CreateSceneBootstrapper()
        {
            GameSceneBootstrapper bootstrapper = Container.Instantiate<GameSceneBootstrapper>();
            bootstrapper.Initialize();
        }
    }
}