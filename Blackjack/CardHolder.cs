using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blackjack
{
    /// <summary>
    /// 
    /// </summary>
    public class CardHolder
    {
        private IScoreCounter scoreCounter;

        protected string name;
        protected CardSet hand = new CardSet();
       

        /// <summary>
        /// 
        /// </summary>
        /// <param name="nm"></param>
        public CardHolder( string nm = "Unknown" )
        {
            name = nm;
        }

        /// <summary>
        /// 
        /// </summary>
        public string Name
        {
            get { return name; }
            set { name = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        public CardSet PlayerHand
        {
            get { return hand; }
            set { hand = value; }
        }

        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="scorer"></param>
    	public void SetScoreCounter(IScoreCounter scorer)
        {
            scoreCounter = scorer;
        }

        
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
	    public byte CountScore()
        {
            return scoreCounter.CountScore( hand );
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="card"></param>
        public void TakeCard( Card card )
        {
            hand.AddCard( card );

            int score = scoreCounter.CountScore(hand);

            if (score == 21)
                throw new BlackjackException(name);		// аж здесь бросаем исключение (аккуратно!)

            if (score > 21)
                throw new BustException(name, score);		// аж здесь бросаем исключение (аккуратно!)
        }

	
        /// <summary>
        /// 
        /// </summary>
        public void ClearHand()
        {
            hand.Clear();
        }
    }
}
