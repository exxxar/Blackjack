using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;


namespace Blackjack
{
    /// <summary>
    /// 
    /// </summary>
    public partial class MainForm : Form
    {
        /// <summary>
        /// 
        /// </summary>
        private BlackjackGame game = null;
        
        /// <summary>
        /// 
        /// </summary>
        private CardTableController gamecontroller = null;
        
        /// <summary>
        /// 
        /// </summary>
        private CardTableVisualizer gamevisualizer = null;

        /// <summary>
        /// 
        /// </summary>
        private PlayerStats statistics = new PlayerStats();
        
        /// <summary>
        /// 
        /// </summary>
        public int curShuffle = 0;
        

        /// <summary>
        /// 
        /// </summary>
        public MainForm()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 
        /// </summary>
        public void FixGameResults()
        {
            // ... after the game is over
            for (int i = 0; i < game.GetPlayersCount(); i++)
                statistics.AddShuffleResult( game.GetPlayer(i), curShuffle );

            curShuffle++;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MainForm_Load(object sender, EventArgs e)
        {
            PlayersForm form = new PlayersForm();
            if (form.ShowDialog() == DialogResult.OK)
            {
                statistics = form.gameStats;

                game = new BlackjackGame();

                gamevisualizer = new CardTableVisualizer(game);
                gamevisualizer.PrepareGraphics(this.Width, this.Height, CreateGraphics());

                gamecontroller = new CardTableController(game, gamevisualizer, FixGameResults);

                game.SetPlayerList( form.GetActivePlayers() );

                NewGame();
            }
            else
            {
                Close();
            }
        }



        /// <summary>
        /// 
        /// </summary>
        private void ChangePlayers()
        {
            PlayersForm form = new PlayersForm();
            form.gameStats = statistics;
            form.ShowDialog();
            statistics = form.gameStats;

            if (form.bChangedPlayer)
            {
                game.SetPlayerList( form.GetActivePlayers() );

                NewGame();
            }
        }


        /// <summary>
        /// Method starts new game (new shuffle).
        /// Method checks if particular player has enough money to proceed and proposes to make bets
        /// </summary>
        private void NewGame()
        {
            // exclude all those players who have not enough money to cannot pay
            game.RemovePlayersMinStake();

            // check if there's at least one player who can do minimal bet
            if (game.GetPlayersCount() == 0)
            {
                MessageBox.Show( "There are no more players or all players have not enough money to play!" );
                gamevisualizer.Invalidate();
                return;
            }

            // propose to do the bet for each player
            for (int i = 0; i < game.GetPlayersCount(); i++)
            {
                MakeBetForm makeBetForm = new MakeBetForm(game.GetPlayer(i));
                makeBetForm.ShowDialog();
            }

            // Redraw the form
            gamevisualizer.Invalidate();
            
            // ...And start new game
            gamecontroller.StartNewShuffle();
        }
       

        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MainForm_MouseClick(object sender, MouseEventArgs e)
        {
            if (gamevisualizer.bPlayersHighlight)
            {
                ChangePlayers();
            }
            else if (gamevisualizer.bNewGameHighlight)
            {
                if (!game.CheckStates())
                {
                    MessageBox.Show("Please wait until the end of current game!");
                    return;
                }
                NewGame();
            }
            else if ( !game.CheckGameFinished() )
            {
                try
                {
                    gamecontroller.UserActions(e.Location);
                }
                catch (InvalidOperationException)
                {
                    MessageBox.Show("You've not enough money to double down!");
                }
            }
        }


        /// <summary>
        /// New game (new shuffle) can be initiated by user pressing F2
        /// </summary>
        /// <param name="sender">The sender of KeyDown event</param>
        /// <param name="e">The arguments of KeyDown event</param>
        private void MainForm_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.F2)
            {
                if (!game.CheckStates())
                {
                    MessageBox.Show("Please wait until the end of current game!");
                    return;
                }
                NewGame();
            }
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MainForm_MouseMove(object sender, MouseEventArgs e)
        {
            Graphics DC = CreateGraphics();
            Rectangle playerRect = new Rectangle(735, 5, 30, 40);
            Rectangle newGameRect = new Rectangle(60, 10, 180, 20);

            gamevisualizer.bPlayersHighlight = (playerRect.Contains( e.Location) ) ? true : false;
            gamevisualizer.bNewGameHighlight = (newGameRect.Contains(e.Location)) ? true : false;
            gamevisualizer.Invalidate();
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MainForm_Paint(object sender, PaintEventArgs e)
        {
            e.Graphics.DrawImage(gamevisualizer.GetShowTable(), 0, 0);
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MainForm_Resize(object sender, EventArgs e)
        {
            Graphics DC = CreateGraphics();
            gamevisualizer.PrepareGraphics(this.Width, this.Height, DC);
            DC.DrawImage(gamevisualizer.GetShowTable(), 0, 0);
        }
    }
}
