using System;
using System.Collections.Generic;
using System.Linq;
using Engine.Enums;

namespace Engine.Objects
{
    internal class Deck
    {
        private readonly int? seed;
        private Queue<Card> deck;

        internal Deck(int? seed)
        {
            this.seed = seed;
            deck = new Queue<Card>(GetInitialCollection());
        }

        internal Card TakeFirst()
        {
            return deck.Dequeue();
        }

        internal void Shuffle()
        {
            var r = seed.HasValue ? new Random(seed.Value) : new Random();
            deck = new Queue<Card>(deck.OrderBy(x => r.Next()));
        }
        
        private static IEnumerable<Card> GetInitialCollection()
        {
            var cards = new List<Card>();

            foreach (Suit suit in Enum.GetValues(typeof(Suit)))
            foreach (Rank rank in Enum.GetValues(typeof(Rank)))
            {
                cards.Add(new Card { Rank = rank, Suit = suit });
            }

            return cards;
        }
    }
}