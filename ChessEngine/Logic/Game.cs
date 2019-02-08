using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using ChessEngine.Interfaces;
using ChessEngine.Models.Events;
using ChessEngine.Models.Interfaces;
using ChessEngine.Models.Pieces;
using ChessEngine.Models.Pieces.Black;
using ChessEngine.Models.Pieces.Moves;
using ChessEngine.Models.Pieces.White;

namespace ChessEngine.Logic
{
    /// <summary>
    /// Implements a chess game.
    /// </summary>
    public class Game : IGame
    {
        /// <summary>
        /// Current board configuration.
        /// </summary>
        private Board _currentBoard;
        
        /// <summary>
        /// Current board index.
        /// </summary>
        private int _currentBoardIndex;
        
        /// <summary>
        /// Move history.
        /// </summary>
        private readonly List<Move> _moveHistory;
        
        /// <summary>
        /// Status of the game.
        /// </summary>
        private GameStatus _status;
        
        /// <summary>
        /// List of the possible moves of the current board.
        /// </summary>
        private readonly List<Move> _possibleMoves;
        
        /// <summary>
        /// Contains pairs consisting of a board hash and the frequency of that board in the history of the game.
        /// </summary>
        private readonly Dictionary<int, int> _historyHashes;

        /// <summary>
        /// Moving event.
        /// </summary>
        public event EventHandler<CancelMoveEventArgs> Moving;
        /// <summary>
        /// Moved event.
        /// </summary>
        public event EventHandler<MoveEventArgs> Moved;
        
        /// <summary>
        /// Going forward event.
        /// </summary>
        public event EventHandler<CancelMoveEventArgs> GoingForward;
        
        /// <summary>
        /// Gone forward event.
        /// </summary>
        public event EventHandler<MoveEventArgs> GoneForward;
        
        /// <summary>
        /// Going back event.
        /// </summary>
        public event EventHandler<CancelMoveEventArgs> GoingBack;
        
        /// <summary>
        /// Gone back event.
        /// </summary>
        public event EventHandler<MoveEventArgs> GoneBack;
        
        /// <summary>
        /// Modifying event.
        /// </summary>
        public event EventHandler<CancelEventArgs> Modifying;
        
        /// <summary>
        /// Modified event.
        /// </summary>
        public event EventHandler Modified;
        
        /// <summary>
        /// Loading event.
        /// </summary>
        public event EventHandler<CancelEventArgs> Loading;
        
        /// <summary>
        /// Board configuration loaded event.
        /// </summary>
        public event EventHandler BoardConfigurationLoaded;
        
        /// <summary>
        /// Game board configuration loaded event.
        /// </summary>
        public event EventHandler GameBoardConfigurationLoaded;
        
        /// <summary>
        /// Game move section loaded event.
        /// </summary>
        public event EventHandler GameMoveSectionLoaded;

        /// <summary>
        /// Raises the Moving event.
        /// </summary>
        /// <param name="e"></param>
        protected virtual void OnMoving(CancelMoveEventArgs e)
        {
            Moving?.Invoke(this, e);
        }
        /// <summary>
        /// Raises the Moved event.
        /// </summary>
        /// <param name="e"></param>
        protected virtual void OnMoved(MoveEventArgs e)
        {
            Moved?.Invoke(this, e);
        }
        /// <summary>
        /// Raises the GoingForward event.
        /// </summary>
        /// <param name="e"></param>
        protected virtual void OnGoingForward(CancelMoveEventArgs e)
        {
            GoingForward?.Invoke(this, e);
        }
        /// <summary>
        /// Raises the GoneForward event.
        /// </summary>
        /// <param name="e"></param>
        protected virtual void OnGoneForward(MoveEventArgs e)
        {
            GoneForward?.Invoke(this, e);
        }
        /// <summary>
        /// Raises the GoingBack event.
        /// </summary>
        /// <param name="e"></param>
        protected virtual void OnGoingBack(CancelMoveEventArgs e)
        {
            GoingBack?.Invoke(this, e);
        }
        /// <summary>
        /// Raises the GoneBack event.
        /// </summary>
        /// <param name="e"></param>
        protected virtual void OnGoneBack(MoveEventArgs e)
        {
            GoneBack?.Invoke(this, e);
        }

