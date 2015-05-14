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
    public class CardTableController
    {
        private Graphics DC;
        private Graphics g = null;
        private Bitmap dbufBitmap = null;

        Bitmap cardBack = null;
        private Bitmap[] cardImages = new Bitmap [52];

        Point dealerCoords;
        public Point[] playerCoords = new Point[BlackjackGame.MAX_PLAYERS];

        public Point[] shoesCoords = new Point[ BlackjackGame.DECKS_COUNT ];
        public Point[] shoesCoordsToDraw = new Point[ BlackjackGame.DECKS_COUNT ];


        BlackjackGame game = null;

        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="blackjackgame"></param>
        public CardTableController( BlackjackGame blackjackgame )
        {
            Bitmap bm = new Bitmap(@"d:\projects\github\Blackjack\Blackjack\images\back.png");
            cardBack = new Bitmap(bm, 90, 130);
            bm.Dispose();

            for (int i = 0; i < 52; i++)
            {
                Bitmap b = new Bitmap( @"d:\projects\github\Blackjack\Blackjack\images\" + i + ".png");
                cardImages[i] = new Bitmap( b, 90, 130 );
                b.Dispose();
            }

            game = blackjackgame;

            dealerCoords.X = 500;
            dealerCoords.Y = 50;

            for (int i = 0; i < BlackjackGame.MAX_PLAYERS; i++)
            {
                playerCoords[i].X = 30 + 105*i;
                playerCoords[i].Y = 320;
            }

            for (int i = 0; i < BlackjackGame.DECKS_COUNT; i++)
            {
                shoesCoords[i].X = 10 + 100 * i;
                shoesCoords[i].Y = 50;
            }
        }


        #region DoubleBuffereing things

        /// <summary>
        /// 
        /// </summary>
        /// <param name="w"></param>
        /// <param name="h"></param>
        /// <param name="realGraphics"></param>
        public void PrepareGraphics(int w, int h, Graphics realGraphics)
        {
            DC = realGraphics;

            if (dbufBitmap != null)
                dbufBitmap.Dispose();

            dbufBitmap = new Bitmap(w, h);
            g = Graphics.FromImage(dbufBitmap);
        }



        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public Bitmap GetShowTable()
        {
            // draw the entire table
            DrawTable();

            // draw shoes
            DrawShoes();

            //draw options
            DrawOptions();

            // draw all dealer's cards
            ShowDealerHand();

            // draw the cards of all players
            for (int i = 0; i < game.GetPlayersCount(); i++)
                ShowPlayerHand(i);

            return dbufBitmap;
        }
        
        #endregion
        

        #region DrawThings

        /// <summary>
        /// 
        /// </summary>
        /// <param name="pos"></param>
        /// <param name="x"></param>
        /// <param name="y"></param>
        public void DrawCard( int pos, int x, int y )
        {
            g.DrawImage( cardImages[pos], x, y, 90, 130 );
        }


        public void ShowDealerHand()
        {
            for (int i = 0; i < game.GetDealer().PlayerHand.GetCardsNumber(); i++ )
                g.DrawImage(cardImages[game.GetDealer().PlayerHand[i].getNumber()],
                    dealerCoords.X + 30*i, dealerCoords.Y, 90, 130);
        }


        public void ShowPlayerHand( int nPlayer )
        {
            Random r = new Random();

            for (int i = 0; i < game.GetPlayer(nPlayer).PlayerHand.GetCardsNumber(); i++)
            {
                g.DrawImage(cardImages[game.GetPlayer(nPlayer).PlayerHand[i].getNumber()],
                    playerCoords[nPlayer].X, playerCoords[nPlayer].Y + 30 * i, 90, 130);
            }
        }


        /// <summary>
        /// 
        /// </summary>
        public void DrawTable()
        {
            g.FillRectangle(new LinearGradientBrush(new Point(0, 0), new Point(dbufBitmap.Width, dbufBitmap.Height),
                                                                            Color.LightGreen, Color.DarkGreen),
                                                                            0, 0, dbufBitmap.Width, dbufBitmap.Height);

            Pen whitePen = new Pen(Color.White, 3);
            Brush whiteBrush = new SolidBrush(Color.White);
            Brush yellowBrush = new SolidBrush(Color.Yellow);
            Font textFont = new Font("Arial", 12);

            for (int i = 0; i < BlackjackGame.MAX_PLAYERS; i++)
            {
                g.DrawRectangle(whitePen, playerCoords[i].X, playerCoords[i].Y, 90, 130);
            }

            for (int i = 0; i < game.GetPlayersCount(); i++)
            {
                g.DrawString(game.GetPlayer(i).Name, textFont, whiteBrush, 30 + 105 * i, 220);
                g.DrawString(game.GetPlayer(i).Money + " $", textFont, whiteBrush, 30 + 105 * i, 240);
                g.DrawString(game.GetPlayer(i).Stake + " $", textFont, yellowBrush, 30 + 105 * i, 260);
            }

            if (game.dealerBlackjack)
            {
                g.DrawImage(Properties.Resources.blackjack_21, 510, 7, 40, 40);
            }

            if (game.dealerBust)
            {
                g.DrawImage(Properties.Resources.bust, 510, 10, 35, 35);
            }


            g.DrawRectangle(whitePen, 500, 50, 90, 130);
            g.DrawRectangle(whitePen, 600, 50, 90, 130);
            g.DrawString("Dealer", textFont, whiteBrush, 560, 20);
 
            g.DrawImage( Properties.Resources.player, 735, 5, 30, 40);

            string casinoMoneyString = string.Format("-{0} $", game.totalLose);
            if (game.totalLose < 0)
                casinoMoneyString = casinoMoneyString.Replace("--", "+");   
            g.DrawString( casinoMoneyString, textFont, whiteBrush, 650, 20);


            whitePen.Dispose();
            whiteBrush.Dispose();
            yellowBrush.Dispose();
            textFont.Dispose();
        }
        

        /// <summary>
        /// 
        /// </summary>
        private void DrawOptions()
        {
            for (int i = 0; i < game.GetPlayersCount(); i++)
            {
                if (game.GetPlayer(i).PlayResult != PlayerResult.UNDEFINED)
                {
                    switch (game.GetPlayer(i).PlayResult)
                    {
                        case PlayerResult.WIN:
                            DrawResult(" WIN!", Color.LightGreen, Color.White, Color.Green, i);
                            break;
                        case PlayerResult.LOSE:
                            DrawResult("LOSE!", Color.LightBlue, Color.White, Color.Blue, i);
                            break;
                        case PlayerResult.STAY:
                            DrawResult("STAY!", Color.LightGoldenrodYellow, Color.Black, Color.Red, i);
                            break;
                    }
                }
                else if ( game.GetPlayerState(i) == PlayerState.STAND)
                {
                    g.DrawImage( Properties.Resources.stand_fix, 60 + 105 * i, 285, 30, 30);
                }
                else
                {
                    g.DrawImage( Properties.Resources.hit, 25 + 105 * i, 285, 30, 30);
                    g.DrawImage( Properties.Resources.stand, 60 + 105 * i, 285, 30, 30);
                    g.DrawImage( Properties.Resources._double, 95 + 105 * i, 285, 30, 30);
                }

                if (game.GetPlayerState(i) == PlayerState.BLACKJACK)
                {
                    g.DrawImage(Properties.Resources.blackjack_21, 90 + 105 * i, 265, 40, 40);
                }
                if (game.GetPlayerState(i) == PlayerState.BUST)
                {
                    g.DrawImage(Properties.Resources.bust, 95 + 105*i, 265, 30, 30);
                }
            }
        }

        
        /// <summary>
        /// 
        /// </summary>
        private void DrawShoes()
        {
            for (int i = 0; i < BlackjackGame.DECKS_COUNT; i++)
            {
                game.GetDeck(i).Shuffle();
                int j = 0;
                for (; j < game.GetDeck(i).GetCardsNumber(); j+=7)
                {
                    g.DrawImage(cardBack, shoesCoords[i].X + j/5, shoesCoords[i].Y + j/7, 90, 130);
                }
                shoesCoordsToDraw[i].X = shoesCoords[i].X  + j/5;
                shoesCoordsToDraw[i].Y = shoesCoords[i].Y +  j/7;
            }
        }



        private void DrawResult(string text, Color backgroundColor, Color textColor, Color borderColor, int nPlayer)
        {
            Pen pen = new Pen( borderColor ); // Pen(Color.Green);     
            Brush backgroundBrush = new SolidBrush( backgroundColor ); // (Color.White);
            Brush textBrush = new SolidBrush( textColor ); // (Color.LightGreen);

            g.DrawRectangle( pen, 25 + 105 * nPlayer, 285, 100, 30);
            g.FillRectangle( backgroundBrush, 26 + 105 * nPlayer, 286, 99, 29);
            g.DrawString( text , new Font("Stencil", 16), textBrush, 35 + 105 * nPlayer, 289);

            pen.Dispose();
            textBrush.Dispose();
            backgroundBrush.Dispose();
        }

        #endregion


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

                DC.DrawImage(dbufBitmap, 0, 0);
            }

            await Task.Delay(500);
            MoveCardToDealer();

            DC.DrawImage(dbufBitmap, 0, 0);

            for (int i = 0; i < game.GetPlayersCount(); i++)
            {
                game.DealerFirstHit(i);
            }
        }



        /// <summary>
        /// 
        /// </summary>
        /// <param name="nPlayer"></param>
        public void MoveCardToPlayer( int nPlayer )
        //public async void MoveCardToPlayer( int nPlayer )
        {
            int nDeck;
            Card card = game.PopCardFromDeck( out nDeck );

            //Animate
            DrawCard(card.getNumber(), shoesCoordsToDraw[nDeck].X, shoesCoordsToDraw[nDeck].Y);
            DC.DrawImage(dbufBitmap, 0, 0);

            Thread.Sleep( 200 );
            //await Task.Delay(1200); //      can't figure out why this doesn't work just like Thread.Sleep()...
            
            DrawShoes();

            try
            {
                game.PlayerHit(nPlayer, card);
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
                DC.DrawImage( GetShowTable(), 0, 0 );
            }
        }


        /// <summary>
        /// 
        /// </summary>
        public void MoveCardToDealer()
        {
            DrawOptions();

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
            DrawCard(card.getNumber(), shoesCoordsToDraw[nDeck].X, shoesCoordsToDraw[nDeck].Y);
            DC.DrawImage(dbufBitmap, 0, 0);
            
            Thread.Sleep(500);

            DrawShoes();
            ShowDealerHand();
            DC.DrawImage(dbufBitmap, 0, 0);
        }

        
        /// <summary>
        /// 
        /// </summary>
        public void DealerHit()
        {
            for ( int i=0; i<game.GetPlayersCount(); i++ )
            {
                game.DealerFirstHit(i);
            }

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
                DC.DrawImage(GetShowTable(), 0, 0);
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
                DC.DrawImage(GetShowTable() , 0, 0);
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
                    DC.DrawImage( GetShowTable(), 0, 0 );
                }
            }

            Rectangle[] doublerects = new Rectangle[ BlackjackGame.MAX_PLAYERS ];
            for (int i = 0; i < game.GetPlayersCount(); i++)
            {
                doublerects[i] = new Rectangle(95 + 105 * i, 285, 30, 30);
                if (doublerects[i].Contains(mousePoint) && game.GetPlayerState(i) != PlayerState.STAND)
                {
                   game.SetPlayerState(i, PlayerState.DOUBLE);
                   DC.DrawImage(dbufBitmap, 0, 0);
                }
            }

            if (game.CheckStates())
                DealerHit();
        }
    }
}
