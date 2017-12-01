using System;
using System.Collections.Generic;
using System.Linq;
using Engine.Interfaces;
using Engine.Objects;

namespace Engine
{
    public class Game
    {
        internal DateTime StartTimeUtc { get; } = DateTime.UtcNow;
        internal Deck Deck { get; }
        internal Player[] Players { get; }
        internal IBlindStructure BlindStructure { get; }
        internal int InitialStack { get; }
        internal int Dealer { get; set; }
        
        private RoundManager RoundManager { get; }
        
        public Game(IGameSettings settings)
        {
            RoundManager = new RoundManager(this);
            Deck = new Deck();
            Players = new Player[settings.PlayersCount];
            BlindStructure = settings.BlindStructure;
            InitialStack = settings.InitialStack;
        }

        
        public void Start(IEnumerable<string> players)
        {
            var random = new Random();
            var shuffledPlayers = players.OrderBy(x => random.Next()).ToArray();
            
            for (var i = 0; i < Players.Length; i++)
            {
                Players[i] = new Player
                {
                    Id = shuffledPlayers[i],
                    Stack = InitialStack
                };
            }
            
            RoundManager.StartNew();
        }
        
        public GameInfo GetInfo()
        {
            throw new NotImplementedException();
        }
        
        public void MakeMove(PlayerAction playerAction)
        {
            
        }
    }
}