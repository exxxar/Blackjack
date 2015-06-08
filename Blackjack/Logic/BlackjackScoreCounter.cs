﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blackjack
{
    /// <summary>
    /// 
    /// </summary>
    public class BlackjackScoreCounter: IScoreCounter
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="hand"></param>
        /// <returns></returns>
        public byte CountScore(CardSet hand)
        {
            byte score = 0;

            byte aces = 0;

            for (int i = 0; i < hand.GetCardsNumber(); i++)
            {
                byte rank = hand[i].Rank;

                if (rank > 10)
                {
                    if (rank == 14)
                    {
                        rank = 11;
                        aces++;
                    }
                    else
                    {
                        rank = 10;
                    }
                }

                score += rank;
            }

            while (score > 21 && aces > 0)
            {
                score -= 10;
                aces--;
            }

            return score;
        }
    }
}
