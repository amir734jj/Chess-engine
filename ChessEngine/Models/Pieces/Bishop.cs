using System.Collections.Generic;
using ChessEngine.Models.Enums;
using ChessEngine.Models.Interfaces;

namespace ChessEngine.Models.Pieces
{
    public class Bishop : IPiece
    {
        public PieceNameEnum PieceNameEnum { get; } = PieceNameEnum.Bishop;
        
        public TeamEnum TeamEnum { get; }

        public Position Position { get; set; }
        
        public Bishop(TeamEnum teamEnum, Position position)
        {
            TeamEnum = teamEnum;
            Position = position;
        }

        public object Clone() => MemberwiseClone();
    }
}