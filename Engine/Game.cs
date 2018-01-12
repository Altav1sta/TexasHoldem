using System;
using System.Collections.Generic;
using System.Linq;
using Engine.Interfaces;
using Engine.Objects;

namespace Engine
{
    public class Game
    {
        private Round round;

        public Game(IGameSettings settings)
        {
            BlindStructure = settings.BlindStructure;
            InitialStack = settings.InitialStack;
            Seed = settings.Seed;
            Deck = new Deck(Seed);
        }

        internal Deck Deck { get; }
        internal DateTime StartTimeUtc { get; } = DateTime.UtcNow;
        internal PlayersChain Players { get; private set; }
        internal IBlindStructure BlindStructure { get; }
        internal IDictionary<DateTime, HistoryRecord> History { get; } = new Dictionary<DateTime, HistoryRecord>();
        internal int RoundNumber { get; private set; }
        internal int InitialStack { get; }
        internal int? Seed { get; }

        public void Start(IEnumerable<string> playerNames)
        {
            var players = playerNames.Select(x => new Player { Id = x, Stack = InitialStack });
            Players = new PlayersChain(players, Seed);
            StartNewRound();
        }

        public GameInfo GetInfo(string playerId)
        {
            return new GameInfo
            {
                History = History,
                AllowedActions = round?.GetAllowedActions(playerId)
            };
        }

        public void MakeMove(PlayerAction playerAction)
        {
        }

        private void StartNewRound()
        {
            round = new Round(this);
            RoundNumber++;
            Deck.Shuffle();
            round.StartStreet();
        }
    }
}