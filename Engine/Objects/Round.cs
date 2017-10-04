using System;
using System.Collections.Generic;
using Engine.Enums;

namespace Engine.Objects
{
    internal class Round
    {
        private Street? Street { get; set; }
        private List<Card> Board { get; } = new List<Card>();

        internal void StartStreet(Player[] players, Deck deck)
        {
            SetNextStreet();
            DealCardsToPlayers(players, deck);
            DealCardsToBoard(deck);
        }


        private void SetNextStreet()
        {
            switch (Street)
            {
                case null:
                    Street = Enums.Street.Preflop;
                    break;
                    
                case Enums.Street.Preflop:
                    Street = Enums.Street.Flop;
                    break;
                    
                case Enums.Street.Flop:
                    Street = Enums.Street.Turn;
                    break;
                    
                case Enums.Street.Turn:
                    Street = Enums.Street.River;
                    break;
                    
                case Enums.Street.River:
                    Street = null;
                    break;
                    
                default:
                    throw new Exception("Can't switch street from unrecognized street type");
            }
        }
        
        private void DealCardsToPlayers(IEnumerable<Player> players, Deck deck)
        {
            foreach (var player in players)
            {
                player?.Cards.Add(deck.TakeFirst());
                player?.Cards.Add(deck.TakeFirst());
            }
        }

        private void DealCardsToBoard(Deck deck)
        {
            switch (Street)
            {
                case null:
                    throw new Exception("You are trying to deal cards before preflop started!");
                    
                case Enums.Street.Preflop:
                    throw new Exception("Can't deal cards to the board during the preflop street!");

                case Enums.Street.Flop:
                    if (Board.Count > 0) throw new Exception("The board must be empty!");
                    Board.Add(deck.TakeFirst());
                    Board.Add(deck.TakeFirst());
                    Board.Add(deck.TakeFirst());
                    break;

                case Enums.Street.Turn:
                    if (Board.Count != 3) throw new Exception("The board must contain 3 cards!");
                    Board.Add(deck.TakeFirst());
                    break;
                    
                case Enums.Street.River:
                    if (Board.Count != 4) throw new Exception("The board must contain 4 cards!");
                    Board.Add(deck.TakeFirst());
                    break;
                    
                default:
                    throw new Exception("Detected the attempt to deal cards to the board with unrecognized street type!");
            }
        }
    }
}