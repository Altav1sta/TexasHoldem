using System;
using System.Collections.Generic;

namespace Engine.Interfaces
{
    public interface IBlindStructure
    {
        /// <summary>
        /// Starting value of the small blind
        /// </summary>
        int StartValue { get; set; }
        
        /// <summary>
        /// The structure of blinds raise where Value of each item is the small blind value
        /// and Key is the period of time which is followed by raise of the small blind
        /// </summary>
        /// <example>
        /// Pair of the number A and period of B (minutes for example) means that the small blind possesses 
        /// the value of 50 after first 10 minutes of game
        /// </example>
        IReadOnlyDictionary<TimeSpan, int> SmallBlinds { get; set; }
        
        /// <summary>
        /// The structure of ante where Value of each item is the ante value
        /// and Key is the period of time which is followed by raise of the ante.
        /// </summary>
        IReadOnlyDictionary<TimeSpan, int> Ante { get; set; }
    }
}