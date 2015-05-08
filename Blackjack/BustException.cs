using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blackjack
{
    public class BustException : BlackjackException
    {
        private string playerName;
        private int score;

        public string PlayerName
        {
            get { return playerName; }
            set { playerName = value; }
        }

        public BustException( string name="Unknown", int s = 22 )
        {
            score = s;
            message = name + " : bust!\n Score: " + s;
        }
    }
}
