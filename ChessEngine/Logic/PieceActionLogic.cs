using System;
using System.Collections.Generic;
using System.Linq;
using ChessEngine.Extensions;
using ChessEngine.Interfaces;
using ChessEngine.Models;
using ChessEngine.Models.Constants;
using ChessEngine.Models.Enums;
using ChessEngine.Models.Interfaces;
using static ChessEngine.Utilities.BoardUtilities;

namespace ChessEngine.Logic
{
    public class PieceActionLogic : IPieceActionLogic
    {
        public IEnumerable<Position> GetActions(Board board, IPiece piece)
        {
            switch (piece.PieceNameEnum)
            {
                case PieceNameEnum.King:
                    return new HashSet<Position>
                    {
                        piece.Position.IncrementI(),
                        piece.Position.IncrementJ(),
                        piece.Position.DecrementI(),
                        piece.Position.DecrementJ()
                    };
                case PieceNameEnum.Queen:
                    break;
                case PieceNameEnum.Rook:
                    return Enumerable.Range(0, BoardConstants.Dimension)
                        .Select(value => new List<Position>
                        {
                            new Position {I = value, J = piece.Position.J},
                            new Position {I = piece.Position.J, J = value}
                        })
                        .SelectMany(x => x)
                        .ToHashSet();
                case PieceNameEnum.Bishop:
                    return Enumerable.Range(0, 2 * BoardConstants.Dimension)
                        .Select(value => new List<Position>
                        {
                            new Position {I = value, J = piece.Position.J},
                            new Position {I = piece.Position.J, J = value}
                        })
                        .SelectMany(x => x)
                        .ToHashSet();
                case PieceNameEnum.Knight:
                    switch (piece.TeamEnum)
                    {
                        case TeamEnum.White:
                            break;
                        case TeamEnum.Black:
                            break;
                        default:
                            throw new ArgumentOutOfRangeException();
                    }
                    break;
                case PieceNameEnum.Pawn:
                    switch (piece.TeamEnum)
                    {
                        case TeamEnum.White:
                            return new HashSet<Position> {piece.Position.DecrementI()};
                        case TeamEnum.Black:
                            return new HashSet<Position> {piece.Position.IncrementI()};
                        default:
                            throw new ArgumentOutOfRangeException();
                    }
                default:
                    throw new ArgumentOutOfRangeException(nameof(PieceNameEnum), piece.PieceNameEnum, null);
            }
        }
        
        /// <summary>
        /// Returns valid actions
        /// </summary>
        /// <param name="board"></param>
        /// <param name="piece"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public IEnumerable<Position> GetValidActions(Board board, IPiece piece)
        {
            return GetActions(board, piece)
                // Remove invalid actions
                .Where(position => ValidateAction(board, piece, position));
        }

        /// <summary>
        /// Returns valid actions wrapped as movements
        /// </summary>
        /// <param name="board"></param>
        /// <param name="piece"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public IEnumerable<Movement> GetValidMovements(Board board, IPiece piece)
        {
            return GetValidActions(board, piece)
                .Select(x => new Movement
                {
                    Piece = piece,
                    PieceNameEnum = piece.PieceNameEnum,
                    AfterPosition = x,
                    BeforePosition = piece.Position,
                    // TODO
                    MovementTypeEnum = MovementTypeEnum.Simple
                });
        }
    }
}