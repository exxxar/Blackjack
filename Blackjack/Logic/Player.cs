namespace Blackjack
{
    /// <summary>
    /// The enum for possible states players can have
    /// </summary>
    public enum PlayerState
    {
        HIT,
        STAND,
        DOUBLE,
        BUST,
        BLACKJACK,
    }

    /// <summary>
    /// The enum for possible results of a game for a player
    /// </summary>
    public enum PlayerResult
    {
        WIN,
        LOSE,
        STAY,
        UNDEFINED
    }


    /// <summary>
    /// The Blackjack player class
    /// </summary>
    public class Player : CardHolder
    {
        /// <summary>
        /// Parameterized constructor
        /// </summary>
        /// <param name="nm">Player's name ("Unknown" is default)</param>
        /// <param name="mon">Player's money (1000$ is default)</param>
        public Player( string nm = "Unknown", int mon = 1000 ) : base(nm)
        {
            Money = mon;
            PlayResult = PlayerResult.UNDEFINED;
        }

        /// <summary>
        /// Player's stake
        /// </summary>
        public int Stake
        {
            get; set;
        }

        /// <summary>
        /// The amount of money a player currently has
        /// </summary>
        public int Money
        {
            get; set;
        }

        /// <summary>
        /// The current result of a game for a player
        /// </summary>
        public PlayerResult PlayResult
        {
            get; set;
        }


        /// <summary>
        /// Method checks whether a player has enough money to double down 
        /// </summary>
        /// <returns>true if he/she does; false - otherwise</returns>
        public bool CanDoubleStake()
        {
            return Stake * 2 <= Money;
        }

        /// <summary>
        /// Method makes a player win his/her stake and win the game
        /// </summary>
        public void WinStake()
        {
            Money += Stake;
            PlayResult = PlayerResult.WIN;
        }

        /// <summary>
        /// Method makes a player lose his/her stake and lose the game
        /// </summary>
        public void LoseStake()
        {
            Money -= Stake;
            PlayResult = PlayerResult.LOSE;
        }

        /// <summary>
        /// Method sets the bonus stake (multiplies by factor of a <paramref name="coeff"/>)
        /// </summary>
        /// <param name="coeff">Bonus coefficient (1.5 is default)</param>
        public void BonusStake(double coeff = 1.5)
        {
            Stake = (int)(Stake * coeff);
        }
    }
}
