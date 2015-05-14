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
    public partial class MainForm : Form
    {
        BlackjackGame game = null;
        CardTableController gamecontroller = null;

        private bool bPlayerChange = false;
        

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
            form.ShowDialog();
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MainForm_Load(object sender, EventArgs e)
        {
            StartGameForm form = new StartGameForm();
            if (form.ShowDialog() == DialogResult.OK)
            {
                game = new BlackjackGame();

                foreach (Player p in form.players)
                {
                    MakeBetForm makeBetForm = new MakeBetForm(p);
                    makeBetForm.ShowDialog();

                    game.addPlayer(p);
                }

                gamecontroller = new CardTableController(game);

                gamecontroller.PrepareGraphics(this.Width, this.Height, CreateGraphics());
                gamecontroller.StartNewShuffle();
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
            }
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MainForm_MouseMove(object sender, MouseEventArgs e)
        {
            Rectangle playerRect = new Rectangle(735, 5, 30, 40);
            if (playerRect.Contains(e.Location))
            {
                CreateGraphics().DrawImage(Properties.Resources.player, 735, 55, 40, 54);
                bPlayerChange = true;
            }
            else
            {
                CreateGraphics().DrawImage(gamecontroller.GetShowTable(), 0, 0);
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
            e.Graphics.DrawImage(gamecontroller.GetShowTable(), 0, 0);
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MainForm_Resize(object sender, EventArgs e)
        {
            Graphics DC = CreateGraphics();
            gamecontroller.PrepareGraphics(this.Width, this.Height, DC);
            DC.DrawImage(gamecontroller.GetShowTable(), 0, 0);
        }
    }
}
