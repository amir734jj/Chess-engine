using System;
using ChessEngine.Models.Enums;
using ChessEngine.Models.Interfaces;

namespace ChessEngine.Models
{
    public class Movement : ICloneable
    {
        public IPiece Piece { get; set; }
        
        public PieceNameEnum PieceNameEnum { get; set; }
        
        public Position BeforePosition { get; set; }
        
        public Position AfterPosition { get; set; }
        
        public MovementTypeEnum MovementTypeEnum { get; set; }
        
        public object Clone() => MemberwiseClone();
    }
}