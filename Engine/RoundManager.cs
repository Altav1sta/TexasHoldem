using Engine.Objects;

namespace Engine
{
    internal class RoundManager
    {
        private Round Round { get; set; }

        internal void StartNew()
        {
            Round = new Round();
        }
    }
}