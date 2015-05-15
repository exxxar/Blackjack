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
    public interface ICardGame
    {
        /// <summary>
        /// 
        /// </summary>
        void Shuffle();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="nPlayer"></param>
        /// <returns></returns>
        int PlayResults( int nPlayer ); 
    }
}
