using System;
using System.Collections.Generic;


namespace Blackjack
{
    /// <summary>
    /// Basically, a wrapper over collection of <see cref="BlackjackResult"/>
    /// </summary>
    public class PlayerStats
    {
        /// <summary>
        /// The list of shuffle infos
        /// </summary>
        public List<BlackjackResult> gameResults = new List<BlackjackResult>();


        /// <summary>
        /// Method adds the info about a new shuffle
        /// </summary>
        /// <param name="p"><see cref="Player"/> object</param>
        /// <param name="shuffleNo">The shuffe number</param>
        public void AddShuffleResult( Player p, int shuffleNo )
        {
            int nPlayer = gameResults.FindIndex( g => g.player.Name == p.Name );

            if (nPlayer >= 0)
            {
                gameResults[nPlayer].shuffles.Add( shuffleNo );
                gameResults[nPlayer].results.Add( p.PlayResult );
                gameResults[nPlayer].stakes.Add( p.Stake );
            }
        }
    }
}
