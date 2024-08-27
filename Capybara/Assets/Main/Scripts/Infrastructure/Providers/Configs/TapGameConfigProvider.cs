using Main.Scripts.Configs.TapGame;
using Main.Scripts.Core.Constants;
using Main.Scripts.Infrastructure.Providers.Assets;

namespace Main.Scripts.Infrastructure.Providers.Configs
{
    public interface ITapGameConfigProvider
    {
        TapGameConfig Config { get; }
    }

    public class TapGameConfigProvider : ITapGameConfigProvider
    {
        public TapGameConfig Config { get; }

        public TapGameConfigProvider(IAssetProvider assetProvider) =>
            Config = assetProvider.Load<TapGameConfig>(ConfigsPaths.TAP_GAME_CONFIG_PATH);
    }
}