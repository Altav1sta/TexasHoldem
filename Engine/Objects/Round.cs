using System;
using System.Linq;
using Engine.Enums;

namespace Engine.Objects
{
    internal class Round
    {
        private readonly Card[] board = new Card[5];
        private readonly Game game;
        private readonly int smallBlind;
        private Street? street;
        private int roundPot;
        private int streetPot;

        internal Round(Game game)
        {
            this.game = game;
            smallBlind = GetSmallBlind(DateTime.UtcNow - game.StartTimeUtc);
        }

        public void StartStreet()
        {
            SetNextStreet();
            DealCardsToPlayers();

            if (street.HasValue && street != Street.Preflop)
            {
                DealCardsToBoard();
            }
            else
            {
                if (!game.Players.MoveDealer()) throw new Exception("Can't move dealer button");
                
                PutBlinds();
            }
        }

        private void SetNextStreet()
        {
            switch (street)
            {
                case null:
                    street = Street.Preflop;
                    return;
                case Street.Preflop:
                    street = Street.Flop;
                    break;
                case Street.Flop:
                    street = Street.Turn;
                    break;
                case Street.Turn:
                    street = Street.River;
                    break;
                case Street.River:
                    street = null;
                    return;
                default:
                    throw new Exception($"Unrecognized street type {street}");
            }
        }

        private void DealCardsToPlayers()
        {
            foreach (var player in game.Players)
            {
                player.Cards = new[] { game.Deck.TakeFirst(), game.Deck.TakeFirst() };
            }
        }

        private void DealCardsToBoard()
        {
            switch (street)
            {
                case null:
                    throw new Exception("An attempt to deal cards to the board before preflop started");

                case Street.Preflop:
                    throw new Exception("An attempt to deal cards to the board during the preflop street!");

                case Street.Flop:
                    board[0] = game.Deck.TakeFirst();
                    board[1] = game.Deck.TakeFirst();
                    board[2] = game.Deck.TakeFirst();
                    break;

                case Street.Turn:
                    board[3] = game.Deck.TakeFirst();
                    break;

                case Street.River:
                    board[4] = game.Deck.TakeFirst();
                    break;

                default:
                    throw new Exception(
                        $"An attempt to deal cards to the board with unrecognized street type: {street}!");
            }
        }

        private void PutBlinds()
        {
            var blindValue = smallBlind;

            foreach (var player in game.Players)
            {
                if (player.Stack == 0) continue;

                if (player.Stack < blindValue)
                {
                    player.CurrentBet = player.Stack;
                    player.Stack = 0;
                }
                else
                {
                    player.CurrentBet = blindValue;
                    player.Stack -= blindValue;
                }

                if (blindValue == smallBlind * 2) break;

                blindValue *= 2;
            }
        }

        private int GetSmallBlind(TimeSpan timeElapsed)
        {
            if (!game.BlindStructure.SmallBlinds?.Keys.Any(x => x <= timeElapsed) ?? true)
            {
                return game.BlindStructure.StartValue;
            }

            var currentTimeSpan = game.BlindStructure.SmallBlinds.Keys.Where(x => x <= timeElapsed).Max();
            return game.BlindStructure.SmallBlinds[currentTimeSpan];
        }
    }
}