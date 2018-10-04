using ChessEngine.Models;
using ChessEngine.Models.Enums;

namespace ChessEngine.Interfaces
{
    public interface IGameLogic
    {
        Board Play(Board board, TeamEnum teamEnum);
    }
}