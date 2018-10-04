using System.Collections.Generic;
using ChessEngine.Models.Enums;
using ChessEngine.Models.Interfaces;

namespace ChessEngine.Models.Pieces
{
    public class Pawn : IPiece
    {
        public PieceNameEnum PieceNameEnum { get; } = PieceNameEnum.Pawn;
        
        public TeamEnum TeamEnum { get; }

        public Position Position { get; set; }
        
        public Pawn(TeamEnum teamEnum, Position position)
        {
            TeamEnum = teamEnum;
            Position = position;
        }
    }
}