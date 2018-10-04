using System.Collections.Generic;
using ChessEngine.Models.Enums;
using ChessEngine.Models.Interfaces;

namespace ChessEngine.Models.Pieces
{
    public class Rock : IPiece
    {
        public PieceNameEnum PieceNameEnum { get; } = PieceNameEnum.Rook;
        
        public TeamEnum TeamEnum { get; }

        public Position Position { get; set; }
        
        public Rock(TeamEnum teamEnum, Position position)
        {
            TeamEnum = teamEnum;
            Position = position;
        }
    }
}