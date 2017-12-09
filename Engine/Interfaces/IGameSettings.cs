namespace Engine.Interfaces
{
    public interface IGameSettings
    {
        int InitialStack { get; }
        
        int? Seed { get; }
        
        IBlindStructure BlindStructure { get; }
    }
}