using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blackjack
{
    public interface IScoreCounter
    {
        byte CountScore(Hand hand);
    }
}
