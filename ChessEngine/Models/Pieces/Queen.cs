using System.Collections.Generic;
using ChessEngine.Models.Enums;
using ChessEngine.Models.Interfaces;

namespace ChessEngine.Models.Pieces
{
    public class Queen : IPiece
    {
        public PieceNameEnum PieceNameEnum { get; } = PieceNameEnum.Queen;
        
        public TeamEnum TeamEnum { get; }

        public Position Position { get; set; }

        public Queen(TeamEnum teamEnum, Position position)
        {
            TeamEnum = teamEnum;
            Position = position;
        }
    }
}