        /// <summary>
        /// Raises the Modifying event.
        /// </summary>
        /// <param name="e"></param>
        protected virtual void OnModifying(CancelEventArgs e)
        {
            Modifying?.Invoke(this, e);
        }
        /// <summary>
        /// Raises the Modified event.
        /// </summary>
        /// <param name="e"></param>
        protected virtual void OnModified(EventArgs e)
        {
            Modified?.Invoke(this, e);
        }

        /// <summary>
        /// Raises the Loading event.
        /// </summary>
        /// <param name="e"></param>
        protected internal virtual void OnLoading(CancelEventArgs e)
        {
            Loading?.Invoke(this, e);
        }
        
        /// <summary>
        /// Raises the BoardConfigurationLoaded event.
        /// </summary>
        /// <param name="e"></param>
        protected internal virtual void OnBoardConfigurationLoaded(EventArgs e)
        {
            BoardConfigurationLoaded?.Invoke(this, e);
        }
        
        /// <summary>
        /// Raises the GameBoardConfigurationLoaded event.
        /// </summary>
        /// <param name="e"></param>
        protected internal virtual void OnGameBoardConfigurationLoaded(EventArgs e)
        {
            GameBoardConfigurationLoaded?.Invoke(this, e);
        }
        
        /// <summary>
        /// Raises the GameMoveSectionLoaded event.
        /// </summary>
        /// <param name="e"></param>
        protected internal virtual void OnGameMoveSectionLoaded(EventArgs e)
        {
            GameMoveSectionLoaded?.Invoke(this, e);
        }

        /// <summary>
        /// Promotion delegate.
        /// </summary>
        public PromotionHandler Promote { get; set; }

        /// <summary>
        /// True if the current board configuration is the last in history, false otherwise.
        /// </summary>
        public bool IsLast => _currentBoardIndex == _moveHistory.Count;

        /// <summary>
        /// True if the current board configuration is the first in history, false otherwise.
        /// </summary>
        public bool IsFirst => _currentBoardIndex == 0;

        /// <summary>
        /// True if the game is ended, false otherwise.
        /// </summary>
        public bool IsEnded => _status != GameStatus.Normal && _status != GameStatus.Check;

        /// <summary>
        /// True if the game is initialized, false otherwise.
        /// </summary>
        public bool IsInitialized => _currentBoard != null;

        /// <summary>
        /// Gets or sets the current board configuration.
        /// </summary>
        public Board CurrentBoard
        {
            get => _currentBoard;
            internal set
            {
                _moveHistory.Clear();
                _historyHashes.Clear();

                _currentBoard = value;
                _currentBoardIndex = 0;

                GenerateMoves();
                AddHistoryHash();
                SetStatus();
            }
        }

        /// <summary>
        /// Gets the list of possible moves.
        /// </summary>
        private IEnumerable<Move> PossibleMoves => _possibleMoves;

        /// <summary>
        /// Gets the game status.
        /// </summary>
        public GameStatus Status => _status;

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="board"></param>
        public Game(Board board)
        {
            _moveHistory = new List<Move>();
            _possibleMoves = new List<Move>();
            _historyHashes = new Dictionary<int, int>();

            CurrentBoard = board;
        }

        /// <summary>
        /// Gets the valid move based on starting square and ending square. 
        /// If the move is a promotion move, it will set the promotion type.
        /// </summary>
        /// <param name="from">The starting square.</param>
        /// <param name="to">The ending square.</param>
        /// <param name="promotionType">the promotion type.</param>
        /// <returns></returns>
        public Move GetMove(int from, int to, Type promotionType)
        {
            // loop through possible moves until a move with the same starting and ending square is found
            foreach (var move in PossibleMoves)
            {
                if (move.From == from && move.To == to)
                {
                    if (move is PromotionMove promotionMove)
                    {
                        promotionMove.PromotionType = promotionType;
                    }

                    return move;
                }
            }

            return null;
        }

