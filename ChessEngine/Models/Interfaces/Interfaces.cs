using ChessEngine.Models.Enums;

namespace ChessEngine.Models.Interfaces
{
    /// <summary>
    /// Bishop interface.
    /// </summary>
    public interface IBishop { }

    /// <summary>
    /// King interface.
    /// </summary>
    public interface IKing
    {
        /// <summary>
        /// Verifies if the king can castle long.
        /// </summary>
        /// <param name="board">The board</param>
        /// <param name="from">The starting square</param>
        /// <param name="to">The ending square</param>
        /// <returns></returns>
        bool CanCastleLong(Pieces.Board board, int from, int to);

        /// <summary>
        /// Verifies if the king can castle short.
        /// </summary>
        /// <param name="board">The board</param>
        /// <param name="from">The starting square</param>
        /// <param name="to">The ending square</param>
        /// <returns></returns>
        bool CanCastleShort(Pieces.Board board, int from, int to);
    }

    /// <summary>
    /// Knight interface.
    /// </summary>
    public interface IKnight { }

    /// <summary>
    /// Pawn interface.
    /// </summary>
    public interface IPawn
    {
        /// <summary>
        /// Checks if it's the two-squares move.
        /// </summary>
        /// <param name="board">The board</param>
        /// <param name="from">The starting square</param>
        /// <param name="to">The ending square</param>
        /// <returns></returns>
        bool IsTwoSquaresMove(Pieces.Board board, int from, int to);

        /// <summary>
        /// Checks if it's the en passant move.
        /// </summary>
        /// <param name="board">The board</param>
        /// <param name="from">The starting square</param>
        /// <param name="to">The ending square</param>
        /// <returns></returns>
        bool IsEnPassantCaptureMove(Pieces.Board board, int from, int to);
    }

    /// <summary>
    /// Queen interface.
    /// </summary>
    public interface IQueen { }

    /// <summary>
    /// Rook interface.
    /// </summary>
    public interface IRook { }


    public interface IPiece
    {
        TeamEnum TeamEnum { get; }
        
        PieceNameEnum PieceNameEnum { get; }
    }
}