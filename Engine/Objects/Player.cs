using System.Collections.Generic;

namespace Engine.Objects
{
    public class Player
    {
        public string Id { get; internal set; }
        
        public int Stack { get; internal set; }
        
        public int CurrentBet { get; internal set; }
        
        public IReadOnlyCollection<Card> Cards { get; internal set; }
        
        public bool HasHoleCards { get; internal set; }
    }
}