using Engine.Enums;

namespace Engine.Objects
{
    public class HistoryRecord
    {
        public int Round { get; private set; }
        
        public Street Street { get; private set; }
        
        public int MainPot { get; private set; }
        
        public PartialPot[] PartialPots { get; private set; }
        
        public Card[] Board { get; private set; }
        
        public PlayerAction PlayerAction { get; private set; }
        
        public PlayersChain Players { get; private set; }
    }
}