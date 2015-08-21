namespace Blackjack
{
    /// <summary>
    /// Interface for classes implementing different score counting techniques (for cardsets)
    /// </summary>
    public interface IScoreCounter
    {
        /// <summary>
        /// Calculates the total number of points according to the cards in a cardset 
        /// </summary>
        /// <param name="hand">A cardset</param>
        /// <returns>The score of a cardset</returns>
        byte CountScore(CardSet hand);
    }
}
