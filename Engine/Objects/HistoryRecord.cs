using System.Collections.Generic;
using Engine.Enums;

namespace Engine.Objects
{
    public class HistoryRecord
    {
        public int Round { get; internal set; }
        
        public Street Street { get; internal set; }
        
        public IReadOnlyCollection<PartialPot> Pots { get; internal set; }
        
        public IReadOnlyList<Card> Board { get; internal set; }
        
        public PlayerAction PlayerAction { get; internal set; }
        
        public PlayersChain Players { get; internal set; }
    }
}