        /// <summary>
        /// Gets a move which if it is made, it will probably end the game in a draw by repetition.
        /// </summary>
        /// <returns></returns>
        public Move RepetitiveMoveCandidate =>
            _currentBoardIndex >= 7 &&
            _moveHistory[_currentBoardIndex - 1].From == _moveHistory[_currentBoardIndex - 5].From &&
            _moveHistory[_currentBoardIndex - 1].To == _moveHistory[_currentBoardIndex - 5].To &&
            _moveHistory[_currentBoardIndex - 2].From == _moveHistory[_currentBoardIndex - 6].From &&
            _moveHistory[_currentBoardIndex - 2].To == _moveHistory[_currentBoardIndex - 6].To &&
            _moveHistory[_currentBoardIndex - 3].From == _moveHistory[_currentBoardIndex - 7].From &&
            _moveHistory[_currentBoardIndex - 3].To == _moveHistory[_currentBoardIndex - 7].To
                ?
                GetMove(_moveHistory[_currentBoardIndex - 4].From, _moveHistory[_currentBoardIndex - 4].To, null)
                :
                null;

        /// <summary>
        /// Makes a move.
        /// If the move is illegal or it's null, throws an Argument(Null)Exception.
        /// </summary>
        /// <param name="move">The move</param>
        public void Make(Move move)
        {
            // check to see if the move it's not null and it's valid
            if (move == null) { throw new ArgumentNullException(); }
            var findMove = PossibleMoves.Any(m => move.From == m.From && move.To == m.To);
            if (!findMove) { throw new ArgumentException(); }

            // build the event args
            var moveEventArgs = new CancelMoveEventArgs(move, _currentBoardIndex - 1);

            // raise the Moving event
            OnMoving(moveEventArgs);

            // if the move was cancelled
            if (moveEventArgs.Cancel) { return; }

            // remove the moves after current board index (if any)
            if (!IsLast)
            {
                _moveHistory.RemoveRange(_currentBoardIndex, _moveHistory.Count - _currentBoardIndex);
            }

            // if this is a promotion, the promotion is not set and the promotion delegate is not null, call the promotion delegate
            if (move is PromotionMove promotionMove && promotionMove.PromotionType == null && Promote != null)
            {
                promotionMove.PromotionType = Promote();
            }

            // make the move
            move.Make(_currentBoard);

            // add the current hash to the history
            AddHistoryHash();

            // generate the possible moves
            GenerateMoves();

            // set the status of the game
            SetStatus();

            // increment the current board index
            _currentBoardIndex++;

            // add move to history
            _moveHistory.Add(move);

            // raise the Moved event
            OnMoved(new MoveEventArgs(move, _currentBoardIndex - 1));
        }

        /// <summary>
        /// Go to the next board configuration in history.
        /// </summary>
        public void Next()
        {
            // if the game is not the at the end
            if (!IsLast)
            {
                var move = _moveHistory[_currentBoardIndex];

                // build the event args
                var moveEventArgs = new CancelMoveEventArgs(move, _currentBoardIndex - 1);

                // raise the GoingForward event
                OnGoingForward(moveEventArgs);

                // if the operation was cancelled
                if (moveEventArgs.Cancel) { return; }

                // make the move
                move.Make(_currentBoard);

                // add the current board hash to the history
                AddHistoryHash();

                // generate the possible moves
                GenerateMoves();

                // set the status of the game
                SetStatus();

                // increment the current board index
                _currentBoardIndex++;

                // raise the GoneForward event
                OnGoneForward(new MoveEventArgs(move, _currentBoardIndex - 1));
            }
        }

        /// <summary>
        /// Go to the previous board configuration in history.
        /// </summary>
        public void Previous()
        {
            // if the game is not at the begining
            if (!IsFirst)
            {
                var move = _moveHistory[_currentBoardIndex - 1];

                // build the event args
                var moveEventArgs = new CancelMoveEventArgs(move, _currentBoardIndex - 1);

                // raise the GoingBack event
                OnGoingBack(moveEventArgs);

                // if the operation was cancelled
                if (moveEventArgs.Cancel) { return; }

                // remove the current board hash from the history
                RemoveHistoryHash();

                // take back the move
                move.TakeBack(_currentBoard);

                // generate the possible moves
                GenerateMoves();

                // set the status of the game
                SetStatus();

                // decrement the current board index
                _currentBoardIndex--;

                // raise the GoneBack event
                OnGoneBack(new MoveEventArgs(move, _currentBoardIndex - 1));
            }
        }

