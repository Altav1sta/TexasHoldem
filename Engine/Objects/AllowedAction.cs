using Engine.Enums;

namespace Engine.Objects
{
    public class AllowedAction
    {
        public ActionType Type { get; }
        
        public int? Min { get; }
        
        public int? Max { get; }

        internal AllowedAction(ActionType type)
        {
            Type = type;
        }

        internal AllowedAction(ActionType type, int min, int max) : this(type)
        {
            Min = min;
            Max = max;
        }
    }
}