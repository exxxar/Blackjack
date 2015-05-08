using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Blackjack
{
    /** @enum Suit
    *  @brief Enum for card suits ( ♥, ♦, ♣, ♠ )
    */
    public enum Suit : byte
    {
        Hearts,			/**< value = 0 */
        Diamonds,		/**< value = 1 */
        Clubs,			/**< value = 2 */
        Spades			/**< value = 3 */
    };


    public class Card
    {
        protected Suit suit;
	    protected byte rank;

        public Card() : this( Suit.Clubs, 2 )
        {
        }

        public Card(Suit s, byte r)
        {
            suit = s;
            rank = r;
        }

        public void setNumber(byte n)
        {
            suit = (Suit)(n / 13);
            rank = (byte)(n % 13 + 2);
        }

        public byte getNumber()
        {
            return (byte)((byte)suit * 13 + rank - 2);
        }

        public byte Rank
        {
            get { return rank; }
            set { rank = value; }
        }

        public Suit CardSuit
        {
            get { return suit; }
            set { suit = value; }
        }
    }
}
