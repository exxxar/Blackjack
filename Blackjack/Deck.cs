using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blackjack
{
    public class Deck
    {
        public const byte DECK_SIZE = 52;

        protected List<Card> cards = new List<Card>();


        public Deck()
        {
            UnpackNew();
        }

	    public Card PopCard()
        {
            Card card = cards.Last();
            cards.Remove( card );
            return card;
        }

        public int GetCardsNumber()
        {
            return cards.Count;
        }

	    public void UnpackNew()
        {
            cards.Clear();
            for (byte i = 0; i < DECK_SIZE; i++)
            {
                Card card = new Card();
                card.setNumber( i );
                cards.Add( card );
            }
        }

	    public void Shuffle()
        {
            UnpackNew();
            Random rand = new Random();
            cards = cards.OrderBy(item => rand.Next()).ToList();
        }
    }
}
