using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;


namespace Blackjack
{
    /// <summary>
    /// 
    /// </summary>
    public class CardTableController
    {
        BlackjackGame game = null;
        CardTableVisualizer cardtable = null;
        
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="blackjackgame"></param>
        public CardTableController( BlackjackGame blackjackgame, CardTableVisualizer gamevisualizer )
        {
            game = blackjackgame;
            cardtable = gamevisualizer;
        }



        #region Actions

        /// <summary>
        /// 
        /// </summary>
        public async void StartNewShuffle()
        {
            game.Shuffle();

            for (int i = 0; i < game.GetPlayersCount(); i++)
            {
                await Task.Delay(900);
                MoveCardToPlayer(i);
                await Task.Delay(900);
                MoveCardToPlayer(i);

                cardtable.Invalidate();
            }

            await Task.Delay(500);
            MoveCardToDealer();

            cardtable.Invalidate();

            for (int i = 0; i < game.GetPlayersCount(); i++)
            {
                DealerFirstHit(i);
            }
            
            cardtable.Invalidate();
        }



        /// <summary>
        /// 
        /// </summary>
        /// <param name="nPlayer"></param>
        public void MoveCardToPlayer( int nPlayer )
        {
            int nDeck;
            Card card = game.PopCardFromDeck( out nDeck );

            //Animate
            cardtable.DrawCard(card, nDeck);
            Thread.Sleep( 300 );
            cardtable.DrawShoes();

            try
            {
                game.GetPlayer( nPlayer ).TakeCard(card);
            }
            catch (BustException)
            {
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
        /// 
        /// </summary>
        public void MoveCardToDealer()
        {
            cardtable.DrawOptions();

            Random r = new Random();
            int nDeck = r.Next( BlackjackGame.DECKS_COUNT );

            Card card = game.PopCardFromDeck(out nDeck);

            try
            {
                game.GetDealer().TakeCard( card );
            }
            catch (BustException)
            {
                game.dealerBust = true;
            }
            catch (BlackjackException)
            {
                game.dealerBlackjack = true;
            }
            
            //Animate
            cardtable.DrawCard(card, nDeck);
            Thread.Sleep(300);
            cardtable.DrawShoes();
            cardtable.ShowDealerHand();
            cardtable.Invalidate();
        }

        
        /// <summary>
        /// 
        /// </summary>
        public void DealerHit()
        {
            // if all busted or won no dealer's move!
            int k = 0;
            for (; k < game.GetPlayersCount(); k++)
            {
                if (game.GetPlayerState(k) != PlayerState.BUST && game.GetPlayer(k).PlayResult != PlayerResult.WIN)
                {
                    break;
                }
            }

            if (k == game.GetPlayersCount())
            {
                for (int i = 0; i < game.GetPlayersCount(); i++)
                {
                    game.PlayResults(i);
                }
                cardtable.Invalidate();
                return;
            }


            while (game.GetDealer().CountScore() < 17)		// дилер здесь добирает карты, пока у него нет 17
            {
                MoveCardToDealer();				            // здесь возможен эксепшн! (он перехватывается в функции уровнем выше)
                Thread.Sleep(300);
            }
    
            for (int i = 0; i < game.GetPlayersCount(); i++)
            {
                game.PlayResults(i);
                cardtable.Invalidate();
            }
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="nPlayer"></param>
        /// <returns></returns>
        public void DealerFirstHit(int nPlayer)
        {
            // самое хитрое тут: сразу проверяем, вдруг у игрока блекджек на 2 картах
            if ( game.CheckBlackJack( game.GetPlayer(nPlayer) ) )
            {
                game.SetPlayerState(nPlayer, PlayerState.BLACKJACK);

                // тут еще проверка, что и у дилера может оказаться первой карта ценой 10 или 11
                // (но это нужно только для случая 2 карт у игрока в хэнде)

                if (game.GetPlayer(nPlayer).PlayerHand.GetCardsNumber() == 2)
                {
                    if (game.GetDealer().CountScore() >= 10)
                    {
                        // можно просто взять выигрыш сразу (а если нет, то может быть выигрыш 3 к 2 (если у дилера не будет блекджека)
                        System.Windows.Forms.DialogResult res =
                                        System.Windows.Forms.MessageBox.Show(
                                        game.GetPlayer(nPlayer).Name + ", would you like to take your win 1-to-1 or keep playing " +
                                        "(in that case if the dealer doesn't have blackjack you'll win 3-to-2!)?",
                                        "Dealer's got 10, J, Q, K or A!",
                                        System.Windows.Forms.MessageBoxButtons.YesNo);

                        // если берем, то в этом случае сразу выходим отсюда
                        if (res == System.Windows.Forms.DialogResult.Yes)
                        {
                            game.SetPlayerState(nPlayer, PlayerState.BLACKJACK);

                            game.GetPlayer(nPlayer).WinStake();
                            game.totalLose += game.GetPlayer(nPlayer).Stake;

                            // check if it was the last player and others are waiting
                            //if (game.CheckGameFinished())
                            if (!game.CheckStates())
                                DealerHit();

                            cardtable.Invalidate();
                        }
                        else
                        {
                            game.SetPlayerState(nPlayer, PlayerState.STANDBLACKJACK);

                            // TODO: check for all stopped playing!!!
                            if (game.CheckStates())
                            //if (game.CheckGameFinished())
                                DealerHit();

                            cardtable.Invalidate();
                        }
                    }
                    else
                    {
                        // если у игрока на 2 картах блекджек, а первая карта дилера меньше 10, то он сразу проигрывает (в схватке с данным игроком)
                        if (game.GetPlayersCount() == 1)
                        {
                            game.GetPlayer(nPlayer).BonusStake();
                            game.GetPlayer(nPlayer).WinStake();
                            game.totalLose += game.GetPlayer(nPlayer).Stake;
                        }
                    }
                }
            }
        }



        #endregion



        /// <summary>
        /// 
        /// </summary>
        /// <param name="mousePoint"></param>
        public void UserActions( Point mousePoint )
        {
            Rectangle[] hitrects = new Rectangle[ BlackjackGame.MAX_PLAYERS ];
            for (int i = 0; i < game.GetPlayersCount(); i++)
            {
                hitrects[i] = new Rectangle(25 + 105 * i, 285, 30, 30);
                if (hitrects[i].Contains(mousePoint) && game.GetPlayerState(i) != PlayerState.STAND)
                {
                    MoveCardToPlayer(i);
                }
            }

            Rectangle[] standrects = new Rectangle[ BlackjackGame.MAX_PLAYERS ];
            for (int i = 0; i < game.GetPlayersCount(); i++)
            {
                standrects[i] = new Rectangle(60 + 105 * i, 285, 30, 30);
                if (standrects[i].Contains(mousePoint) && game.GetPlayerState(i) != PlayerState.BUST)
                {
                    game.SetPlayerState(i, PlayerState.STAND);
                    cardtable.Invalidate();
                }
            }

            Rectangle[] doublerects = new Rectangle[ BlackjackGame.MAX_PLAYERS ];
            for (int i = 0; i < game.GetPlayersCount(); i++)
            {
                doublerects[i] = new Rectangle(95 + 105 * i, 285, 30, 30);
                if (doublerects[i].Contains(mousePoint) && game.GetPlayerState(i) != PlayerState.STAND)
                {
                   game.SetPlayerState(i, PlayerState.DOUBLE);
                   MoveCardToPlayer(i);
                   cardtable.Invalidate();
                }
            }

            if (game.CheckStates())
                DealerHit();
        }
    }
}
