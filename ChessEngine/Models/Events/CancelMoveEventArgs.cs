using ChessEngine.Models.Pieces;

namespace ChessEngine.Models.Events
{
    /// <summary>
    /// Cancel move event args.
    /// </summary>
    public class CancelMoveEventArgs : MoveEventArgs
    {
        /// <summary>
        /// Indicates whether the event should be cancelled.
        /// </summary>
        public bool Cancel { get; set; }

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="move"></param>
        /// <param name="index"></param>
        public CancelMoveEventArgs(Move move, int index)
            : base(move, index)
        {
        }
    }
}
