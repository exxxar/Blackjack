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
    public class BlackjackResult
    {
        public Player player;
        public List<int> shuffles = new List<int>();
        public List<PlayerResult> results = new List<PlayerResult>();
        public List<int> stakes = new List<int>();


        /// <summary>
        /// 
        /// </summary>
        public BlackjackResult()
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="p"></param>
        /// <param name="shuffleNo"></param>
        public BlackjackResult( Player p, int shuffleNo )
        {
            player = p;
            shuffles.Add( shuffleNo );
            results.Add( p.PlayResult );
            stakes.Add(p.Stake);
        }
    }


    /// <summary>
    /// 
    /// </summary>
    public class PlayerStats
    {
        /// <summary>
        /// 
        /// </summary>
        public List<BlackjackResult> gameResults = new List<BlackjackResult>();

        /// <summary>
        /// 
        /// </summary>
        public int curShuffle = 0;


        /// <summary>
        /// 
        /// </summary>
        /// <param name="p"></param>
        /// <param name="shuffleNo"></param>
        public void AddShuffleResult( Player p, int shuffleNo )
        {
            int nPlayer = gameResults.FindIndex( g => g.player.Name == p.Name );

            if (nPlayer >= 0)
            {
                gameResults[nPlayer].shuffles.Add( shuffleNo );
                gameResults[nPlayer].results.Add( p.PlayResult );
                gameResults[nPlayer].stakes.Add( p.Stake );
            }
            else
            {
                gameResults.Add( new BlackjackResult(p, curShuffle) );
            }
        }
    }
}
