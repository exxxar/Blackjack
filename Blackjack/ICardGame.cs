using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blackjack
{
    interface ICardGame
    {
        void Shuffle();
	    void ShowTable();
        void PlayResults(); 
    }
}
