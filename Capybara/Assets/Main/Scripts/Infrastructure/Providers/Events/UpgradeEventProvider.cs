namespace Main.Scripts.Infrastructure.Providers.Events
{
    public class UpgradeEventProvider : GameEventProvider
    {
        public TapUpgradeEvent TapUpgradeEvent { get; } = new TapUpgradeEvent();
        public CardBoughtEvent CardBoughtEvent { get; } = new CardBoughtEvent();
        public MiningUpgradeEvent MiningUpgradeEvent { get; } = new MiningUpgradeEvent();
        public ComboCardBoughtEvent ComboCardBoughtEvent { get; } = new ComboCardBoughtEvent();
    }

    public class TapUpgradeEvent : GameEvent
    {
    }

    public class MiningUpgradeEvent : GameEvent
    {
    }

    public class CardBoughtEvent : GameEvent
    {
    }

    public class ComboCardBoughtEvent : GameEvent
    {
    }
}