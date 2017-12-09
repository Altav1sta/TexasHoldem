using System;
using System.Collections.Generic;
using System.Linq;
using Engine.Interfaces;
using Engine.Objects;

namespace Engine
{
    public class Game
    {
        public Game(IGameSettings settings)
        {
            BlindStructure = settings.BlindStructure;
            InitialStack = settings.InitialStack;
            Seed = settings.Seed;
            Deck = new Deck(Seed);
        }

        internal DateTime StartTimeUtc { get; } = DateTime.UtcNow;
        internal Deck Deck { get; }
        internal PlayersChain Players { get; private set; }
        internal IBlindStructure BlindStructure { get; }
        internal int InitialStack { get; }
        internal int? Seed { get; }

        private Round round;

        public void Start(IEnumerable<string> playerNames)
        {
            var players = playerNames.Select(x => new Player { Id = x, Stack = InitialStack });
            Players = new PlayersChain(players, Seed);
            StartNewRound();
        }

        public GameInfo GetInfo()
        {
            throw new NotImplementedException();
        }

        public void MakeMove(PlayerAction playerAction)
        {
        }

        private void StartNewRound()
        {
            round = new Round(this);
            Deck.Shuffle();
            round.StartStreet();
        }
    }
}