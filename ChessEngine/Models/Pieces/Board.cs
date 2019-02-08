using System;
using ChessEngine.Models.Pieces.Black;
using ChessEngine.Models.Pieces.White;
using static ChessEngine.Models.Constants.BoardConstants;

namespace ChessEngine.Models.Pieces
{
    /// <summary>
    /// Implements a chessboard.
    /// </summary>
    public class Board
    {
        /// <summary>
        /// The number of squares on the board side.
        /// </summary>
        public const int SideSquareNo = Dimension;

        /// <summary>
        /// The number of squares.
        /// </summary>
        public const int SquareNo = SideSquareNo * SideSquareNo;

        #region Frequently used squares
        /// <summary>
        /// A1 square position.
        /// </summary>
        public const int A1 = 56;
        /// <summary>
        /// B1 square position.
        /// </summary>
        public const int B1 = 57;
        /// <summary>
        /// C1 square position.
        /// </summary>
        public const int C1 = 58;
        /// <summary>
        /// D1 square position.
        /// </summary>
        public const int D1 = 59;
        /// <summary>
        /// E1 square position.
        /// </summary>
        public const int E1 = 60;
        /// <summary>
        /// F1 square position.
        /// </summary>
        public const int F1 = 61;
        /// <summary>
        /// G1 square position.
        /// </summary>
        public const int G1 = 62;
        /// <summary>
        /// H1 square position.
        /// </summary>
        public const int H1 = 63;

        /// <summary>
        /// A8 square position.
        /// </summary>
        public const int A8 = 0;
        /// <summary>
        /// B8 square position.
        /// </summary>
        public const int B8 = 1;
        /// <summary>
        /// C8 square position.
        /// </summary>
        public const int C8 = 2;
        /// <summary>
        /// D8 square position.
        /// </summary>
        public const int D8 = 3;
        /// <summary>
        /// E8 square position.
        /// </summary>
        public const int E8 = 4;
        /// <summary>
        /// F8 square position.
        /// </summary>
        public const int F8 = 5;
        /// <summary>
        /// G8 square position.
        /// </summary>
        public const int G8 = 6;
        /// <summary>
        /// H8 square position.
        /// </summary>
        public const int H8 = 7;
        #endregion

        /// <summary>
        /// Array of 64 squares, from left to right, top to bottom.
        /// </summary>
        private readonly Piece[] _squares;
        /// <summary>
        /// Board status.
        /// </summary>
        private BoardStatus _status;

        /// <summary>
        /// Gets a board in the starting position.
        /// </summary>
        /// <returns></returns>
        public static Board GetStartingBoard()
        {
            var squares = new Piece[SquareNo];
            var status = new BoardStatus();

            squares[A8] = new BlackRook();
            squares[B8] = new BlackKnight();
            squares[C8] = new BlackBishop();
            squares[D8] = new BlackQueen();
            squares[E8] = new BlackKing();
            squares[F8] = new BlackBishop();
            squares[G8] = new BlackKnight();
            squares[H8] = new BlackRook();

            for (var sqIndex = SideSquareNo; sqIndex < SideSquareNo * 2; sqIndex++)
            {
                squares[sqIndex] = new BlackPawn();
            }

            for (var sqIndex = SideSquareNo * (SideSquareNo - 2); sqIndex < SideSquareNo * (SideSquareNo - 1); sqIndex++)
            {
                squares[sqIndex] = new WhitePawn();
            }

            squares[A1] = new WhiteRook();
            squares[B1] = new WhiteKnight();
            squares[C1] = new WhiteBishop();
            squares[D1] = new WhiteQueen();
            squares[E1] = new WhiteKing();
            squares[F1] = new WhiteBishop();
            squares[G1] = new WhiteKnight();
            squares[H1] = new WhiteRook();

            status.WhiteTurn = true;
            status.WhiteCouldCastleLong = status.WhiteCouldCastleShort = status.BlackCouldCastleLong = status.BlackCouldCastleShort = true;
            status.EnPassantTarget = null;
            status.Ply = 0;
            status.Moves = 1;

            return new Board(squares, status);
        }

