using System.Collections.Generic;

namespace Engine.Objects
{
    internal class Player
    {
        internal string Id { get; set; }
        
        internal List<Card> Cards { get; } = new List<Card>();
    }
}