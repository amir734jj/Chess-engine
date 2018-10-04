using System.Collections.Generic;
using ChessEngine.Models.Enums;

namespace ChessEngine.Models.Interfaces
{
    public interface IPiece
    {
        PieceNameEnum PieceNameEnum { get; }
        
        TeamEnum TeamEnum { get; }

        Position Position { get; set; }
    }
}