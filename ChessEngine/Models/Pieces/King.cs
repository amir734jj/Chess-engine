using System.Collections.Generic;
using ChessEngine.Models.Enums;
using ChessEngine.Models.Interfaces;

namespace ChessEngine.Models.Pieces
{
    public class King : IPiece
    {
        public PieceNameEnum PieceNameEnum { get; } = PieceNameEnum.King;
        
        public TeamEnum TeamEnum { get; }

        public Position Position { get; set; }

        public King(TeamEnum teamEnum, Position position)
        {
            TeamEnum = teamEnum;
            Position = position;
        }
    }
}