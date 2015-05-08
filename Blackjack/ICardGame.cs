using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blackjack
{
    public interface ICardGame
    {
        void Shuffle();
        int PlayResults( int nPlayer ); 
    }
}
