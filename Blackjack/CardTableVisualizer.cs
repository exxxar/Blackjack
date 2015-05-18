using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Reflection;
using System.Resources;


namespace Blackjack
{
    /// <summary>
    /// The class responsible for double-buffered drawing of all objects on a card table during particular game (shuffle)
    /// Notes:
    /// 1) the class stores and processes the coordinates and size of slots for cards of players and a dealer
    /// 2) the card images are taken from the Cards.dll assembly. This assembly should be located in the same folder as the main assembly
    /// </summary>
    public class CardTableVisualizer : IDisposable
    {
        // The reference to a main game to be visualized
        private BlackjackGame game = null;

        // Device contexts and the bitmap for drawing
        private Graphics DC;
        private Graphics g = null;
        private Bitmap dbufBitmap = null;

        // Bitmaps for cards
        private Bitmap cardBack = null;
        private Bitmap[] cardImages = new Bitmap[ Deck.DECK_SIZE ];

        // The size of cards to draw (yep, we can change these parameters)
        private int drawCardWidth = 90;
        private int drawCardHeight = 130; 

        // The coordinates of a dealer, players, shoes
        Point dealerCoords;
        private Point[] playerCoords = new Point[BlackjackGame.MAX_PLAYERS];
        private Point[] shoesCoords = new Point[BlackjackGame.DECKS_COUNT];

        // the array of coordinates of the lowest card in each deck
        private Point[] shoesCoordsToDraw = new Point[BlackjackGame.DECKS_COUNT];
        

        /// <summary>
        /// The boolean variable that indicates whether the mouse is over the "Players" icon
        /// </summary>
        public bool bPlayersHighlight = false;

        /// <summary>
        /// The boolean variable that indicates whether the mouse is over the "New game" text
        /// </summary>
        public bool bNewGameHighlight = false;
        

        /// <summary>
        /// The CardTableVisualizer constructor.
        /// It tries to load the Cards.dll assembly and each card from it. If some problems occur the corresponding message is shown and the program is terminated.
        /// It sets the main parameters of the class
        /// </summary>
        /// <param name="blackjackgame">The game to visualize</param>
        public CardTableVisualizer( BlackjackGame blackjackgame )
        {
            game = blackjackgame;

            // try to load the Cards.dll assembly and all cards from it
            try
            {
                Assembly cardsAssembly = Assembly.LoadFrom("Cards.dll");
                ResourceManager rm = new ResourceManager("Cards.Properties.Resources", cardsAssembly);
                Bitmap b = (Bitmap)rm.GetObject("back");
                cardBack = new Bitmap(b, drawCardWidth, drawCardHeight);                // resize the bitmap

                for (int i = 0; i < 52; i++)
                {
                    b = (Bitmap)rm.GetObject("_" + i);
                    cardImages[i] = new Bitmap(b, drawCardWidth, drawCardHeight);       // resize the bitmap
                }
            }
            catch (Exception)
            {
                System.Windows.Forms.MessageBox.Show( "Some problems occured with the Cards.dll or a card image from it" );
                System.Windows.Forms.Application.Exit();
            }

            // ------------------------------------------------------------------ set up the coordinates
            dealerCoords.X = 500;
            dealerCoords.Y = 50;

            for (int i = 0; i < BlackjackGame.MAX_PLAYERS; i++)
            {
                playerCoords[i].X = 30 + (15 + drawCardWidth) * i;
                playerCoords[i].Y = 320;
            }

            for (int i = 0; i < BlackjackGame.DECKS_COUNT; i++)
            {
                shoesCoords[i].X = 10 + (10 + drawCardWidth) * i;
                shoesCoords[i].Y = 50;
            }
        }



        #region DoubleBuffering things

        /// <summary>
        /// The method is responsible for double buffering.
        /// It creates internal temporary bitmap and Graphics object according to the parameters
        /// </summary>
        /// <param name="w">The width of a bitmap for double buffering</param>
        /// <param name="h">The height of a bitmap for double buffering</param>
        /// <param name="realGraphics">Real device context to draw on</param>
        public void PrepareGraphics(int w, int h, Graphics realGraphics)
        {
            DC = realGraphics;

            if (dbufBitmap != null)
                dbufBitmap.Dispose();

            dbufBitmap = new Bitmap(w, h);
            g = Graphics.FromImage(dbufBitmap);
        }
        

