using Engine.Interfaces;

namespace ConsoleRunner
{
    public class GameSettings : IGameSettings
    {
        public GameSettings(int initialStack, IBlindStructure blindStructure)
        {
            BlindStructure = blindStructure;
            InitialStack = initialStack;
        }

        public GameSettings(int initialStack, IBlindStructure blindStructure, int? seed)
            : this(initialStack, blindStructure)
        {
            Seed = seed;
        }

        public int InitialStack { get; }
        public int? Seed { get; }
        public IBlindStructure BlindStructure { get; }
    }
}