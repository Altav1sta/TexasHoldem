using Engine.Enums;

namespace Engine.Objects
{
    public class PlayerAction
    {
        public string PlayerId { get; set; }
        
        public ActionType Type { get; set; }

        public int? Value { get; set; }
    }
}