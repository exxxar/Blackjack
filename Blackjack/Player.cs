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
    public enum PlayerState
    {
        HIT,
        STAND,
        DOUBLE,
        BUST,
        BLACKJACK,
    }

    /// <summary>
    /// 
    /// </summary>
    public enum PlayerResult
    {
        WIN,
        LOSE,
        STAY,
        UNDEFINED
    }

    /// <summary>
    /// 
    /// </summary>
    public class Player : CardHolder
    {
        protected int money;
	    protected int stake;
        protected PlayerResult playerResult;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="nm"></param>
        /// <param name="mon"></param>
        public Player( string nm = "Unknown", int mon = 1000 ) : base(nm)
        {
            money = mon;
            PlayResult = PlayerResult.UNDEFINED;
        }

        /// <summary>
        /// 
        /// </summary>
	    public int Stake
        {
            get { return stake; }
            set { stake = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        public int Money
        {
            get { return money; }
            set { money = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        public PlayerResult PlayResult
        {
            get { return playerResult; }
            set { playerResult = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public bool CanDoubleStake()
        {
            return stake * 2 <= money;
        }

        /// <summary>
        /// 
        /// </summary>
        public void WinStake()
        {
            money += stake;
            PlayResult = PlayerResult.WIN;
        }

        /// <summary>
        /// 
        /// </summary>
        public void LoseStake()
        {
            money -= stake;
            PlayResult = PlayerResult.LOSE;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="coeff"></param>
        public void BonusStake(double coeff = 1.5)
        {
            stake = (int)(stake * coeff);
        }
    }
}
