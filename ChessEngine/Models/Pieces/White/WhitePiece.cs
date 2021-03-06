using ChessEngine.Models.Enums;

namespace ChessEngine.Models.Pieces.White
{
    /// <summary>
    /// Implements a white chess piece.
    /// </summary>
    public abstract class WhitePiece : Piece
    {
        /// <summary>
        /// Generates the move.
        /// Adds check verification to move generation,
        /// returns null if its own king will be in check.
        /// </summary>
        /// <param name="board">The board</param>
        /// <param name="from">The starting square</param>
        /// <param name="to">The ending square</param>
        /// <returns></returns>
        internal override Move GenerateMove(Board board, int from, int to)
        {
            var move = base.GenerateMove(board, from, to);

            if (move != null)
            {
                // verify for king in check 
                move.Make(board);
                var result = !board.WhiteKingInCheck();
                move.TakeBack(board);
                return result ? move : null;
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// Checks if the piece might move on this "board", 
        /// from the "from" square to the "to" square according to the chess rules.
        /// It doesn't verify if its own king is in check after the move.
        /// </summary>
        /// <param name="board">The board</param>
        /// <param name="from">The starting square</param>
        /// <param name="to">The ending square</param>
        /// <returns></returns>
        protected override bool MightMove(Board board, int from, int to)
        {
            // the first condition for a piece to be able to move
            // is that starting square and the ending square be different
            // and the ending square be empty or opposite piece
            return from != to && !(board[to] is WhitePiece);
        }
    }
}