        /// <summary>
        /// Returns true if the board is in starting position, false otherwise.
        /// </summary>
        /// <returns></returns>
        public bool IsInStartingPosition()
        {
            if (
                !(_squares[A8] is BlackRook) ||
                !(_squares[B8] is BlackKnight) ||
                !(_squares[C8] is BlackBishop) ||
                !(_squares[D8] is BlackQueen) ||
                !(_squares[E8] is BlackKing) ||
                !(_squares[F8] is BlackBishop) ||
                !(_squares[G8] is BlackKnight) ||
                !(_squares[H8] is BlackRook)
                )
            {
                return false;
            }

            for (var sqIndex = SideSquareNo; sqIndex < SideSquareNo * 2; sqIndex++)
            {
                if (!(_squares[sqIndex] is BlackPawn)) { return false; }
            }

            for (var sqIndex = SideSquareNo * 2 + 1; sqIndex < SideSquareNo * (SideSquareNo - 2); sqIndex++)
            {
                if (_squares[sqIndex] != null) { return false; }
            }

            for (var sqIndex = SideSquareNo * (SideSquareNo - 2); sqIndex < SideSquareNo * (SideSquareNo - 1); sqIndex++)
            {
                if (!(_squares[sqIndex] is WhitePawn)) { return false; }
            }

            if (
                !(_squares[A1] is WhiteRook) ||
                !(_squares[B1] is WhiteKnight) ||
                !(_squares[C1] is WhiteBishop) ||
                !(_squares[D1] is WhiteQueen) ||
                !(_squares[E1] is WhiteKing) ||
                !(_squares[F1] is WhiteBishop) ||
                !(_squares[G1] is WhiteKnight) ||
                !(_squares[H1] is WhiteRook)
                )
            {
                return false;
            }

            if (
                _status.BlackTurn ||
                !_status.WhiteCouldCastleLong || !_status.WhiteCouldCastleShort || !_status.BlackCouldCastleLong || !_status.BlackCouldCastleShort ||
                _status.EnPassantTarget != null ||
                _status.Ply != 0 ||
                _status.Moves != 1)
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// Indexer for the squares.
        /// </summary>
        public Piece this[int index]
        {
            get => _squares[index];
            internal set => _squares[index] = value;
        }

        /// <summary>
        /// Board status.
        /// </summary>
        public BoardStatus Status
        {
            get => _status;
            internal set => _status = value;
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="squares"></param>
        /// <param name="status"></param>
        public Board(Piece[] squares, BoardStatus status)
        {
            _squares = squares;
            _status = status;
        }

        /// <summary>
        /// Checks if the squares from "from" square to "to" square are empty, excluding the starting and ending square.
        /// The direction can be horizontally, vertically or diagonally.
        /// </summary>
        /// <param name="from">The starting square</param>
        /// <param name="to">The ending square</param>
        /// <returns></returns>
        public bool IsPathClear(int from, int to)
        {
            // rdiff and fdiff will be -1,0 or 1
            var rdiff = Math.Sign(Rank(to) - Rank(from));
            var fdiff = Math.Sign(File(to) - File(from));

            var rank = Rank(from) + rdiff;
            var file = File(from) + fdiff;

            // loop through the squares
            while (rank != Rank(to) || file != File(to))
            {
                if (_squares[Position(rank, file)] != null)
                {
                    return false;
                }

                rank += rdiff;
                file += fdiff;
            }
            return true;
        }

        /// <summary>
        /// Checks if a square is attacked by White side.
        /// </summary>
        /// <param name="position">The position</param>
        /// <returns></returns>
        public bool IsAttackedByWhite(int position)
        {
            var board = this;

            // loop through all the board squares
            // if a square is occupied by a White piece
            // check if it attacks the position
            for (var sqIndex = 0; sqIndex < SquareNo; sqIndex++)
            {
                if (_squares[sqIndex] is WhitePiece && _squares[sqIndex].Attacks(board, sqIndex, position))
                {
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// Checks if a square is attacked by Black side.
        /// </summary>
        /// <param name="position">The position</param>
        /// <returns></returns>
        public bool IsAttackedByBlack(int position)
        {
            var board = this;

            // loop through all the board squares
            // if a square is occupied by a Black piece
            // check if it attacks the position
            for (var sqIndex = 0; sqIndex < SquareNo; sqIndex++)
            {
                if (_squares[sqIndex] is BlackPiece && _squares[sqIndex].Attacks(board, sqIndex, position))
                {
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// Verifies if the White King is in check.
        /// </summary>
        /// <returns></returns>
        public bool WhiteKingInCheck()
        {
            // loop through the squares to find the White King
            // once it is found, checks if its position is attacked by Black
            for (var sqIndex = 0; sqIndex < SquareNo; sqIndex++)
            {
                if (_squares[sqIndex] is WhiteKing) { return IsAttackedByBlack(sqIndex); }
            }
            return false;
        }

        /// <summary>
        /// Verifies if the Black King is in check.
        /// </summary>
        /// <returns></returns>
        public bool BlackKingInCheck()
        {
            // loop through the squares to find the Black King
            // once it is found, checks if its position is attacked by White
            for (var sqIndex = 0; sqIndex < SquareNo; sqIndex++)
            {
                if (_squares[sqIndex] is BlackKing) { return IsAttackedByWhite(sqIndex); }
            }
            return false;
        }

        /// <summary>
        /// Checks if a square is occupied by a side to move piece.
        /// </summary>
        /// <param name="position">The position</param>
        /// <returns></returns>
        public bool IsSideToMovePiece(int position)
        {
            return
                _status.WhiteTurn && _squares[position] is WhitePiece ||
                _status.BlackTurn && _squares[position] is BlackPiece;
        }

        /// <summary>
        /// Gets the rank of this square.
        /// The rank goes from 0 to 7, top to bottom.
        /// </summary>
        /// <param name="position">The position</param>
        /// <returns></returns>
        public static int Rank(int position)
        {
            return position / SideSquareNo;
        }

        /// <summary>
        /// Gets the file of this square.
        /// The file goes from 0 to 7, left to right.
        /// </summary>
        /// <param name="position">The position</param>
        /// <returns></returns>
        public static int File(int position)
        {
            return position % SideSquareNo;
        }

        /// <summary>
        /// Gets a square position, given its rank and file.
        /// </summary>
        /// <param name="rank">The rank number</param>
        /// <param name="file">The file number</param>
        /// <returns></returns>
        public static int Position(int rank, int file)
        {
            return rank * SideSquareNo + file;
        }

        /// <summary>
        /// Checks if the square is white.
        /// </summary>
        /// <param name="position">The position</param>
        /// <returns></returns>
        public static bool IsWhiteSquare(int position)
        {
            return ((File(position) + Rank(position)) & 1) == 0;
        }

        /// <summary>
        /// Gets the hash code of this board.
        /// Needed to check for repetition in a game.
        /// </summary>
        /// <returns></returns>
        public override int GetHashCode()
        {
            var hash = 0;

            // loop through the squares
            for (var sqIndex = 0; sqIndex < SquareNo; sqIndex++)
            {
                // if the square is not empty
                if (this[sqIndex] != null)
                {
                    // XOR the Zobrist key which coresponds to type of piece and for this square
                    // to find the key index, offset the key index of the type by the square number
                    hash ^= this[sqIndex].GetHashCode() + sqIndex;
                }
            }

            return hash;
        }
    }

    /// <summary>
    /// Implements the board status.
    /// </summary>
    public struct BoardStatus
    {
        /// <summary>
        /// True if white is to move, false otherwise.
        /// </summary>
        public bool WhiteTurn { get; set; }

        /// <summary>
        /// True if black is to move, false otherwise.
        /// </summary>
        public bool BlackTurn
        {
            get => !WhiteTurn;
            set => WhiteTurn = !value;
        }

        /// <summary>
        /// White short castling availability.
        /// </summary>
        public bool WhiteCouldCastleShort { get; set; }

        /// <summary>
        /// White long astling availability.
        /// </summary>
        public bool WhiteCouldCastleLong { get; set; }

        /// <summary>
        /// Black short castling availability.
        /// </summary>
        public bool BlackCouldCastleShort { get; set; }

        /// <summary>
        /// Black long castling availability.
        /// </summary>
        public bool BlackCouldCastleLong { get; set; }

        /// <summary>
        /// The position of the en passant target.
        /// </summary>
        public int? EnPassantTarget { get; set; }

        /// <summary>
        /// The ply.
        /// </summary>
        public int Ply { get; set; }

        /// <summary>
        /// The move number.
        /// </summary>
        public int Moves { get; set; }
    }
}
