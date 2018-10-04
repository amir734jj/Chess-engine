using System;

namespace ChessEngine.Models
{
    /// <summary>
    /// Describes a position on the board
    /// </summary>
    public class Position : ICloneable
    {
        public int I { get; set; }
        
        public int J { get; set; }
        
        public object Clone() => MemberwiseClone();
    }
}