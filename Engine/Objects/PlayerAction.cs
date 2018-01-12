using Engine.Enums;

namespace Engine.Objects
{
    public class PlayerAction
    {
        public string PlayerId { get; internal set; }
        
        public ActionType Type { get; internal set; }

        public int? Value { get; internal set; }
    }
}