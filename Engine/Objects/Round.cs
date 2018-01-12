using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Engine.Enums;

namespace Engine.Objects
{
    internal class Round
    {
        private readonly Card[] board = new Card[5];
        private readonly Game game;
        private readonly int smallBlind;
        private readonly IList<PartialPot> pots = new List<PartialPot>();
        private Street? street;

        internal Round(Game game)
        {
            this.game = game;
            smallBlind = GetSmallBlind(DateTime.UtcNow - game.StartTimeUtc);
        }

        internal void StartStreet()
        {
            SetNextStreet();

            if (street != Street.Preflop)
            {
                DealCardsToBoard();
            }
            else
            {
                DealCardsToPlayers();
                PutBlinds();
            }
        }

        internal IReadOnlyCollection<ActionType> GetAllowedActions(string playerId)
        {
            if (!IsCurrentActivePlayer(playerId)) return null;

            var maxBet = game.Players.Max(x => x.CurrentBet);

            // no need to bet (call can be 0)
            if (maxBet <= game.Players[playerId].CurrentBet)
            {
                return new[] { ActionType.Call, ActionType.Raise };
            }

            // call != all in
            if (maxBet <= game.Players[playerId].Stack)
            {
                return new[] { ActionType.Call, ActionType.Fold, ActionType.Raise };
            }
            
            // no money for raise
            return new[] { ActionType.Call, ActionType.Fold };
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
                if (player.Stack <= 0) continue;
                
                player.Cards = new[] { game.Deck.TakeFirst(), game.Deck.TakeFirst() };
                player.HasHoleCards = true;
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

                game.History[DateTime.UtcNow] = new HistoryRecord
                {
                    Round = game.RoundNumber,
                    Street = Street.Preflop,
                    Pots = new ReadOnlyCollection<PartialPot>(pots),
                    Board = board,
                    PlayerAction = new PlayerAction
                    {
                        PlayerId = player.Id,
                        Type = ActionType.Blind,
                        Value = player.CurrentBet
                    },
                    Players = game.Players
                };
                
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
        
        private bool IsCurrentActivePlayer(string playerId)
        {
            if (!street.HasValue) return false;
            
            using (var enumerator = game.Players.GetEnumerator())
            {
                var lastHistoryRecord = game.History.Count > 0 ? game.History.OrderBy(x => x.Key).Last().Value : null;
                var lastActivePlayerInStreet =
                    lastHistoryRecord?.Round == game.RoundNumber && lastHistoryRecord.Street == street
                        ? lastHistoryRecord.PlayerAction.PlayerId
                        : null;
                
                // if nobody made move in this street return flag depending on street type
                if (lastActivePlayerInStreet == null)
                {
                    return street == Street.Preflop
                        ? game.Players[2].Id == playerId
                        : game.Players[0].Id == playerId;
                }
                
                // move enumerator to last active player
                while (enumerator.Current.Id != lastActivePlayerInStreet)
                {
                    enumerator.MoveNext();
                }

                // find next player with positive stack
                while (true)
                {
                    enumerator.MoveNext();

                    if (enumerator.Current.Stack > 0) 
                        return enumerator.Current.Id == playerId;
                }
            }
        }
    }
}