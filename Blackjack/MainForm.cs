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
        private async void GiveTheFirstCards()
        {
            await Task.Delay( 400 );
            gamecontroller.MoveCardToDealer();

            for (int i = 0; i < game.GetPlayersCount(); i++)
            {
                await Task.Delay( 400 );
                gamecontroller.MoveCardToPlayer(i);
                await Task.Delay( 400 );
                gamecontroller.MoveCardToPlayer(i);
            }
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
                GiveTheFirstCards();
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
        private void MainForm_Paint(object sender, PaintEventArgs e)
        {
            e.Graphics.DrawImage( gamecontroller.GetShowTable(), 0, 0);
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MainForm_Resize(object sender, EventArgs e)
        {
            gamecontroller.PrepareGraphics(this.Width, this.Height, CreateGraphics());
            CreateGraphics().DrawImage(gamecontroller.GetShowTable(), 0, 0);
        }


        private void ChangePlayers()
        {

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

            Rectangle[] hitrects = new Rectangle[7];
            for (int i = 0; i < game.GetPlayersCount(); i++)
            {
                hitrects[i] = new Rectangle(25 + 105*i, 285, 30, 30);
                if ( hitrects[i].Contains(e.Location) && game.GetPlayerState(i) != PlayerState.STAND )
                {
                    gamecontroller.MoveCardToPlayer( i );
                }
            }

            Rectangle[] standrects = new Rectangle[7];
            for (int i = 0; i < game.GetPlayersCount(); i++)
            {
                standrects[i] = new Rectangle(60 + 105 * i, 285, 30, 30);
                if (standrects[i].Contains(e.Location) && game.GetPlayerState(i) != PlayerState.BUST )
                {
                    game.SetPlayerState(i, PlayerState.STAND);
                    this.Invalidate();
                }
            }

            Rectangle[] doublerects = new Rectangle[7];
            for (int i = 0; i < game.GetPlayersCount(); i++)
            {
                doublerects[i] = new Rectangle(95 + 105 * i, 285, 30, 30);
                if (doublerects[i].Contains(e.Location) && game.GetPlayerState(i) != PlayerState.STAND)
                {
                    try
                    {
                        game.PlayerDouble(i);
                        this.Invalidate();
                    }
                    catch (InvalidOperationException iex)
                    {
                        MessageBox.Show( "You've not enough money!" );
                    }
                }
            }

            if (game.CheckStates())
                gamecontroller.DealerHit();
        }

        private void MainForm_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.F2)
            {
                game.Shuffle();
                
                for (int i = 0; i < game.GetPlayersCount(); i++)
                {
                    MakeBetForm makeBetForm = new MakeBetForm( game.GetPlayer(i) );
                    makeBetForm.ShowDialog();
                }

                gamecontroller.PrepareGraphics(this.Width, this.Height, CreateGraphics());
                GiveTheFirstCards();
            }
        }


        private void MainForm_MouseMove(object sender, MouseEventArgs e)
        {
            Rectangle playerRect = new Rectangle(735, 5, 30, 40);
            if (playerRect.Contains(e.Location))
            {
                CreateGraphics().DrawImage(Properties.Resources.player, 735, 5, 40, 54);
                bPlayerChange = true;
            }
            else
            {
                CreateGraphics().DrawImage(gamecontroller.GetShowTable(), 0, 0);
                bPlayerChange = false;
            }
        }
    }
}
