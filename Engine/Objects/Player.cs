using System.Collections.Generic;

namespace Engine.Objects
{
    internal class Player
    {
        internal string Id { get; set; }
        
        internal int Stack { get; set; }
        
        internal int CurrentBet { get; set; }
        
        internal IReadOnlyCollection<Card> Cards { get; set; }
    }
}