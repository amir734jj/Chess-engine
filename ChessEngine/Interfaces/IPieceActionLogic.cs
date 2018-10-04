using System.Collections.Generic;
using ChessEngine.Models;
using ChessEngine.Models.Interfaces;

namespace ChessEngine.Interfaces
{
    public interface IPieceActionLogic
    {
        IEnumerable<Position> GetValidActions(Board board, IPiece piece);
    }
}