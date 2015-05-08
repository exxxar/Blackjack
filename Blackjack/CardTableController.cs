﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;


namespace Blackjack
{
    class CardTableController
    {
        private Graphics DC;
        private Graphics g = null;
        private Bitmap dbufBitmap = null;

        Bitmap cardBack = null;
        private Bitmap[] cardImages = new Bitmap [52];

        Point dealerCoords;
        public Point[] playerCoords = new Point[7];

        public Point[] shoesCoords = new Point[4];
        public Point[] shoesCoordsToDraw = new Point[4];

        BlackjackGame game = null;

        

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

            for (int i = 0; i < 7; i++)
            {
                playerCoords[i].X = 30 + 105*i;
                playerCoords[i].Y = 320;
            }

            for (int i = 0; i < 4; i++)
            {
                shoesCoords[i].X = 10 + 100 * i;
                shoesCoords[i].Y = 50;
            }
        }


        public void PrepareGraphics(int w, int h, Graphics realGraphics)
        {
            DC = realGraphics;

            if (dbufBitmap != null)
                dbufBitmap.Dispose();

            dbufBitmap = new Bitmap( w, h );
            g = Graphics.FromImage(dbufBitmap);
        }


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


        public void DrawTable()
        {
            g.FillRectangle(new LinearGradientBrush(new Point(0, 0), new Point(dbufBitmap.Width, dbufBitmap.Height),
                                                                        Color.LightGreen, Color.DarkGreen),
                                                   0, 0, dbufBitmap.Width, dbufBitmap.Height);

            
            Pen whitePen = new Pen(Color.White, 3);
            Brush whiteBrush = new SolidBrush(Color.White);
            Brush yellowBrush = new SolidBrush(Color.Yellow);
            Font textFont = new Font("Arial", 12);

            for (int i = 0; i < 7; i++)
            {
                g.DrawRectangle(whitePen, playerCoords[i].X, playerCoords[i].Y, 90, 130);
            }

            for (int i = 0; i < game.GetPlayersCount(); i++)
            {
                g.DrawString(game.GetPlayer(i).Name, textFont, whiteBrush, 30 + 105 * i, 220);
                g.DrawString(game.GetPlayer(i).Money + " $", textFont, whiteBrush, 30 + 105 * i, 240);
                g.DrawString(game.GetPlayer(i).Stake + " $", textFont, yellowBrush, 30 + 105 * i, 260);
            }

            DrawOptions();

            g.DrawRectangle(whitePen, 500, 50, 90, 130);
            g.DrawRectangle(whitePen, 600, 50, 90, 130);
            g.DrawString("Dealer", textFont, whiteBrush, 560, 20);
 
            g.DrawImage(new Bitmap(Properties.Resources.player), 735, 5, 30, 40);
            g.DrawString("+ 0 - $", textFont, whiteBrush, 650, 20);


            whitePen.Dispose();
            whiteBrush.Dispose();
            yellowBrush.Dispose();
            textFont.Dispose();
        }


        private void DrawOptions()
        {
            for (int i = 0; i < game.GetPlayersCount(); i++)
            {
                if ( game.GetPlayerState(i) == PlayerState.STAND)
                {
                    g.DrawImage(new Bitmap(Properties.Resources.stand_fix), 60 + 105 * i, 285, 30, 30);
                }
                else if (game.GetPlayerState(i) == PlayerState.BUST)
                {
                    DrawBust( i );
                }
                else
                {
                    g.DrawImage(new Bitmap(Properties.Resources.hit), 25 + 105 * i, 285, 30, 30);
                    g.DrawImage(new Bitmap(Properties.Resources.stand), 60 + 105 * i, 285, 30, 30);
                    g.DrawImage(new Bitmap(Properties.Resources._double), 95 + 105 * i, 285, 30, 30);
                }
            }
        }

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



        public void DrawBust(int nPlayer)
        {
            Pen redPen = new Pen(Color.Red);
            Brush whiteBrush = new SolidBrush(Color.White);
            Brush lightRedBrush = new SolidBrush(Color.LightCoral);

            g.DrawRectangle(redPen, 25 + 105 * nPlayer, 285, 100, 30);
            g.FillRectangle(lightRedBrush, 26 + 105 * nPlayer, 286, 99, 29);
            g.DrawString("BUST!", new Font("Stencil", 16), whiteBrush, 40 + 105 * nPlayer, 289);

            redPen.Dispose();
            whiteBrush.Dispose();
            lightRedBrush.Dispose();
        }



        public Bitmap GetShowTable()
        {
            // draw the entire table
            DrawTable();

            // draw shoes
            DrawShoes();

            // draw all dealer's cards
            ShowDealerHand();

            // draw the cards of all players
            for (int i = 0; i < game.GetPlayersCount(); i++ )
                ShowPlayerHand( i );

            return dbufBitmap;
        }












        public /*async*/ void MoveCardToDealer()
        {
            Random r = new Random();
            int nDeck = r.Next(4);
            
            Hand dealerHand = game.GetDealer().PlayerHand;
            Card card = game.GetDeck( nDeck ).PopCard();
            dealerHand.AddCard( card );

            //Animate
            DrawCard( card.getNumber(), shoesCoordsToDraw[nDeck].X, shoesCoordsToDraw[nDeck].Y );
            DC.DrawImage( dbufBitmap, 0, 0 );

            //await Task.Delay( 500 );
            Thread.Sleep(200);

            DrawShoes();

            ShowDealerHand();
            DC.DrawImage(dbufBitmap, 0, 0);
        }


        public /*async*/ void MoveCardToPlayer( int nPlayer )
        {
            int nDeck;
            Card card = game.ChooseCard( out nDeck );

            //Animate
            DrawCard(card.getNumber(), shoesCoordsToDraw[nDeck].X, shoesCoordsToDraw[nDeck].Y);
            DC.DrawImage(dbufBitmap, 0, 0);

            Thread.Sleep( 200 );
            //await Task.Delay(500);       can't figure out why this doesn't work just like Thread.Sleep()...
            
            DrawShoes();

            try
            {
                game.PlayerHit(nPlayer, card);
            }
            catch (BustException bustEx)
            {
                game.SetPlayerState( nPlayer, PlayerState.BUST );
            }
            finally
            {
                ShowPlayerHand(nPlayer);
                DC.DrawImage(dbufBitmap, 0, 0);
            }

        }

        

        public void DealerHit()
        {
            for ( int i=0; i<game.GetPlayersCount(); i++ )
            {
                if (game.GetPlayer(i).PlayerHand.GetCardsNumber() == 2 && game.GetPlayer(i).CountScore() == 21)
                {
                    game.DealerHit( i );
                }
            }

            if (game.CheckAllBusted())
            {
                for (int i = 0; i < game.GetPlayersCount(); i++)
                {
                    game.GetPlayer(i).LoseStake();
                }
                return;
            }

            try
            {
                for (int i = 0; i < game.GetPlayersCount(); i++)
                {
                    game.DealerHit(i);

                    while (game.GetDealer().CountScore() < 17)		// дилер здесь добирает карты, пока у него нет 17
                    {
                        MoveCardToDealer();				            // здесь возможен эксепшн! (он перехватывается в функции уровнем выше)
                    }
                }
            }
            catch (BustException bustEx)
            {
                System.Windows.Forms.MessageBox.Show("DEALER BUST!");
            }

            for (int i = 0; i < game.GetPlayersCount(); i++)
            {
                game.SetBonuses(i);
                System.Windows.Forms.MessageBox.Show(game.PlayResults(i).ToString());
            }
        }
    }
}
