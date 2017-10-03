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
        /// The structure of blinds raise where <see cref="KeyValuePair.Key"/> of each item is the small blind value
        /// and <see cref="KeyValuePair.Value"/> is the period of time which is followed by raise of the small blind
        /// </summary>
        /// <example>
        /// Pair of the number A and period of B (minutes for example) means that the small blind possesses 
        /// the value of 50 after first 10 minutes of game
        /// </example>
        IEnumerable<KeyValuePair<int, TimeSpan>> SmallBlinds { get; set; }
        
        /// <summary>
        /// The structure of ante where <see cref="KeyValuePair.Key"/> of each item is the ante value
        /// and <see cref="KeyValuePair.Value"/> is the period of time which is followed by raise of the ante.
        /// </summary>
        IEnumerable<KeyValuePair<int, TimeSpan>> Ante { get; set; }
    }
}