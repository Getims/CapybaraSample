using Main.Scripts.Configs.Accounts;
using Main.Scripts.Core.Constants;
using Main.Scripts.Infrastructure.Providers.Assets;

namespace Main.Scripts.Infrastructure.Providers.Configs
{
    public interface IAccountConfigProvider
    {
        AccountConfig Config { get; }
    }

    public class AccountConfigProvider : IAccountConfigProvider
    {
        public AccountConfig Config { get; }

        public AccountConfigProvider(IAssetProvider assetProvider) =>
            Config = assetProvider.Load<AccountConfig>(ConfigsPaths.ACCOUNT_CONFIG_PATH);
    }
}