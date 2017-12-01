using Engine.Objects;

namespace Engine
{
    internal class RoundManager
    {
        private Game Game { get; }
        private Round Round { get; set; }

        public RoundManager(Game game)
        {
            Game = game;
        }
        
        internal void StartNew()
        {
            Round = new Round(Game);
            Round.StartStreet();
        }
    }
}