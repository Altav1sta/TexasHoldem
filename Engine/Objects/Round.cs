using System;
using System.Collections.Generic;
using System.Linq;
using Engine.Enums;

namespace Engine.Objects
{
    internal class Round
    {
        private Game Game { get; }
        private Street? Street { get; set; }
        private List<Card> Board { get; } = new List<Card>();
        private int Pot { get; set; }
        private int Dealer { get; set; }

        public Round(Game game)
        {
            Game = game;
        }
        
        internal void StartStreet()
        {
            SetNextStreet();
            DealCardsToPlayers();
            DealCardsToBoard();
            MoveDealer();
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
        
        private void DealCardsToPlayers()
        {
            foreach (var player in Game.Players)
            {
                player?.Cards.Add(Game.Deck.TakeFirst());
                player?.Cards.Add(Game.Deck.TakeFirst());
            }
        }

        private void DealCardsToBoard()
        {
            switch (Street)
            {
                case null:
                    throw new Exception("You are trying to deal cards before preflop started!");
                    
                case Enums.Street.Preflop:
                    throw new Exception("Can't deal cards to the board during the preflop street!");

                case Enums.Street.Flop:
                    if (Board.Count > 0) throw new Exception("The board must be empty!");
                    Board.Add(Game.Deck.TakeFirst());
                    Board.Add(Game.Deck.TakeFirst());
                    Board.Add(Game.Deck.TakeFirst());
                    break;

                case Enums.Street.Turn:
                    if (Board.Count != 3) throw new Exception("The board must contain 3 cards!");
                    Board.Add(Game.Deck.TakeFirst());
                    break;
                    
                case Enums.Street.River:
                    if (Board.Count != 4) throw new Exception("The board must contain 4 cards!");
                    Board.Add(Game.Deck.TakeFirst());
                    break;
                    
                default:
                    throw new Exception(
                        $"Detected the attempt to deal cards to the board with unrecognized street type: {Street}!");
            }
        }

        private void MoveDealer()
        {
            Dealer++;
            
            if (Dealer == Game.Players.Length)
            {
                Dealer = 0;
            }

            if (Game.Players[Dealer].Stack == 0)
            {
                Dealer++;
            }
        }

        private void PutBlinds()
        {
            var timeElapsed = Game.StartTimeUtc - DateTime.UtcNow;
            
        }

        private int GetSmallBlind(TimeSpan timeElapsed)
        {
            if (Game.BlindStructure.SmallBlinds != null &&
                Game.BlindStructure.SmallBlinds.Keys.Any(x => x <= timeElapsed))
            {
                var currentTimeSpan = Game.BlindStructure.SmallBlinds.Keys
                    .Where(x => x <= timeElapsed)
                    .Max();

                return Game.BlindStructure.SmallBlinds[currentTimeSpan];
            }

            return Game.BlindStructure.StartValue;
        }
    }
}