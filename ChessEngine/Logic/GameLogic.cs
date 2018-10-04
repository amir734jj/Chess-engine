using System;
using ChessEngine.Interfaces;
using ChessEngine.Models;
using ChessEngine.Models.Enums;

namespace ChessEngine.Logic
{
    public class GameLogic : IGameLogic
    {
        public void Play(Board board, TeamEnum teamEnum)
        {
            switch (teamEnum)
            {
                case TeamEnum.White:
                    break;
                case TeamEnum.Black:
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(teamEnum), teamEnum, null);
            }
        }
    }
}