        /// <summary>
        /// Returns the entire double buffered bitmap with all card table objects for drawing
        /// (the method can be used by any class responsivle for drawing)
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
        /// Draws the double buffered bitmap (does the refreshing of a window drawing area, basically)
        /// </summary>
        public void Invalidate()
        {
            DC.DrawImage( GetShowTable(), 0, 0);
        }


        /// <summary>
        /// Method draws a single card on top of a deck
        /// </summary>
        /// <param name="card">The card to draw</param>
        /// <param name="nDeck">The deck from which we got this card</param>
        public void DrawCard(Card card, int nDeck)
        {
            g.DrawImage( cardImages[card.getNumber()],
                shoesCoordsToDraw[nDeck].X, shoesCoordsToDraw[nDeck].Y,
                drawCardWidth, drawCardHeight);
            DC.DrawImage( dbufBitmap, 0, 0 );
        }


        /// <summary>
        /// The method draws all cards in dealer's hand (shifted horizontally by 30 pixels)
        /// </summary>
        public void ShowDealerHand()
        {
            for (int i = 0; i < game.GetDealer().PlayerHand.GetCardsNumber(); i++)
                g.DrawImage( cardImages[game.GetDealer().PlayerHand[i].getNumber()],
                    dealerCoords.X + 30 * i, dealerCoords.Y,
                    drawCardWidth, drawCardHeight);
        }


        /// <summary>
        /// The method draws all cards in player's hand (shifted vaertically by 30 pixels)
        /// </summary>
        /// <param name="nPlayer"></param>
        public void ShowPlayerHand(int nPlayer)
        {
            for (int i = 0; i < game.GetPlayer(nPlayer).PlayerHand.GetCardsNumber(); i++)
            {
                g.DrawImage(cardImages[game.GetPlayer(nPlayer).PlayerHand[i].getNumber()],
                    playerCoords[nPlayer].X, playerCoords[nPlayer].Y + 30 * i,
                    drawCardWidth, drawCardHeight);
            }
        }


        /// <summary>
        /// The method draws the card table:
        /// 1) card slots
        /// 2) player names, money stakes
        /// 3) dealer's info
        /// 5) Players icon and New Game text
        /// </summary>
        public void DrawTable()
        {
            // gradient green fill of the table surface
            g.FillRectangle(new LinearGradientBrush(new Point(0, 0), new Point(dbufBitmap.Width, dbufBitmap.Height),
                                                                            Color.LightGreen, Color.DarkGreen),
                                                                            0, 0, dbufBitmap.Width, dbufBitmap.Height);

            // creating all necessary resources
            Pen whitePen = new Pen(Color.White, 3);
            Brush whiteBrush = new SolidBrush(Color.White);
            Brush blackBrush = new SolidBrush(Color.Black);
            Brush yellowBrush = new SolidBrush(Color.Yellow);
            Font textFont = new Font("Arial", 12);
            Font gameFont = new Font("Stencil", 16);

            // drawing card slots
            for (int i = 0; i < BlackjackGame.MAX_PLAYERS; i++)
            {
                g.DrawRectangle(whitePen, playerCoords[i].X, playerCoords[i].Y, drawCardWidth, drawCardHeight);
            }

            // drawing player names, stakes and money
            for (int i = 0; i < game.GetPlayersCount(); i++)
            {
                g.DrawString(game.GetPlayer(i).Name, textFont, whiteBrush, 30 + (15 + drawCardWidth) * i, 220);
                g.DrawString(game.GetPlayer(i).Money + " $", textFont, whiteBrush, 30 + (15 + drawCardWidth) * i, 240);
                g.DrawString(game.GetPlayer(i).Stake + " $", textFont, yellowBrush, 30 + (15 + drawCardWidth) * i, 260);
            }

            //  -------------------------------------------------------------- drawing dealer's info
            if (game.dealerBlackjack)
            {
                g.DrawImage(Properties.Resources.blackjack_21, 510, 7, 40, 40);
            }

            if (game.dealerBust)
            {
                g.DrawImage(Properties.Resources.bust, 510, 10, 35, 35);
            }
            
            g.DrawRectangle(whitePen, 500, 50, drawCardWidth, drawCardHeight);
            g.DrawRectangle(whitePen, 510 + drawCardWidth, 50, drawCardWidth, drawCardHeight);
            g.DrawString("Dealer", textFont, whiteBrush, 560, 20);

            string casinoMoneyString = string.Format("-{0} $", game.totalLose);
            if (game.totalLose < 0)
                casinoMoneyString = casinoMoneyString.Replace("--", "+");
            g.DrawString(casinoMoneyString, textFont, whiteBrush, 650, 20);

            // --------------------------------------------------------------------------------------

            // enlarges the Player icon if the mouse is over it 
            if (bPlayersHighlight)
            {
                g.DrawImage(Properties.Resources.player, 730, 5, 50, 60);
            }
            else
            {
                g.DrawImage(Properties.Resources.player, 735, 5, 30, 40);
            }

            // changes the color of the "New Game - F2" text into yellow if the mouse is over it
            if (bNewGameHighlight)
            {
                g.DrawString("New Game - F2", gameFont, yellowBrush, 60, 10);
            }
            else
            {
                g.DrawString("New Game - F2", gameFont, blackBrush, 60, 10);
            }

            // dispose all used resources
            whitePen.Dispose();
            whiteBrush.Dispose();
            blackBrush.Dispose();
            yellowBrush.Dispose();
            textFont.Dispose();
            gameFont.Dispose();

        }


