using System;
using System.Collections.Generic;
using Main.Scripts.UI.Base;

namespace Main.Scripts.UI.Common
{
    public abstract class MenuPanel : UIPanel
    {
        protected IUIMenuFactory _uiMenuFactory;
        protected UIContainerProvider _uiContainerProvider;
        protected List<UIPanel> _popups = new List<UIPanel>();

        public virtual void Initialize(IUIMenuFactory uiMenuFactory, UIContainerProvider containerProvider)
        {
            _uiContainerProvider = containerProvider;
            _uiMenuFactory = uiMenuFactory;
        }

        protected void CloseAllPopups()
        {
            foreach (UIPanel popup in _popups)
            {
                if (popup != null)
                    popup.Hide();
            }

            _popups.Clear();
        }

        protected T OpenPopup<T>(Action closeClick, Action claimClick) where T : PopupPanel
        {
            T popup = _uiMenuFactory.Create<T>(_uiContainerProvider.WindowsContainer);
            _popups.Add(popup);
            popup.Initialize();
            popup.Show();
            if (closeClick != null)
                popup.OnCloseClick += closeClick;
            if (claimClick != null)
                popup.OnClaimClick += claimClick;

            return popup;
        }

        protected T OpenWindow<T>() where T : UIPanel
        {
            T window = _uiMenuFactory.Create<T>(_uiContainerProvider.MenuContainer);
            _popups.Add(window);
            window.Initialize();
            window.Show();

            return window;
        }
    }
}