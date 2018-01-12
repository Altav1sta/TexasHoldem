using System;
using System.Collections.Generic;
using Engine.Enums;

namespace Engine.Objects
{
    public class GameInfo
    {
        public IReadOnlyCollection<ActionType> AllowedActions { get; internal set; }
        
        public IReadOnlyDictionary<DateTime, HistoryRecord> History { get; internal set; }
    }
}