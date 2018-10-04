using ChessEngine.Models;

namespace ChessEngine.Extensions
{
    public static class PositionExtension
    {
        /// <summary>
        /// Increments the I property of position and returns a new position with updated props
        /// </summary>
        /// <param name="position"></param>
        /// <param name="offset"></param>
        /// <returns></returns>
        public static Position IncrementI(this Position position, int offset = 1) => new Position
        {
            I = position.I + offset,
            J = position.J
        };

        /// <summary>
        /// Increments the J property of position and returns a new position with updated props
        /// </summary>
        /// <param name="position"></param>
        /// <param name="offset"></param>
        /// <returns></returns>
        public static Position IncrementJ(this Position position, int offset = 1) => new Position
        {
            I = position.I,
            J = position.J + offset
        };
        
        /// <summary>
        /// Decrements the I property of position and returns a new position with updated props
        /// </summary>
        /// <param name="position"></param>
        /// <param name="offset"></param>
        /// <returns></returns>
        public static Position DecrementI(this Position position, int offset = 1) => new Position
        {
            I = position.I - offset,
            J = position.J
        };

        /// <summary>
        /// Decrements the J property of position and returns a new position with updated props
        /// </summary>
        /// <param name="position"></param>
        /// <param name="offset"></param>
        /// <returns></returns>
        public static Position DecrementJ(this Position position, int offset = 1) => new Position
        {
            I = position.I,
            J = position.J - offset
        };
    }
}