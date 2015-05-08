using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blackjack
{
    public class Hand
    {
        protected List<Card> cards = new List<Card>();


        public void AddCard( Card card )
        {
            cards.Add( card );
        }

	    public Card this [int idx]
        {
            get { return cards[idx]; }
            set { cards[idx] = value; }
        }

	    public int GetCardsNumber()
        {
            return cards.Count;
        }

	    public void Clear()
        {
            cards.Clear();
        }
    }
}
