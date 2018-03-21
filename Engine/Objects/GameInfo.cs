using System;
using System.Collections.Generic;

namespace Engine.Objects
{
    public class GameInfo
    {
        public IReadOnlyCollection<AllowedAction> AllowedActions { get; internal set; }
        
        public IReadOnlyDictionary<DateTime, HistoryRecord> History { get; internal set; }
    }
}