using System.Collections.Generic;
using System.Linq;
using ChessEngine.Models;
using ChessEngine.Models.Enums;
using ChessEngine.Models.Interfaces;

namespace ChessEngine.Extensions
{
    public static class BoardExtensions
    {
        /// <summary>
        /// Returns the available pieces
        /// </summary>
        /// <param name="board"></param>
        /// <param name="teamEnum"></param>
        /// <returns></returns>
        public static IEnumerable<IPiece> GetAvailablePieces(this Board board, TeamEnum teamEnum)
        {
            return board.Matrix
                // Get matching team pieces
                .Select(x => x.Where(y => y.TeamEnum == teamEnum))
                // Flatten
                .SelectMany(x => x);
        }
    }
}