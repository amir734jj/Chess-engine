using ChessEngine.Models.Interfaces;

namespace ChessEngine.Models
{
    public class Board
    {
        public IPiece[][] Matrix { get; set; }
    }
}