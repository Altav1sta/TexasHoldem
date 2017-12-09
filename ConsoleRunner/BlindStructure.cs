using System;
using System.Collections.Generic;
using Engine.Interfaces;

namespace ConsoleRunner
{
    public class BlindStructure : IBlindStructure
    {
        public BlindStructure(int startValue)
        {
            StartValue = startValue;
        }

        public BlindStructure(int startValue, IReadOnlyDictionary<TimeSpan, int> smallBlinds,
                              IReadOnlyDictionary<TimeSpan, int> ante) : this(startValue)
        {
            SmallBlinds = smallBlinds;
            Ante = ante;
        }

        public int StartValue { get; }
        public IReadOnlyDictionary<TimeSpan, int> SmallBlinds { get; }
        public IReadOnlyDictionary<TimeSpan, int> Ante { get; }
    }
}