using ChessEngine.Models;
using ChessEngine.Models.Enums;

namespace ChessEngine.Interfaces
{
    public interface IGameLogic
    {
        void Play(Board board, TeamEnum teamEnum);
    }
}