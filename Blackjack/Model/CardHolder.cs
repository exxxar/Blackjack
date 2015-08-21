namespace Blackjack
{
    /// <summary>
    /// Class representing any card holder (both dealer and player)
    /// </summary>
    public class CardHolder
    {
        private IScoreCounter scoreCounter;

        protected string name;
        protected CardSet hand = new CardSet();
       

        /// <summary>
        /// Parameterized constructor
        /// </summary>
        /// <param name="nm">The name of a card holder</param>
        public CardHolder( string nm = "Unknown" )
        {
            name = nm;
        }

        /// <summary>
        /// Name property
        /// </summary>
        public string Name
        {
            get { return name; }
            set { name = value; }
        }

        /// <summary>
        /// Hand property
        /// </summary>
        public CardSet PlayerHand
        {
            get { return hand; }
            set { hand = value; }
        }

        
        /// <summary>
        /// Method sets the object responsible for score counting
        /// </summary>
        /// <param name="scorer">The object of score counter implementing IScoreCounter</param>
    	public void SetScoreCounter(IScoreCounter scorer)
        {
            scoreCounter = scorer;
        }

        
        /// <summary>
        /// Method counts the total score
        /// </summary>
        /// <returns>The total score in card holder's hand</returns>
	    public byte CountScore()
        {
            return scoreCounter.CountScore( hand );
        }


        /// <summary>
        /// Method for taking new card and adding to card holder's hand
        /// </summary>
        /// <param name="card">The card received by card holder</param>
        public void TakeCard( Card card )
        {
            hand.AddCard( card );

            int score = scoreCounter.CountScore(hand);

            // throw exception if card holder's got blackjack (watch out!)
            if (score == 21)
                throw new BlackjackException(name);

            // throw exception if card holder's got bust (watch out!)
            if (score > 21)
                throw new BustException(name, score);		
        }

	
        /// <summary>
        /// Method clears the card holder's hand
        /// </summary>
        public void ClearHand()
        {
            hand.Clear();
        }
    }
}
