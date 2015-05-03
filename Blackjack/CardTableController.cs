using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
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
        public Point[] cardsCoords = new Point[7];

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
                cardsCoords[i].X = 30 + 105*i;
                cardsCoords[i].Y = 320;
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


        public void ShowDealerHand()
        {
            for (int i = 0; i < game.GetDealer().PlayerHand.GetCardsNumber(); i++ )
                g.DrawImage(cardImages[game.GetDealer().PlayerHand[i].getNumber()],
                    dealerCoords.X + 30*i, dealerCoords.Y, 90, 130);
        }


        public void ShowPlayerHand( int nPlayer )
        {
            for (int i = 0; i < game.GetPlayer(nPlayer).PlayerHand.GetCardsNumber(); i++)
                g.DrawImage(cardImages[game.GetPlayer(nPlayer).PlayerHand[i].getNumber()],
                    cardsCoords[nPlayer].X, cardsCoords[nPlayer].Y + 30 * i, 90, 130);
        }


        public void DrawTable()
        {
            g.FillRectangle(new LinearGradientBrush(new Point(0, 0), new Point(dbufBitmap.Width, dbufBitmap.Height),
                                                                        Color.LightGreen, Color.DarkGreen),
                                                   0, 0, dbufBitmap.Width, dbufBitmap.Height);
            
            Pen whitePen = new Pen(Color.White, 3);
            Brush whiteBrush = new SolidBrush(Color.White);
            Font textFont = new Font("Broadway", 15);
            for (int i = 0; i < 7; i++)
            {
                g.DrawRectangle(whitePen, cardsCoords[i].X, cardsCoords[i].Y, 90, 130);
                g.DrawString(game.GetPlayer(i).Name, textFont, whiteBrush, 30 + 105 * i, 230);
                g.DrawString(game.GetPlayer(i).Money + " $", textFont, whiteBrush, 30 + 105 * i, 252);

                if (game.IsStand(i))
                {
                    g.DrawImage(new Bitmap(@"d:\projects\github\Blackjack\Blackjack\images\stand_fix.jpeg"), 60 + 105 * i, 285, 30, 30);
                }
                else
                {
                    g.DrawImage(new Bitmap(@"d:\projects\github\Blackjack\Blackjack\images\hit.jpeg"), 25 + 105 * i, 285, 30, 30);
                    g.DrawImage(new Bitmap(@"d:\projects\github\Blackjack\Blackjack\images\stand.jpeg"), 60 + 105 * i, 285, 30, 30);
                    g.DrawImage(new Bitmap(@"d:\projects\github\Blackjack\Blackjack\images\double.png"), 95 + 105 * i, 285, 30, 30);
                }
            }

            g.DrawRectangle(whitePen, 500, 50, 90, 130);
            g.DrawRectangle(whitePen, 600, 50, 90, 130);
            g.DrawString("Dealer", textFont, whiteBrush, 560, 20);

            whitePen.Dispose();
            whiteBrush.Dispose();
            textFont.Dispose();
        }


        private void DrawShoes()
        {
            for (int i = 0; i < BlackjackGame.DECKS_COUNT; i++)
            {
                game.GetDeck(i).Shuffle();
                for (int j = 0; j < game.GetDeck(i).GetCardsNumber(); j+=7)
                {
                    g.DrawImage(cardBack, 100 * i + 10 + j/5, 50 + j/7, 90, 130);
                }
            }
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
            for (int i = 0; i < 7; i++ )
                ShowPlayerHand( i );

            return dbufBitmap;
        }


        public void MoveCardToDealer(int nDeck, int nPlayer)
        {
        }


        public void MoveCardToPlayer( int nDeck, int nPlayer )
        {
        }
    }
}
