using Main.Scripts.Configs.Friends;
using Main.Scripts.Core.Constants;
using Main.Scripts.Infrastructure.Providers.Assets;

namespace Main.Scripts.Infrastructure.Providers.Configs
{
    public interface IFriendsConfigProvider
    {
        FriendsConfig Config { get; }
    }

    public class FriendsConfigProvider : IFriendsConfigProvider
    {
        public FriendsConfig Config { get; }

        public FriendsConfigProvider(IAssetProvider assetProvider) =>
            Config = assetProvider.Load<FriendsConfig>(ConfigsPaths.FRIENDS_CONFIG_PATH);
    }
}