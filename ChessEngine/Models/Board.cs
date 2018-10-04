using System;
using ChessEngine.Models.Interfaces;

namespace ChessEngine.Models
{
    public class Board :  ICloneable
    {
        public IPiece[][] Matrix { get; set; }

        public object Clone() => MemberwiseClone();
    }
}