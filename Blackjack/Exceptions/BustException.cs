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
    public class BustException : BlackjackException
    {
        private string playerName;
        private int score;

        /// <summary>
        /// 
        /// </summary>
        public string PlayerName
        {
            get { return playerName; }
            set { playerName = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="name"></param>
        /// <param name="s"></param>
        public BustException( string name="Unknown", int s = 22 )
        {
            score = s;
            message = name + " : bust!\n Score: " + s;
        }
    }
}