        /// <summary>
        /// Go to the last board configuration in history.
        /// </summary>
        public void GoToLast()
        {
            // build the event args
            var emptyArgs = new CancelEventArgs();

            // raise the Modified event
            OnModifying(emptyArgs);

            // if the operation was cancelled
            if (emptyArgs.Cancel) { return; }

            // go to the last board configuration step by step
            while (!IsLast)
            {
                // make the move and increment the current board index
                _moveHistory[_currentBoardIndex++].Make(_currentBoard);

                // add the current board hash to history
                AddHistoryHash();

                // generate the possible moves
                GenerateMoves();

                // set the game status
                SetStatus();
            }

            // raise the Modified event
            OnModified(EventArgs.Empty);
        }

        /// <summary>
        /// Go to the first board configuration in history.
        /// </summary>
        public void GoToFirst()
        {
            // build the event args
            var emptyArgs = new CancelEventArgs();

            // raise the Modified event
            OnModifying(emptyArgs);

            // if the operation was cancelled
            if (emptyArgs.Cancel) { return; }

            // go to the first board configuration step by step
            while (!IsFirst)
            {
                // remove the board hash from history
                RemoveHistoryHash();

                // take back the move and decrement the current board index
                _moveHistory[--_currentBoardIndex].TakeBack(_currentBoard);

                // generate the possible moves
                GenerateMoves();

                // set the game status
                SetStatus();
            }

            // raise the Modified event
            OnModified(EventArgs.Empty);
        }

        /// <summary>
        /// Generates the possible moves of the current board.
        /// </summary>
        private void GenerateMoves()
        {
            // clear the list
            _possibleMoves.Clear();

            // loop the starting squares through all the squares 
            for (var fromIndex = 0; fromIndex < Board.SquareNo; fromIndex++)
            {
                // if it's a side to move piece on this square
                if (_currentBoard.IsSideToMovePiece(fromIndex))
                {
                    // loop the ending squares through all the squares 
                    for (var toIndex = 0; toIndex < Board.SquareNo; toIndex++)
                    {
                        // try to generate the move
                        var move = _currentBoard[fromIndex].GenerateMove(_currentBoard, fromIndex, toIndex);
                        if (move != null) { _possibleMoves.Add(move); }
                    }
                }
            }
        }

        /// <summary>
        /// Set the game status.
        /// </summary>
        private void SetStatus()
        {
            // if there are no moves it's checkmate or stalemate
            if (_possibleMoves.Count == 0)
            {
                _status = IsCheck() ? GameStatus.Checkmate : GameStatus.Stalemate;
                return;
            }

            // if it's draw by insufficient material
            if (IsDrawInsufficientMaterial())
            {
                _status = GameStatus.DrawInsufficientMaterial;
                return;
            }

            // if it's draw by 50-move rule
            if (_currentBoard.Status.Ply >= 100)
            {
                _status = GameStatus.Draw50Move;
                return;
            }

            // if it's draw by repetition
            if (_historyHashes.ContainsValue(3))
            {
                _status = GameStatus.DrawRepetition;
                return;
            }

            // if it's check or normal status
            _status = IsCheck() ? GameStatus.Check : GameStatus.Normal;
        }

        /// <summary>
        /// Adds the current board hash to history.
        /// </summary>
        private void AddHistoryHash()
        {
            var hash = _currentBoard.GetHashCode();

            // if the hash exists increment the frequency, otherwise add the hash with frequency 1
            _historyHashes[hash] = _historyHashes.ContainsKey(hash) ? _historyHashes[hash] + 1 : 1;
        }

        /// <summary>
        /// Removes the current board hash from history.
        /// </summary>
        private void RemoveHistoryHash()
        {
            var hash = _currentBoard.GetHashCode();
            var freq = _historyHashes[hash];

            // if the frequency is more than 1 decrement it, otherwise remove the hash
            if (freq > 1)
            {
                _historyHashes[hash] = freq - 1;
            }
            else
            {
                _historyHashes.Remove(hash);
            }
        }

