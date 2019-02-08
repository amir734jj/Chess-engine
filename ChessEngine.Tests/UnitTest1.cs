using System;
using ChessEngine.Logic;
using ChessEngine.Models;
using ChessEngine.Models.Pieces;
using Xunit;

namespace ChessEngine.Tests
{
    public class UnitTest1
    {
        [Fact]
        public void Test1()
        {
            var game = new Game(Board.GetStartingBoard());

            var res = game.GetMove(0, 0, null);
            
            Assert.True(false);
        }
    }
}