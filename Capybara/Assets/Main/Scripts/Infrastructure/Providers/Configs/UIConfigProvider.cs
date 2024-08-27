using System.Linq;
using Main.Scripts.Configs.UI;
using Main.Scripts.Core.Constants;
using Main.Scripts.Core.Enums;
using Main.Scripts.Infrastructure.Providers.Assets;
using UnityEngine;

namespace Main.Scripts.Infrastructure.Providers.Configs
{
    public interface IUIConfigProvider
    {
        UIConfig GetConfig(GameState gameState);
    }

    public class UIConfigProvider : IUIConfigProvider
    {
        private readonly IAssetProvider assetProvider;

        private UIConfigsReferences _configsReferences;

        public UIConfigProvider(IAssetProvider assetProvider)
        {
            this.assetProvider = assetProvider;
            _configsReferences = assetProvider.Load<UIConfigsReferences>(ConfigsPaths.UI_CONFIG_PATH);
        }

        public UIConfig GetConfig(GameState gameState)
        {
            ConfigReference configReference =
                _configsReferences.References.FirstOrDefault(cr => cr.GameState == gameState);
            if (configReference == null)
            {
                Debug.LogError("No reference for " + gameState);
                return null;
            }

            return assetProvider.Load<UIConfig>(configReference.ConfigPath);
        }
    }
}