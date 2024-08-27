using Main.Scripts.Configs.UI;
using Main.Scripts.Core.Enums;
using Main.Scripts.GameLogic.Sound;
using Main.Scripts.Infrastructure.Providers.Configs;
using Main.Scripts.UI.Base;
using Main.Scripts.UI.CardsMenu;
using Main.Scripts.UI.Controls;
using Main.Scripts.UI.FriendsMenu;
using Main.Scripts.UI.MainMenu;
using Main.Scripts.UI.TasksMenu;
using UnityEngine;
using Zenject;

namespace Main.Scripts.UI
{
    public class UIController : MonoBehaviour
    {
        [SerializeField]
        private UIContainerProvider _uiContainerProvider;

        [SerializeField]
        private ControlPanel _controlPanel;

        [SerializeField]
        private MainMenuPanel _mainMenuPanel;

        [SerializeField]
        private TasksMenuPanel _tasksMenuPanel;

        [SerializeField]
        private FriendsMenuPanel _friendsMenuPanel;

        [SerializeField]
        private CardsMenuPanel _cardsMenuPanel;

        private DiContainer _container;
        private ISoundService _soundService;
        private UIMenuFactory _uiMenuFactory;
        private IUIConfigProvider _uiConfigProvider;
        private UIPanel _currentPanel;

        [Inject]
        public void Construct(DiContainer container, ISoundService soundService, IUIConfigProvider uiConfigProvider)
        {
            _uiConfigProvider = uiConfigProvider;
            _soundService = soundService;
            _container = container;
        }

        private void Start()
        {
            Initialize();
        }

        private void Initialize()
        {
            _soundService.PlayGameBackgroundMusic();
            _controlPanel.Initialize();
            _controlPanel.OnControlButtonClick += OnControlButtonClick;

            SetupMenuFactory(GameState.MainMenu);
            ShowMainMenu();
        }

        private void ShowMainMenu()
        {
            _mainMenuPanel.Initialize(_uiMenuFactory, _uiContainerProvider);
            _mainMenuPanel.Show();
            _currentPanel = _mainMenuPanel;
        }

        private void ShowMiningPanel()
        {
            _cardsMenuPanel.Initialize(_uiMenuFactory, _uiContainerProvider);
            _cardsMenuPanel.Show();
            _currentPanel = _cardsMenuPanel;
        }

        private void ShowAirDropPanel()
        {
            //Add new panel if need
        }

        private void ShowTasksPanel()
        {
            _tasksMenuPanel.Initialize(_uiMenuFactory, _uiContainerProvider);
            _tasksMenuPanel.Show();
            _currentPanel = _tasksMenuPanel;
        }

        private void ShowFriendsPanel()
        {
            _friendsMenuPanel.Initialize(_uiMenuFactory, _uiContainerProvider);
            _friendsMenuPanel.Show();
            _currentPanel = _friendsMenuPanel;
        }

        private void SetupMenuFactory(GameState gameState)
        {
            UIConfig uiConfig = _uiConfigProvider.GetConfig(gameState);
            _uiMenuFactory = new UIMenuFactory(_container, uiConfig, _uiContainerProvider.MenuContainer);
        }

        private void OnControlButtonClick(GameState gameState)
        {
            _currentPanel.Hide();
            SetupMenuFactory(gameState);

            switch (gameState)
            {
                case GameState.MainMenu:
                    ShowMainMenu();
                    break;
                case GameState.Mining:
                    ShowMiningPanel();
                    break;
                case GameState.Friends:
                    ShowFriendsPanel();
                    break;
                case GameState.Tasks:
                    ShowTasksPanel();
                    break;
                case GameState.AirDrop:
                    ShowAirDropPanel();
                    break;
            }

            Resources.UnloadUnusedAssets();
        }
    }
}