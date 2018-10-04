using ChessEngine.Models;
using ChessEngine.Models.Constants;
using ChessEngine.Models.Interfaces;

namespace ChessEngine.Utilities
{
    public static class BoardUtilities
    {
        /// <summary>
        /// Validates an action
        /// </summary>
        /// <param name="board"></param>
        /// <param name="piece"></param>
        /// <param name="position"></param>
        /// <returns></returns>
        public static bool ValidateAction(Board board, IPiece piece, Position position)
        {
            var destination = board.Matrix[position.I][position.J];

            // Position is available
            if (destination == null)
            {
                return true;
            }

            // Cannot override itself
            if (destination == piece)
            {
                return false;
            }

            // Out of range
            if (position.I < 0 || position.J < 0)
            {
                return false;
            }

            // Out of range
            if (position.I >= BoardConstants.Dimension || position.J >= BoardConstants.Dimension)
            {
                return false;
            }

            // Cannot override the piece from the same team
            // ReSharper disable once ConvertIfStatementToReturnStatement
            if (destination.TeamEnum == piece.TeamEnum)
            {
                return false;
            }

            return true;
        }
    }
}