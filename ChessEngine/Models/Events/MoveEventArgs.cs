using System;
using ChessEngine.Models.Pieces;

namespace ChessEngine.Models.Events
{
    /// <summary>
    /// Move event args.
    /// </summary>
    public class MoveEventArgs : EventArgs
    {
        /// <summary>
        /// The move.
        /// </summary>
        public Move Move { get; }

        /// <summary>
        /// The move index.
        /// </summary>
        public int Index { get; }

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="move"></param>
        /// <param name="index"></param>
        public MoveEventArgs(Move move, int index)
        {
            Move = move;
            Index = index;
        }
    }
}