namespace Engine.Interfaces
{
    public interface IGameSettings
    {
        int PlayersCount { get; set; }
        
        int InitialStack { get; set; }
        
        IBlindStructure BlindStructure { get; set; }
    }
}