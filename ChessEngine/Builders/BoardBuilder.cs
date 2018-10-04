using System;
using System.Collections.Generic;
using System.Linq;
using ChessEngine.Interfaces;
using ChessEngine.Models;
using ChessEngine.Models.Constants;
using ChessEngine.Models.Enums;
using ChessEngine.Models.Interfaces;
using ChessEngine.Models.Pieces;

namespace ChessEngine.Builders
{
    public class BoardBuilder : IBoardBuilder
    {
        public static IBoardBuilder New = new BoardBuilder();
        
        /// <summary>
        /// Creates a new instance of a board
        /// </summary>
        /// <returns></returns>
        public Board NewEmptyBoard()
        {
            var matrix = Enumerable.Range(0, BoardConstants.Dimension)
                .Select(x => Setup(x).ToArray())
                .ToArray();

            var board = new Board {Matrix = matrix};
            
            return board;
        }

        /// <summary>
        /// Sets up the initial board
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        private static IEnumerable<IPiece> Setup(int index)
        {
            switch (index)
            {
                case 0:
                    return new List<IPiece>
                    {
                        new Rook(TeamEnum.Black, new Position {I = index, J = 0}),
                        new Bishop(TeamEnum.Black, new Position {I = index, J = 1}),
                        new Knight(TeamEnum.Black, new Position {I = index, J = 2}),
                        new Knight(TeamEnum.Black, new Position {I = index, J = 3}),
                        new Queen(TeamEnum.Black, new Position {I = index, J = 4}),
                        new King(TeamEnum.Black, new Position {I = index, J = 4}),
                        new Bishop(TeamEnum.Black, new Position {I = index, J = 5}),
                        new Knight(TeamEnum.Black, new Position {I = index, J = 6}),
                        new Rook(TeamEnum.Black, new Position {I = index, J = 7}),
                    };
                case 1:
                    return Enumerable.Range(0, BoardConstants.Dimension)
                        .Select(j => new Pawn(
                            TeamEnum.Black,
                            new Position {I = index, J = j})
                        );
                case 2:
                    return new IPiece[BoardConstants.Dimension];
                case 3:
                    return new IPiece[BoardConstants.Dimension];
                case 4:
                    return new IPiece[BoardConstants.Dimension];
                case 5:
                    return new IPiece[BoardConstants.Dimension];
                case 6:
                    return Enumerable.Range(0, BoardConstants.Dimension).Select(j => new Pawn(
                        TeamEnum.White,
                        new Position {I = index, J = j}));
                case 7:
                    return new List<IPiece>
                    {
                        new Rook(TeamEnum.White, new Position {I = index, J = 0}),
                        new Bishop(TeamEnum.White, new Position {I = index, J = 1}),
                        new Knight(TeamEnum.White, new Position {I = index, J = 2}),
                        new Knight(TeamEnum.White, new Position {I = index, J = 3}),
                        new Queen(TeamEnum.White, new Position {I = index, J = 4}),
                        new King(TeamEnum.White, new Position {I = index, J = 4}),
                        new Bishop(TeamEnum.White, new Position {I = index, J = 5}),
                        new Knight(TeamEnum.White, new Position {I = index, J = 6}),
                        new Rook(TeamEnum.White, new Position {I = index, J = 7}),
                    };
                default:
                    throw new Exception("Error: cannot arrange out of range pieces");
            }
        }
    }
}