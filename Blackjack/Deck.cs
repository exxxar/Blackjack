using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blackjack
{
    public class Deck
    {
        /// <summary>
        /// 
        /// </summary>
        public const byte DECK_SIZE = 52;

        /// <summary>
        /// 
        /// </summary>
        protected List<Card> cards = new List<Card>();


        /// <summary>
        /// 
        /// </summary>
        public Deck()
        {
            UnpackNew();
        }


        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
	    public Card PopCard()
        {
            Card card = cards.Last();
            cards.Remove( card );
            return card;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public int GetCardsNumber()
        {
            return cards.Count;
        }


        /// <summary>
        /// 
        /// </summary>
	    public void UnpackNew()
        {
            Random r = new Random();

            cards.Clear();
            for (byte i = 0; i < DECK_SIZE; i++)
            {
                Card card = new Card();
                card.setNumber( i );

                // for testing blackjacks:
                //card.setNumber((byte)(r.Next() % 3 + 49) );

                cards.Add( card );
            }
        }


        /// <summary>
        /// 
        /// </summary>
	    public void Shuffle()
        {
            UnpackNew();

            Random rand = new Random( DateTime.Now.Second );
            cards = cards.OrderBy(item => rand.Next() % 52).ToList();
        }
    }
}
