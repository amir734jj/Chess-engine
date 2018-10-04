using System;
using ChessEngine.Models;
using ChessEngine.Models.Enums;
using ChessEngine.Models.Interfaces;
using ChessEngine.Models.Pieces;

namespace ChessEngine.Extensions
{
    public static class PieceExtension
    {
        /// <summary>
        /// Moves the piece given a new position
        /// </summary>
        /// <param name="piece"></param>
        /// <param name="position"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        public static IPiece Move(this IPiece piece, Position position)
        {
            switch (piece.PieceNameEnum)
            {
                case PieceNameEnum.King:
                    return new King(piece.TeamEnum, position);
                case PieceNameEnum.Queen:
                    return new Queen(piece.TeamEnum, position);
                case PieceNameEnum.Rook:
                    return new Rook(piece.TeamEnum, position);
                case PieceNameEnum.Bishop:
                    return new Bishop(piece.TeamEnum, position);
                case PieceNameEnum.Knight:
                    return new Knight(piece.TeamEnum, position);
                case PieceNameEnum.Pawn:
                    return new Pawn(piece.TeamEnum, position);
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}