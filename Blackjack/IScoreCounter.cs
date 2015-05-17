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
    public interface IScoreCounter
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="hand"></param>
        /// <returns></returns>
        byte CountScore(CardSet hand);
    }
}
