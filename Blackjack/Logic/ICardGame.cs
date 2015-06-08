using System;


namespace Blackjack
{
    /// <summary>
    /// Interface for classes implementing different card games
    /// </summary>
    public interface ICardGame
    {
        /// <summary>
        /// Method starts new game
        /// </summary>
        void Shuffle();

        /// <summary>
        /// Method analyzes the results of a game
        /// </summary>
        /// <param name="nPlayer">The number of player among players</param>
        /// <returns>The resulting value depending on a strategy used to analyze the results of a game</returns>
        int PlayResults( int nPlayer ); 
    }
}
