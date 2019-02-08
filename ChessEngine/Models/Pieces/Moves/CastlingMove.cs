using ChessEngine.Models.Interfaces;

namespace ChessEngine.Models.Pieces.Moves
{
    /// <summary>
    /// Implements a castling move.
    /// </summary>
    public sealed class CastlingMove : Move
    {
        /// <summary>
        /// The rook move.
        /// </summary>
        public Move RookMove { get; }

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="before">The before status</param>
        /// <param name="from">The starting square</param>
        /// <param name="to">The ending square</param>
        /// <param name="rookMove">The rook move</param>
        /// <param name="actor"></param>
        internal CastlingMove(BoardStatus before, int from, int to, Move rookMove, IPiece actor)
            : base(before, from, to, actor)
        {
            RookMove = rookMove;
        }

        /// <summary>
        /// Makes the move, it doesn't check if it's a valid move.
        /// </summary>
        /// <param name="board">The board</param>
        internal override void Make(Board board)
        {
            RookMove.Make(board);// make the rook move first

            base.Make(board);// make the king move last to set the after board status
        }

        /// <summary>
        /// Takes back the move, it doesn't check if it's a valid move.
        /// </summary>
        /// <param name="board">The board</param>
        internal override void TakeBack(Board board)
        {
            RookMove.TakeBack(board);// take back the rook move first

            base.TakeBack(board);// take back the king move last to set the before board status
        }
    }
}
