using System;
using System.Collections.Generic;


namespace Blackjack
{
    /// <summary>
    /// The objects of this class are stored in shuffles stats
    /// </summary>
    public class BlackjackResult
    {
        /// <summary>
        /// The player
        /// </summary>
        public Player player;

        /// <summary>
        /// Numbers of shuffles the player took part in
        /// </summary>
        public List<int> shuffles = new List<int>();

        /// <summary>
        /// Player results in all shuffles he/she took part in
        /// </summary>
        public List<PlayerResult> results = new List<PlayerResult>();

        /// <summary>
        /// Player stakes in all shuffles he/she took part in
        /// </summary>
        public List<int> stakes = new List<int>();


        /// <summary>
        /// Default constructor
        /// </summary>
        public BlackjackResult()
        {
        }

        /// <summary>
        /// Parameterized constructor
        /// </summary>
        /// <param name="p"><see cref="Player"/> object</param>
        /// <param name="shuffleNo">The number of shuffle in stats</param>
        public BlackjackResult( Player p, int shuffleNo )
        {
            player = p;
            shuffles.Add( shuffleNo );
            results.Add( p.PlayResult );
            stakes.Add(p.Stake);
        }
    }
}
