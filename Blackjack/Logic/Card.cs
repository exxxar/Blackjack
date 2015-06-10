namespace Blackjack
{
    /// <summary>
    /// Enum for card suits ( ♥, ♦, ♣, ♠ )
    /// </summary>
    public enum Suit : byte
    {
        /// <summary>
        /// suit ♥
        /// </summary>
        Hearts,
        /// <summary>
        /// suit ♦
        /// </summary>
        Diamonds,
        /// <summary>
        /// suit ♣
        /// </summary>
        Clubs,
        /// <summary>
        /// suit ♠
        /// </summary>
        Spades
    };


    /// <summary>
    /// Represents the Card entity.
    /// Card numbers : 0..51	
    ///	<ul>		
    /// <li>0..12  ->  hearts	 suit = 0	</li>
    /// <li>13..25 ->  diamonds	 suit = 1	</li>
    /// <li>26..38 ->  clubs	 suit = 2	</li>
    /// <li>39-51  ->  spades	 suit = 3	</li>
    /// </ul>
    /// <p>Card ranks:</p>
    ///	2..10 J=11 Q=12 K=13 A=14 ( J, Q, K, A will be interpreted differently depending on the game type )
    /// </summary>
    public class Card
    {
        /// <summary>
        /// Card suit
        /// </summary>
        protected Suit suit;

        /// <summary>
        /// Card rank
        /// </summary>
	    protected byte rank;

        /// <summary>
        /// Default constructor creates the card 2♣
        /// </summary>
        public Card() : this( Suit.Clubs, 2 )
        {
        }

        /// <summary>
        /// Parameterized constructor
        /// </summary>
        /// <param name="s">Card suit</param>
        /// <param name="r">Card rank</param>
        public Card(Suit s, byte r)
        {
            suit = s;
            rank = r;
        }

        /// <summary>
        /// Method automatically evaluates correct suit and rank of the Card based on its ordinal number
        /// </summary>
        /// <param name="n">The ordinal number of a Card</param>
        public void setNumber(byte n)
        {
            suit = (Suit)(n / 13);
            rank = (byte)(n % 13 + 2);
        }

        /// <summary>
        /// Method automatically evaluates correct ordinal number of the Card based on its rank and suit
        /// </summary>
        /// <returns></returns>
        public byte getNumber()
        {
            return (byte)((byte)suit * 13 + rank - 2);
        }

        /// <summary>
        /// Card rank property
        /// </summary>
        public byte Rank
        {
            get { return rank; }
            set { rank = value; }
        }

        /// <summary>
        /// Card suit property
        /// </summary>
        public Suit CardSuit
        {
            get { return suit; }
            set { suit = value; }
        }
    }
}
