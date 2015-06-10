using System;
using System.Collections.Generic;


namespace Blackjack
{
    /// <summary>
    /// BlackjackGame class represents the BlackJack card game entity
    /// </summary>
    public class BlackjackGame: ICardGame
    {
        /// <summary>
        /// The size of minimal stake
        /// </summary>
        public const int MIN_STAKE = 100;

        /// <summary>
        /// The number of decks is 4
        /// </summary>
        public const int DECKS_COUNT = 4;

        /// <summary>
        /// Shoes, DECKS_COUNT decks
        /// </summary>
        protected Deck[] decks = new Deck[DECKS_COUNT];

        
        /// <summary>
        /// The number of players is 7 (tops) 
        /// </summary>
        public const int MAX_PLAYERS = 7;

        /// <summary>
        /// Composite for the dealer
        /// </summary>
        protected CardHolder dealer = new CardHolder("Dealer");

        /// <summary>
        /// Aggregates the player
        /// </summary>
        protected List<Player> players = new List<Player>();
        

        /// <summary>
        /// Player states (BUST, BLACKJACK, etc.) are not included to the Player Info
        /// since these states are not universal for all kinds of games while Players should be possible to use in any card game 
        /// </summary>
        private List<PlayerState> playerStates = new List<PlayerState>();

        /// <summary>
        /// BlackjackScoreCounter object for calculation of the total number of points in a cardset
        /// </summary>
        IScoreCounter scoreCounter = new BlackjackScoreCounter();


        /// <summary>
        /// Dealer's total lose property (can be +2000 or -1500, for instance)
        /// </summary>
        public int TotalLose { get; set; }

        /// <summary>
        /// DealerBlackjack property (we don't store this info in enum like in case of player)
        /// </summary>
        public bool DealerBlackjack { get; set; }
        
        /// <summary>
        /// DealerBust property (we don't store this info in enum like in case of player)
        /// </summary>
        public bool DealerBust { get; set; }

        
        /// <summary>
        /// Default constructor
        /// </summary>
        public BlackjackGame()
        {
            dealer.SetScoreCounter( scoreCounter );

            for (int i = 0; i < DECKS_COUNT; i++)
                decks[i] = new Deck();

            TotalLose = 0;

            DealerBlackjack = DealerBust = false;
        }
	

        /// <summary>
        /// Method forms the list of players for a current game
        /// </summary>
        /// <param name="list">Collection of players</param>
        public void SetPlayerList( IEnumerable<Player> list )
        {
            players.Clear();

            foreach (var p in list)
            {
                p.SetScoreCounter(scoreCounter);
                players.Add(p);
                playerStates.Add(PlayerState.HIT);
            }
        }


        /// <summary>
        /// Method removes all players 
        /// </summary>
        public void RemovePlayersMinStake()
        {
            for ( int i=0; i<players.Count; i++ )
                if ( players[i].Money < MIN_STAKE )
                {
                    players.RemoveAt(i);
                    i--;
                }
        }


        /// <summary>
        /// Method returns the player by his/her number
        /// </summary>
        /// <param name="pos">Player's number</param>
        /// <returns>Player</returns>
        public Player GetPlayer(int pos)
        {
            return players[pos];
        }


        /// <summary>
        /// Method returns the dealer object
        /// </summary>
        /// <returns></returns>
        public CardHolder GetDealer()
        {
            return dealer;
        }


        /// <summary>
        /// Method returns the deck by its number
        /// </summary>
        /// <param name="pos">Deck's number</param>
        /// <returns>Deck</returns>
        public Deck GetDeck(int pos)
        {
            return decks[pos];
        }


        /// <summary>
        /// Method return the total number of players
        /// </summary>
        /// <returns>Total number of players</returns>
        public int GetPlayersCount()
        {
            return players.Count;
        }


        /// <summary>
        /// Method sets a player's state
        /// </summary>
        /// <param name="nPlayer">Player's number</param>
        /// <param name="state">Player's state</param>
        /// <exception cref="InvalidOperationException">Exception is thrown if the player has not enough money to double down</exception>
        public void SetPlayerState(int nPlayer, PlayerState state)
        {
            if (state == PlayerState.DOUBLE)
            {
                if (players[nPlayer].CanDoubleStake())
                    players[nPlayer].BonusStake(2);
                else
                    throw new InvalidOperationException();
            }

            playerStates[nPlayer] = state;
        }


        /// <summary>
        /// Method gets a player's state
        /// </summary>
        /// <param name="nPlayer">Player's number</param>
        /// <returns>Player's state</returns>
        public PlayerState GetPlayerState(int nPlayer)
        {
            return playerStates[nPlayer];
        }
        

        /// <summary>
        /// Helper function that checks states of all players
        /// </summary>
        /// <returns></returns>
        public bool CheckStates()
        {
            for (int i = 0; i < players.Count; i++)
            {
                if (GetPlayerState(i) != PlayerState.BUST &&
                    GetPlayerState(i) != PlayerState.STAND &&
                    GetPlayerState(i) != PlayerState.BLACKJACK)
                    return false;
            }
            return true;
        }


        /// <summary>
        /// Helper function that checks if the game's finished (all players have definite results)
        /// </summary>
        public bool CheckGameFinished()
        {
            for (int i = 0; i < players.Count; i++)
            {
                if (players[i].PlayResult == PlayerResult.UNDEFINED)
                    return false;
            }
            return true;
        }


