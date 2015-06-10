using System.Drawing;
using System.Threading;
using System.Threading.Tasks;


namespace Blackjack
{
    /// <summary>
    /// Handler of the GameOver event (passed to the caller)
    /// </summary>
    public delegate void GameOverHandler();


    /// <summary>
    /// Controller class is responsive for interaction between the visualizer and the game
    /// </summary>
    public class CardTableController
    {
        private BlackjackGame game = null;
        private CardTableVisualizer cardtable = null;

        /// <summary>
        /// GameOver event (passed to the caller)
        /// </summary>
        public event GameOverHandler GameOver;

        /// <summary>
        /// The number of players who refused to take winning 1-to-1 immediately after the blackjack on two cards
        /// (causing dealer to wait until the end of a shuffle)
        /// </summary>
        private byte nDealerWaits = 0;

        
        
        /// <summary>
        /// Controller's constructor sets the objects passed as parameters
        /// </summary>
        /// <param name="blackjackgame">The game object</param>
        /// <param name="gamevisualizer">The visualizer object</param>
        /// <param name="handler">The GameOver event handler (caller's function - in our case it's in the MainForm)</param>
        public CardTableController( BlackjackGame blackjackgame, CardTableVisualizer gamevisualizer, GameOverHandler handler )
        {
            game = blackjackgame;
            cardtable = gamevisualizer;
            GameOver += handler;
        }
        
        
        #region Actions
        
        /// <summary>
        /// Method starts new game (new shuffle)
        /// </summary>
        public async void StartNewShuffle()
        {
            // new game
            game.Shuffle();
            nDealerWaits = 0;

            // give two cards to each player
            for (int i = 0; i < game.GetPlayersCount(); i++)
            {
                await Task.Delay(700);
                MoveCardToPlayer(i);
                await Task.Delay(700);
                MoveCardToPlayer(i);
            }

            // give card to the dealer
            await Task.Delay(700);
            MoveCardToDealer();
            
            // iterate across all players and see if someone's got blackjack on 2 cards
            for (int i = 0; i < game.GetPlayersCount(); i++)
            {
                // in that case dealer should act specially
                DealerFirstHit(i);
            }
        }


        /// <summary>
        /// Give a card to a player from the top of some deck (with animation)
        /// </summary>
        /// <param name="nPlayer">The ordinal number of a player to give card to</param>
        private void MoveCardToPlayer( int nPlayer )
        {
            int nDeck;
            Card card = game.PopCardFromDeck( out nDeck );

            //Animate
            cardtable.DrawCard(card, nDeck);
            Thread.Sleep( 300 );                    // yeah, I know (((
            cardtable.DrawShoes();

            // Player can get busted or get a score of 21 after receiving new card 
            try
            {
                game.GetPlayer( nPlayer ).TakeCard(card);
            }
            catch (BustException)
            {
                // setting the BUST state and indicating LOSE right away (no matter what the dealer will get)
                game.SetPlayerState(nPlayer, PlayerState.BUST);
                game.GetPlayer(nPlayer).PlayResult = PlayerResult.LOSE;
            }
            catch (BlackjackException)
            {
                game.SetPlayerState(nPlayer, PlayerState.BLACKJACK);
            }
            finally
            {
                cardtable.Invalidate();
            }
        }


        /// <summary>
        /// Give a card to the dealer from the top of some deck (with animation)
        /// </summary>
        private void MoveCardToDealer()
        {
            cardtable.DrawOptions();

            int nDeck;
            Card card = game.PopCardFromDeck(out nDeck);

            // Dealer can get busted or get a score of 21 after receiving new card (just like any player)
            try
            {
                game.GetDealer().TakeCard( card );
            }
            catch (BustException)
            {
                game.DealerBust = true;
            }
            catch (BlackjackException)
            {
                game.DealerBlackjack = true;
            }
            
            //Animate
            cardtable.DrawCard(card, nDeck);
            Thread.Sleep(300);
            
            cardtable.DrawShoes();
            cardtable.ShowDealerHand();
            
            cardtable.Invalidate();
        }


        /// <summary>
        /// Method checks the states of all players
        /// </summary>
        /// <returns>true if all players are busted or won the game; false - otherwise</returns>
        private bool CheckAllBustOrWon()
        {    
            int k = 0;
            for (; k < game.GetPlayersCount(); k++)
            {
                if (game.GetPlayerState(k) != PlayerState.BUST && game.GetPlayer(k).PlayResult != PlayerResult.WIN)
                {
                    break;
                }
            }
            return ( k == game.GetPlayersCount() );
        }

        
        /// <summary>
        /// Method emulates dealer moves
        /// </summary>
        private void DealerHit()
        {
            // if all busted or won no dealer's move!
            if ( CheckAllBustOrWon() )
            {
                for (int i = 0; i < game.GetPlayersCount(); i++)
                {
                    game.GameResults(i);
                }

                // emit the GameOver event
                GameOver();

                cardtable.Invalidate();
                return;
            }

            // Give dealer cards until he's got 17 or more
            while (game.GetDealer().CountScore() < 17)		
            {
                MoveCardToDealer();				            
                Thread.Sleep(300);
            }
    
            // Calculate the game results for each player
            for (int i = 0; i < game.GetPlayersCount(); i++)
            {
                game.GameResults(i);
                cardtable.Invalidate();
            }

            // emit the GameOver event
            GameOver();
        }


