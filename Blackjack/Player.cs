using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blackjack
{

    public enum PlayerState
    {
        HIT,
        STAND,
        DOUBLE,
        BUST,
        BLACKJACK
    }

    public enum PlayerResult
    {
        WIN,
        LOSE,
        STAY,
        UNDEFINED
    }


    public class Player : CardHolder
    {
        protected int money;
	    protected int stake;
        protected PlayerResult playerResult;

        public Player( string nm = "Unknown", int mon = 1000 ) : base(nm)
        {
            money = mon;
            PlayResult = PlayerResult.UNDEFINED;
        }

	    public int Stake
        {
            get { return stake; }
            set { stake = value; }
        }

        public int Money
        {
            get { return money; }
            set { money = value; }
        }

        public PlayerResult PlayResult
        {
            get { return playerResult; }
            set { playerResult = value; }
        }

        public bool CanDoubleStake()
        {
            return stake * 2 <= money;
        }

        public void WinStake()
        {
            money += stake;
            PlayResult = PlayerResult.WIN;
        }

        public void LoseStake()
        {
            money -= stake;
            PlayResult = PlayerResult.LOSE;
        }

        public void BonusStake(double coeff = 1.5)
        {
            stake = (int)(stake * coeff);
        }
    }
}