        /// <summary>
        /// The method draws:
        /// 1) results of the game for each player
        /// 2) the set of possible options for each player at the current moment
        /// 3) additional things such as the Blackjack sign and bust sign
        /// </summary>
        public void DrawOptions()
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
                else if (game.GetPlayerState(i) == PlayerState.STAND)
                {
                    g.DrawImage(Properties.Resources.stand_fix, 60 + 105 * i, 285, 30, 30);
                }
                else
                {
                    g.DrawImage(Properties.Resources.hit, 25 + (15 + drawCardWidth) * i, 285, 30, 30);
                    g.DrawImage(Properties.Resources.stand, 60 + (15 + drawCardWidth) * i, 285, 30, 30);
                    g.DrawImage(Properties.Resources._double, 95 + (15 + drawCardWidth) * i, 285, 30, 30);
                }

                if (game.GetPlayerState(i) == PlayerState.BLACKJACK)
                {
                    g.DrawImage(Properties.Resources.blackjack_21, 90 + (15 + drawCardWidth) * i, 265, 40, 40);
                }
                if (game.GetPlayerState(i) == PlayerState.BUST)
                {
                    g.DrawImage(Properties.Resources.bust, 95 + (15 + drawCardWidth) * i, 265, 30, 30);
                }
            }
        }


        /// <summary>
        /// Method draws <see cref="BlackjackGame.DECKS_COUNT"/> shoes with "closed" cards
        /// and remembers the coordniates of the lowest card (since we pop cards from each deck every time) 
        /// </summary>
        public void DrawShoes()
        {
            for (int i = 0; i < BlackjackGame.DECKS_COUNT; i++)
            {
                int j = 0;
                for (; j < game.GetDeck(i).GetCardsNumber(); j += 7)
                {
                    g.DrawImage(cardBack, shoesCoords[i].X + j / 5, shoesCoords[i].Y + j / 7, drawCardWidth, drawCardHeight);
                }
                shoesCoordsToDraw[i].X = shoesCoords[i].X + j / 5;
                shoesCoordsToDraw[i].Y = shoesCoords[i].Y + j / 7;
            }
        }


        /// <summary>
        /// The universal method for drawing a rectangle with the result of a game for each player
        /// </summary>
        /// <param name="text">The text to draw (WIN, LOSE, STAY)</param>
        /// <param name="backgroundColor">The color of the rectangle background</param>
        /// <param name="textColor">The color of text to draw</param>
        /// <param name="borderColor">The color of the rectangle border</param>
        /// <param name="nPlayer">The players's number</param>
        public void DrawResult(string text, Color backgroundColor, Color textColor, Color borderColor, int nPlayer)
        {
            Pen pen = new Pen(borderColor);    
            Brush backgroundBrush = new SolidBrush(backgroundColor);
            Brush textBrush = new SolidBrush(textColor);

            g.DrawRectangle(pen, 25 + (15 + drawCardWidth) * nPlayer, 285, 100, 30);
            g.FillRectangle(backgroundBrush, 26 + (15 + drawCardWidth) * nPlayer, 286, 99, 29);
            g.DrawString(text, new Font("Stencil", 16), textBrush, 35 + (15 + drawCardWidth ) * nPlayer, 289);

            pen.Dispose();
            textBrush.Dispose();
            backgroundBrush.Dispose();
        }

        #endregion


        /// <summary>
        /// Conventional Dispose method (we dispose all previously used bitmaps)
        /// </summary>
        public void Dispose()
        {
            dbufBitmap.Dispose();
            cardBack.Dispose();
            for (int i = 0; i < Deck.DECK_SIZE; i++)
                cardImages[i].Dispose();
        }
    }
}
