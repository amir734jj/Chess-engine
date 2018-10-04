using System;
using System.Collections.Generic;
using System.Linq;
using ChessEngine.Extensions;
using ChessEngine.Interfaces;
using ChessEngine.Models;
using ChessEngine.Models.Enums;
using static ChessEngine.Utilities.BoardUtilities;

namespace ChessEngine.Logic
{
    public class GameLogic : IGameLogic
    {
        private readonly IPieceActionLogic _pieceActionLogic;

        public GameLogic(IPieceActionLogic pieceActionLogic)
        {
            _pieceActionLogic = pieceActionLogic;
        }
        
        public Board Play(Board board, TeamEnum teamEnum)
        {
            var action = board.GetAvailablePieces(teamEnum)
                .Select(x => _pieceActionLogic.GetValidMovements(board, x))
                .SelectMany(x => x)
                .Select(x =>
                {
                    switch (x.MovementTypeEnum)
                    {
                        case MovementTypeEnum.Simple:
                            return new KeyValuePair<int, Movement>(1, x);
                        case MovementTypeEnum.Eliminate:
                            return new KeyValuePair<int, Movement>(10, x);
                        default:
                            throw new ArgumentOutOfRangeException();
                    }
                })
                .OrderBy(x => x.Key)
                .First()
                .Value;

            return ApplyAction(board, action);
        }
    }
}