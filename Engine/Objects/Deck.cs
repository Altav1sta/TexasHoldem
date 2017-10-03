using System;
using System.Collections.Generic;
using System.Linq;
using Engine.Enums;

namespace Engine.Objects
{
    public class Deck
    {
        private readonly Queue<Card> _deck;

        public Deck()
        {
            _deck = new Queue<Card>(GetInitialCollection());
        }

        public Card TakeFirst()
        {
            return _deck.Dequeue();
        }
        
        private static IEnumerable<Card> GetInitialCollection()
        {
            var cards = new List<Card>();

            foreach (Suit suit in Enum.GetValues(typeof(Suit)))
            {
                foreach (Rank rank in Enum.GetValues(typeof(Rank)))
                {
                    cards.Add(new Card
                    {
                        Rank = rank,
                        Suit = suit
                    });
                }
            }

            var r = new Random();
            
            return cards.OrderBy(x => r.Next());
        }
    }
}