        /// <summary>
        /// Method analyzes and forms the results of the game for a particular player
        /// </summary>
        /// <param name="nPlayer">Player</param>
        /// <returns>1 - Player wins; 0 - Stay; -1 - Player loses</returns>
	    public int GameResults( int nPlayer )
        {
            // recalculate total lose for casino if some of the players loses
                        
            // first check for player's bust
            if (GetPlayerState(nPlayer) == PlayerState.BUST)
            {
                players[nPlayer].LoseStake();
                TotalLose -= players[nPlayer].Stake;
                return -1;
            }

            // first, set bonuses (if there are any) to the player
            SetBonuses( nPlayer );


            // then check for dealer's bust
            if (dealer.CountScore() > 21)
            {
                // additional check!
                if (GetPlayer(nPlayer).PlayResult != PlayerResult.WIN)
                {
                    TotalLose += players[nPlayer].Stake;
                }
                players[nPlayer].WinStake();
                return 1;
            }

            // then simply compare the score:

            // DEALER WINS
	        if ( players[ nPlayer ].CountScore() < dealer.CountScore() )
	        {
                players[ nPlayer ].LoseStake();
                TotalLose -= players[nPlayer].Stake;
                return -1;
	        }
            // PLAYER WINS
	        else if ( players[ nPlayer ].CountScore() > dealer.CountScore() )
	        {
                // additional check if the player has already taken the win
                if ( GetPlayer(nPlayer).PlayResult != PlayerResult.WIN )
                {
                    TotalLose += players[nPlayer].Stake;
                }
                players[nPlayer].WinStake();
                return 1;
	        }
            // STAY
            else
            {
                // it can happen if afer the 1st dealer hit player already chose win 1-to-1
                if ( players[nPlayer].PlayResult != PlayerResult.WIN )      
                    players[nPlayer].PlayResult = PlayerResult.STAY;

                return 0;
            }
        }

	    

        /// <summary>
        /// Method sets bonus stakes for a player under certain circumstances among which are:
        /// <ul>
        ///     <li>Player's got Blackjack and dealer has more than one card</li>
        ///     <li>Player's got Blackjack on the first two cards</li>
        ///     <li>Player has 777</li>
        ///     <li>Dealer's got blackjack</li>
        /// </ul>
        /// </summary>
        /// <param name="nPlayer"></param>
	    public void SetBonuses( int nPlayer )
        {
            // отдельно рассматриваем случаи с блекджеком (там другие правила начисления выигрыша)
            if ( CheckBlackJack( players[ nPlayer ] ) )				
	        {
                // if he/she didn't take 1-to-1 right away
                if (players[nPlayer].PlayResult != PlayerResult.WIN)
                {
                    // если при блекджеке игрока нет блекджека дилера,
                    if (!CheckBlackJack(dealer))
                    {
					    // если три семерки, это особая ситуация доп. выигрыш игроку (2 к 1)
                        if ( Check777(players[nPlayer]) )
                        {
                            players[nPlayer].BonusStake( 2 );
                        }
                        // то еще проверяем, по сколько у них карт:			
                        // если у дилера больше 1 карты, т.е. он пытался добрать карты, чтобы "перебить" блекджек игрока    
                        else if (dealer.PlayerHand.GetCardsNumber() > 1)
                            // то выигрыш игрока должен быть 3 к 2 (по умолчанию)
                            players[nPlayer].BonusStake();

                        // также есть особая ситуация в самом начале (с раздачи):
                        // у дилера 1 карта, у игрока - 2
                        else if (dealer.PlayerHand.GetCardsNumber() == 1
                                    && players[nPlayer].PlayerHand.GetCardsNumber() == 2)
                            // в таком случае тоже выигрыш игрока равен 3 к 2
                            players[nPlayer].BonusStake();
                    }
                }
	        }
        }


        /// <summary>
        /// Method checks if a card holder's got the BlackJack
        /// </summary>
        /// <param name="cardHolder">Card holder</param>
        /// <returns>true if a cardholder's got the BlackJack; false - otherwise</returns>
	    public bool CheckBlackJack( CardHolder cardHolder )
        {
            return (cardHolder.CountScore() == 21);
        }
	

        /// <summary>
        /// Method checks if a card holder's got the BlackJack with "777" combination
        /// </summary>
        /// <param name="cardHolder">Card holder</param>
        /// <returns>true if a cardholder's got the "777" combination; false - otherwise</returns>
        public bool Check777( CardHolder cardHolder )
        {
            if (cardHolder.CountScore() == 21)
            {
                CardSet hand = cardHolder.PlayerHand;

                if (hand.GetCardsNumber() == 3)
                    if (hand[0].Rank == 7 && hand[1].Rank == 7 && hand[2].Rank == 7)
                        return true;
            }

            return false;
        }


        /// <summary>
        /// Method pops a card from random deck
        /// </summary>
        /// <param name="nDeck">The number of a deck from which a card was popped</param>
        /// <returns>The card popped from the deck</returns>
        public Card PopCardFromDeck( out int nDeck )
        {
            Random r = new Random();
            nDeck = r.Next( DECKS_COUNT );
            return decks[nDeck].PopCard();
        }


        /// <summary>
        /// Initialize new shuffle (start new game)
        /// </summary>
        public void Shuffle()
        {
            // initialize dealer states
            DealerBlackjack = DealerBust = false;
            
            // ------------------------------------------ clear hands
            dealer.ClearHand();				

            for (int i=0; i<players.Count; i++ )
            {
                players[i].ClearHand();
                SetPlayerState( i, PlayerState.HIT );
                players[i].PlayResult = PlayerResult.UNDEFINED;
            }
            // ------------------------------------------------------

            // randomly shuffle all decks with different seed
            for (int i = 0; i < DECKS_COUNT; i++)
                decks[i].Shuffle( i+1 );

            // initialize TotalLose
            TotalLose = 0;
        }
    }
}
