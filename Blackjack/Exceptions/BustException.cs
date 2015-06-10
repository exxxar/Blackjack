namespace Blackjack
{
    /// <summary>
    /// Reflects the situation when a card holder gets busted
    /// </summary>
    public class BustException : BlackjackException
    {
        // The name of a cardholder who's been busted
        private string playerName;
        
        // The score of a cardholder who's been busted
        private int score;


        /// <summary>
        /// Name property
        /// </summary>
        public string PlayerName
        {
            get { return playerName; }
            set { playerName = value; }
        }

        /// <summary>
        /// Initializes BustException with the name and the total score of a cardholder who's been busted
        /// </summary>
        /// <param name="name">Card holder's name</param>
        /// <param name="s">Card holder's score</param>
        public BustException( string name="Unknown", int s = 22 )
        {
            score = s;
            message = name + " : bust!\n Score: " + s;
        }
    }
}