        /// <summary>
        /// Gets the captured pieces.
        /// Returns a dictionary with (piece type, frequency) pairs
        /// </summary>
        /// <returns></returns>
        public Dictionary<Type, int> GetCapturedPieces()
        {
            var captures = new Dictionary<Type, int>();

            // loop through the move history and get the captured pieces from each move until the current board is reached
            for (var moveIndex = 0; moveIndex < _currentBoardIndex; moveIndex++)
            {
                Piece capture;
                if ((capture = _moveHistory[moveIndex].Capture) == null) continue;
                var captureType = capture.GetType();

                // if the capture type exists increment the frequency, otherwise add the type with frequency 1
                captures[captureType] = captures.ContainsKey(captureType) ? captures[captureType] + 1 : 1;
            }

            return captures;
        }

        /// <summary>
        /// Verifies the current board for check.
        /// </summary>
        /// <returns></returns>
        private bool IsCheck()
        {
            return _currentBoard.Status.WhiteTurn ? _currentBoard.WhiteKingInCheck() : _currentBoard.BlackKingInCheck();
        }

        /// <summary>
        /// Verifies the current board for draw by insufficient material.
        /// </summary>
        /// <returns></returns>
        private bool IsDrawInsufficientMaterial()
        {
            // "N" is true if there is one White Knight
            // "B" is true if there is one White Bishop
            // "BW" is true if the White Bishop is on a white square
            // "n" is true if there is one Black Knight
            // "b" is true if there is one Black Bishop
            // "bw" is true if the Black Bishop is on a white square
            bool N, B, BW, n, b, bw;
            N = B = BW = n = b = bw = false;

            // loop through the squares
            for (var sqIndex = 0; sqIndex < Board.SquareNo; sqIndex++)
            {
                if (_currentBoard[sqIndex] != null)
                {
                    if (_currentBoard[sqIndex] is WhiteKnight)
                    {
                        // if there is more than one White Knight there is no draw by insufficient material
                        if (N) { return false; }

                        N = true;

                        continue;
                    }

                    if (_currentBoard[sqIndex] is WhiteBishop)
                    {
                        // if there is more than one White Bishop there is no draw by insufficient material
                        if (B) { return false; }

                        B = true;

                        // remember the Bishop square colour
                        BW = Board.IsWhiteSquare(sqIndex);

                        continue;
                    }

                    if (_currentBoard[sqIndex] is BlackKnight)
                    {
                        // if there is more than one Black Knight there is no draw by insufficient material
                        if (n) { return false; }

                        n = true;

                        continue;
                    }

                    if (_currentBoard[sqIndex] is BlackBishop)
                    {
                        // if there are more than one Black Bishop there is no draw by insufficient material
                        if (b) { return false; }

                        b = true;

                        // remember the Bishop square colour
                        bw = Board.IsWhiteSquare(sqIndex);

                        continue;
                    }

                    // there will be always the King
                    if (_currentBoard[sqIndex] is IKing) { continue; }

                    // if there is a piece which is not Bishop, Knight or King there is no draw by insufficient material
                    return false;
                }
            }

            // if there are only the kings
            if (!B && !b && !N && !n)
            {
                return true;
            }

            // if there are only the kings and one bishop on one side
            if (b && !B && !N && !n || B && !b && !n && !N)
            {
                return true;
            }

            // if there are only the kings and one knight on one side
            if (n && !N && !B && !b || N && !n && !b && !B)
            {
                return true;
            }

            // if there are only the kings and one bishop on each side, both of them on squares with the same colour
            if (B && b && !N && !n && bw == BW)
            {
                return true;
            }

            return false;
        }
    }

    /// <summary>
    /// Game status enumeration.
    /// </summary>
    public enum GameStatus { Normal, Check, Checkmate, Stalemate, Draw50Move, DrawInsufficientMaterial, DrawRepetition };

    /// <summary>
    /// Promotion delegate.
    /// </summary>
    public delegate Type PromotionHandler();
}

