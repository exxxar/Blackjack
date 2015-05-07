using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blackjack
{
    public class Player : CardHolder
    {
        protected int money;
	    protected int stake;

        public Player( string nm = "Unknown", int mon = 1000 ) : base(nm)
        {
            money = mon;
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

        public bool CanDoubleStake()
        {
            return stake * 2 <= money;
        }

        public void WinStake()
        {
            money += stake;
        }

        public void LoseStake()
        {
            money -= stake;
        }

        public void BonusStake(double coeff = 1.5)
        {
            stake = (int)(stake * coeff);
        }
    }
}
