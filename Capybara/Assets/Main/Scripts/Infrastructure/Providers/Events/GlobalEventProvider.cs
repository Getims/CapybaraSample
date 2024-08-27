using Main.Scripts.Core.Enums;

namespace Main.Scripts.Infrastructure.Providers.Events
{
    public class GlobalEventProvider : GameEventProvider
    {
        public GameLoadCompleteEvent GameLoadCompleteEvent { get; } = new GameLoadCompleteEvent();
        public SoundSwitchEvent SoundSwitchEvent { get; } = new SoundSwitchEvent();
        public MusicSwitchEvent MusicSwitchEvent { get; } = new MusicSwitchEvent();
        public MoneyChangedEvent MoneyChangedEvent { get; } = new MoneyChangedEvent();
        public EnergyChangedEvent EnergyChangedEvent { get; } = new EnergyChangedEvent();
        public AccountLevelSwitchEvent AccountLevelSwitchEvent { get; } = new AccountLevelSwitchEvent();
        public StockChangedEvent StockChangedEvent { get; } = new StockChangedEvent();
        public TryToSwitchGameState TryToSwitchGameState { get; } = new TryToSwitchGameState();
    }

    public class GameLoadCompleteEvent : GameEvent
    {
    }

    public class SoundSwitchEvent : GameEvent<bool>
    {
    }

    public class MusicSwitchEvent : GameEvent<bool>
    {
    }

    public class MoneyChangedEvent : GameEvent<long>
    {
    }

    public class EnergyChangedEvent : GameEvent<int>
    {
    }

    public class AccountLevelSwitchEvent : GameEvent<int>
    {
    }

    public class StockChangedEvent : GameEvent<string>
    {
    }

    public class TryToSwitchGameState : GameEvent<GameState>
    {
    }
}