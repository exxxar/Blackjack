using System;
using System.Linq;


namespace Blackjack
{
    /// <summary>
    /// Deck class
    /// </summary>
    public class Deck : CardSet
    {
        /// <summary>
        /// The number of cards in each deck is 52 by default (from 2♥ to A♠)
        /// </summary>
        public const byte DECK_SIZE = 52;

        
        /// <summary>
        /// Default constructor
        /// </summary>
        public Deck()
        {
            UnpackNew();
        }

        
        /// <summary>
        /// Method fills a deck with cards sequentially from 2♥ to A♠
        /// </summary>
	    public void UnpackNew()
        {
            cards.Clear();
            for (byte i = 0; i < DECK_SIZE; i++)
            {
                Card card = new Card();
                card.setNumber( i );

                // for testing blackjacks (only Q,K and A are generated)
                //card.setNumber((byte)(r.Next() % 3 + 49) );

                AddCard( card );
            }
        }


        /// <summary>
        /// Method shuffles new deck
        /// </summary>
	    public void Shuffle( int nDeck )
        {
            UnpackNew();
            Random rand = new Random( nDeck );
            cards = cards.OrderBy(item => rand.Next()).ToList();
        }
    }
}
