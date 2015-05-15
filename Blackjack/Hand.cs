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
    public class Hand
    {
        protected List<Card> cards = new List<Card>();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="card"></param>
        public void AddCard( Card card )
        {
            cards.Add( card );
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="idx"></param>
        /// <returns></returns>
	    public Card this [int idx]
        {
            get { return cards[idx]; }
            set { cards[idx] = value; }
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
	    public void Clear()
        {
            cards.Clear();
        }
    }
}
