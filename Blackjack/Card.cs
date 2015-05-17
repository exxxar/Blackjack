using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Blackjack
{
    /// <summary>
    /// Enum for card suits ( ♥, ♦, ♣, ♠ )
    /// </summary>
    public enum Suit : byte
    {
        Hearts,			/**< value = 0 */
        Diamonds,		/**< value = 1 */
        Clubs,			/**< value = 2 */
        Spades			/**< value = 3 */
    };


    /// <summary>
    /// 
    /// </summary>
    public class Card
    {
        protected Suit suit;
	    protected byte rank;

        /// <summary>
        /// 
        /// </summary>
        public Card() : this( Suit.Clubs, 2 )
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="s"></param>
        /// <param name="r"></param>
        public Card(Suit s, byte r)
        {
            suit = s;
            rank = r;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="n"></param>
        public void setNumber(byte n)
        {
            suit = (Suit)(n / 13);
            rank = (byte)(n % 13 + 2);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public byte getNumber()
        {
            return (byte)((byte)suit * 13 + rank - 2);
        }

        /// <summary>
        /// 
        /// </summary>
        public byte Rank
        {
            get { return rank; }
            set { rank = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        public Suit CardSuit
        {
            get { return suit; }
            set { suit = value; }
        }
    }
}
