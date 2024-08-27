using Main.Scripts.Configs.Cards;
using Main.Scripts.Core.Constants;
using Main.Scripts.Infrastructure.Providers.Assets;

namespace Main.Scripts.Infrastructure.Providers.Configs
{
    public interface ICardsConfigProvider
    {
        CardsGroupConfig Config { get; }
    }

    public class CardsConfigProvider : ICardsConfigProvider
    {
        public CardsGroupConfig Config { get; }

        public CardsConfigProvider(IAssetProvider assetProvider) =>
            Config = assetProvider.Load<CardsGroupConfig>(ConfigsPaths.CARDS_CONFIG_PATH);
    }
}