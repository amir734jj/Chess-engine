using System.Collections.Generic;
using ChessEngine.Models;
using ChessEngine.Models.Interfaces;

namespace ChessEngine.Interfaces
{
    public interface IPieceActionLogic
    {
        IEnumerable<Position> GetActions(Board board, IPiece piece);
        
        IEnumerable<Position> GetValidActions(Board board, IPiece piece);

        IEnumerable<Movement> GetValidMovements(Board board, IPiece piece);
    }
}