        /// <summary>
        /// Method checks if dealer should wait and not start getting cards from shoes
        /// </summary>
        private bool DealerShouldWait( byte nDealerWaits )
        {
            byte nTotalWaits = 0;

            // count the number of cases when a player hasn't already finished the game or made a "stand"
            for (int i = 0; i < game.GetPlayersCount(); i++)
            {
                if (game.GetPlayerState(i) == PlayerState.STAND ||
                    game.GetPlayer(i).PlayResult != PlayerResult.UNDEFINED)
                        nTotalWaits++;
            }

            // consider also nDealerWaits
            if (nTotalWaits + nDealerWaits == game.GetPlayersCount())
                return false;
            else
                return true;
        }


        /// <summary>
        /// Method emulates the first moves of a dealer - if some player's got a blackjack on 2 cards, the dealer acts specially 
        /// </summary>
        private void DealerFirstHit(int nPlayer)
        {
            // if the player hasn't got blackjack then do nothing
            if ( game.CheckBlackJack( game.GetPlayer(nPlayer) ) )
            {
                game.SetPlayerState(nPlayer, PlayerState.BLACKJACK);

                // check the first dealer's card: if it has rank of 10 or 11
                if (game.GetPlayer(nPlayer).PlayerHand.GetCardsNumber() == 2)
                {
                    if (game.GetDealer().CountScore() >= 10)
                    {
                        // the dealer asks player if he/she wants to take the winning right away
                        System.Windows.Forms.DialogResult res =
                                        System.Windows.Forms.MessageBox.Show(
                                        game.GetPlayer(nPlayer).Name + ", would you like to take your win 1-to-1 or keep playing " +
                                        "(in that case if the dealer doesn't have blackjack you'll win 3-to-2!)?",
                                        "Dealer's got 10, J, Q, K or A!",
                                        System.Windows.Forms.MessageBoxButtons.YesNo);

                        // if player clicks Yes then he/she wins immediately
                        if (res == System.Windows.Forms.DialogResult.Yes)
                        {
                            game.GetPlayer(nPlayer).PlayResult = PlayerResult.WIN;
                            game.TotalLose += game.GetPlayer(nPlayer).Stake;
                        }

                        // otherwise dealer will wait until the end of a shuffle and we increment nDealerWaits
                        else if (res == System.Windows.Forms.DialogResult.No)
                            nDealerWaits++;
                        
                        // check if it was the last player and others are waiting
                        if ( !DealerShouldWait( nDealerWaits ) )
                                DealerHit();

                        // if game is finished
                        if (game.CheckGameFinished())
                        {
                            GameOver();
                        }

                        cardtable.Invalidate();
                    }
                    else
                    {
                        // if a player's got blackjack on 2 cards and the first dealer's card has rank less than 10
                        // the dealer loses immediately to this player (and if there's only one player we finish the game here)
                        if (game.GetPlayersCount() == 1)
                        {                       
                            game.GameResults( nPlayer );
                            GameOver();
                        }
                    }
                }
            }
        }

        #endregion



        /// <summary>
        /// Process the actions of a player (mouse click on HIT, STAND or DOUBLE)
        /// </summary>
        /// <param name="mousePoint"></param>
        public void UserActions( Point mousePoint )
        {
            // check the mouse location (over HIT, STAND or DOUBLE option)
            for (int i = 0; i < game.GetPlayersCount(); i++)
            {
                if ( cardtable.hitrects[i].Contains(mousePoint) && 
                    game.GetPlayerState(i) != PlayerState.STAND && 
                    game.GetPlayer(i).PlayResult == PlayerResult.UNDEFINED)
                {
                    MoveCardToPlayer(i);
                }

                else if (cardtable.standrects[i].Contains(mousePoint) &&
                    game.GetPlayerState(i) != PlayerState.BUST &&
                    game.GetPlayer(i).PlayResult == PlayerResult.UNDEFINED)
                {
                    game.SetPlayerState(i, PlayerState.STAND);
                    cardtable.Invalidate();
                }

                else if (cardtable.doublerects[i].Contains(mousePoint) &&
                    game.GetPlayerState(i) != PlayerState.STAND &&
                    game.GetPlayer(i).PlayResult == PlayerResult.UNDEFINED)
                {
                    game.SetPlayerState(i, PlayerState.DOUBLE);
                    MoveCardToPlayer(i);
                    cardtable.Invalidate();
                }
            }

            // if all players (they have some of the following states: BUST, STAND or BLACKJACK)
            if ( game.CheckStates() )
                // dealer gets into play
                DealerHit();
        }
    }
}
