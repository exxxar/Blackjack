using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blackjack
{
    public class CardHolder
    {
        private IScoreCounter scoreCounter;

        protected string name;

        protected Hand hand = new Hand();

        public CardHolder( string nm = "Unknown" )
        {
            name = nm;
        }

        public string Name
        {
            get { return name; }
            set { name = value; }
        }

        public Hand PlayerHand
        {
            get { return hand; }
            set { hand = value; }
        }
        	    
	    public void SetScoreCounter(IScoreCounter scorer)
        {
            scoreCounter = scorer;
        }

	    public byte CountScore()
        {
            return scoreCounter.CountScore( hand );
        }
		 
	    public void TakeCard( Deck[] decks )
        {
            int nDecks = decks.Length;
            Random rand = new Random();

            Card cardOutOfDeck = decks[rand.Next(nDecks)].PopCard();
            hand.AddCard(cardOutOfDeck);

            int score = scoreCounter.CountScore(hand);
            if (score > 21)
                throw new BustException(name, score);		// аж здесь бросаем исключение (аккуратно!)

        }
	
        public void ClearHand()
        {
            hand.Clear();
        }
    }
}
