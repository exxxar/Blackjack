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

        public BlackjackResult()
        {
        }

        public BlackjackResult( Player p, int shuffleNo )
        {
            player = p;
            shuffles.Add( shuffleNo );
            results.Add( p.PlayResult );
            stakes.Add( p.Stake );
        }
    }


    /// <summary>
    /// 
    /// </summary>
    public class PlayerStats
    {
        public List<BlackjackResult> gameResults = new List<BlackjackResult>();

        public int curShuffle = 0;


        public void AddPlayer(Player p)
        {
            // check if a player with this name already exists
            //...

            gameResults.Add( new BlackjackResult( p, curShuffle ) );
        }

        public void AddShuffleResult( Player p )
        {
            BlackjackResult res = new BlackjackResult();
            res.player = p;
            res.shuffles[curShuffle] = curShuffle;
            res.results[curShuffle] = p.PlayResult;
            res.stakes[curShuffle] = p.Stake;

            gameResults.Add( res );

            curShuffle++;
        }

        //public void SetPlayerResult(Player p)
        //{
        //    int idx = gameResults.Count - 1;
        //    gameResults[idx].playerNames.Add(p.Name);
        //    gameResults[idx].stakes.Add(p.Stake);
        //    gameResults[idx].results.Add(p.PlayResult);
        //}
    }
}
