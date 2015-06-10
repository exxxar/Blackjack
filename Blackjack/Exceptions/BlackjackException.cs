using System;


namespace Blackjack
{
    /// <summary>
    /// Reflects the situation when card holder has blackjack
    /// </summary>
    public class BlackjackException: Exception
    {
        /// <summary>
        /// The message of an exception
        /// </summary>
        protected string message;

        /// <summary>
        /// Initializes BlackJackException with the name of a cardholder who's got some blackjack game exception (score=21 by default)
        /// </summary>
        /// <param name="msg">Message by default ("BlackJack!")</param>
        public BlackjackException( string msg = "BlackJack!" )
        {
            message = msg;
        }
    }
}
