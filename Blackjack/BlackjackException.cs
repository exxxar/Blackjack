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
    public class BlackjackException: InvalidOperationException
    {
        protected string message;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="msg"></param>
        public BlackjackException( string msg = "BlackJck!" )
        {
            message = msg;
        }
    }
}
