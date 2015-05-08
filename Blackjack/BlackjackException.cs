using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blackjack
{
    public class BlackjackException: InvalidOperationException
    {
        protected string message;

        public BlackjackException( string msg = "Something wrong happened!" )
        {
            message = msg;
        }
    }
}
