namespace Blackjack
{
    /// <summary>
    /// Class for counting the total score in a cardset
    /// </summary>
    public class BlackjackScoreCounter: IScoreCounter
    {
        /// <summary>
        /// Main method counts the total score
        /// </summary>
        /// <param name="hand">The set of cards for which the score is calculated</param>
        /// <returns>Total score in the cardset</returns>
        public byte CountScore(CardSet hand)
        {
            byte score = 0;

            byte aces = 0;

            for (int i = 0; i < hand.GetCardsNumber(); i++)
            {
                byte rank = hand[i].Rank;

                if (rank > 10)
                {
                    if (rank == 14)
                    {
                        rank = 11;
                        aces++;
                    }
                    else
                    {
                        rank = 10;
                    }
                }

                score += rank;
            }

            while (score > 21 && aces > 0)
            {
                score -= 10;
                aces--;
            }

            return score;
        }
    }
}
