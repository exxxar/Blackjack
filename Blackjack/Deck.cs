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
    public class Deck : CardSet
    {
        /// <summary>
        /// 
        /// </summary>
        public const byte DECK_SIZE = 52;

        
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
	    public void UnpackNew()
        {
            Random r = new Random();

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
