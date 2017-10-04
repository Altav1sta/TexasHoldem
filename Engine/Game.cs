using System;
using System.Linq;
using Engine.Interfaces;
using Engine.Objects;

namespace Engine
{
    public class Game
    {
        private GameManager Manager { get; } = new GameManager();
        private Deck Deck { get; } = new Deck();
        private Player[] Players { get; }
        
        public Game(IGameSettings settings)
        {
            var random = new Random();
            var identifiers = settings.PlayersIdentifiers.OrderBy(x => random.Next()).ToArray();
            
            Players = new Player[settings.PlayersCount];

            for (var i = 0; i < Players.Length; i++)
            {
                Players[i] = new Player
                {
                    Id = identifiers[i]
                };
            }
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