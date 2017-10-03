using System.Collections.Generic;

namespace Engine.Interfaces
{
    public interface IGameSettings
    {
        int PlayersCount { get; set; }
        
        HashSet<string> PlayersIdentifiers { get; set; }
        
        IBlindStructure BlindStructure { get; set; }
    }
}