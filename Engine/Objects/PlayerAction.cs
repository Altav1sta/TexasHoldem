using Engine.Enums;

namespace Engine.Objects
{
    public class PlayerAction
    {
        public string PlayerId { get; }
        
        public ActionType Type { get; }

        public int? Value { get; }

        public PlayerAction(string playerId, ActionType type)
        {
            PlayerId = playerId;
            Type = type;
        }
        
        public PlayerAction(string playerId, ActionType type, int? value) : this(playerId, type)
        {
            Value = value;
        }
    }
}