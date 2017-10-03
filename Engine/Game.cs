using System;
using Engine.Interfaces;
using Engine.Objects;

namespace Engine
{
    public class Game
    {
        
        private readonly Deck _deck = new Deck();
        private readonly int _playersCount;
        
        
        public Game(IGameSettings settings)
        {
            _playersCount = settings.PlayersCount;
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