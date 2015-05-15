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
        BlackjackGame game = null;
        CardTableController gamecontroller = null;
        CardTableVisualizer gamevisualizer = null;

        PlayerStats statistics = new PlayerStats();
        public int curShuffle = 0;

        private bool bPlayerChange = false;
        

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
        private void ChangePlayers()
        {
            PlayersForm form = new PlayersForm();
            form.gameStats = statistics;
            form.ShowDialog();
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MainForm_Load(object sender, EventArgs e)
        {
            //StartGameForm form = new StartGameForm();
            PlayersForm form = new PlayersForm();
            if (form.ShowDialog() == DialogResult.OK)
            {
                statistics = form.gameStats;

                game = new BlackjackGame();

                foreach (Player p in form.GetActivePlayers() )
                {
                    MakeBetForm makeBetForm = new MakeBetForm(p);
                    makeBetForm.ShowDialog();

                    game.addPlayer(p);
                }

                gamevisualizer = new CardTableVisualizer(game);
                gamevisualizer.PrepareGraphics(this.Width, this.Height, CreateGraphics());

                gamecontroller = new CardTableController(game, gamevisualizer);
                gamecontroller.StartNewShuffle();

                for (int i = 0; i < game.GetPlayersCount(); i++ )
                    statistics.gameResults.Add(new BlackjackResult( game.GetPlayer(i), curShuffle++) );
            }
            else
            {
                Close();
            }
        }
       

        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MainForm_MouseClick(object sender, MouseEventArgs e)
        {
            if ( bPlayerChange )
            {
                ChangePlayers();
            }

            try
            {
                gamecontroller.UserActions( e.Location );
            }
            catch (InvalidOperationException)
            {
                MessageBox.Show( "You've not enough money to double down!" );
            }
        }


        /// <summary>
        /// New game (new shuffle) can be started by user after pressing F@
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MainForm_KeyDown(object sender, KeyEventArgs e)
        {
            //if (!game.CheckGameFinished())
            if (!game.CheckStates())
            {
                MessageBox.Show("Please wait until the end of current game!");
                return;
            }

            if (e.KeyCode == Keys.F2)
            {
                for (int i = 0; i < game.GetPlayersCount(); i++)
                {
                    MakeBetForm makeBetForm = new MakeBetForm( game.GetPlayer(i) );
                    makeBetForm.ShowDialog();
                }

                // Redraw the form
                this.Invalidate();

                // ...And start new game
                gamecontroller.StartNewShuffle();

                for (int i = 0; i < game.GetPlayersCount(); i++)
                    statistics.gameResults.Add(new BlackjackResult(game.GetPlayer(i), curShuffle++));
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

            if (playerRect.Contains(e.Location))
            {
                DC.DrawImage(Properties.Resources.player, 735, 55, 40, 54);
                bPlayerChange = true;
            }
            else
            {
                DC.DrawImage(gamevisualizer.GetShowTable(), 0, 0);
                bPlayerChange = false;
            }
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
