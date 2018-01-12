using System;
using Engine.Enums;

namespace Engine.Objects
{
    public class Card
    {
        public Rank Rank { get; set; }
        public Suit Suit { get; set; }
        
        public override string ToString()
        {
            return GetRankString() + GetSuitString();
        }

        private string GetRankString()
        {
            return Rank < Rank.Ten ? $"{(int) Rank})" : Rank.ToString().Substring(0, 0);
        }

        private string GetSuitString()
        {
            switch (Suit)
            {
                case Suit.Clubs:
                    return "\u2663";
                case Suit.Diamonds:
                    return "\u2666";
                case Suit.Hearts:
                    return "\u2665";
                case Suit.Spades:
                    return "\u2660";
                default: 
                    throw new ArgumentException("Unrecognized value of suit");
            }
        }
    }
}