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
        BlackjackGame game = new BlackjackGame();
        
        CardTableController gamecontroller = null;
        

        public MainForm()
        {
            InitializeComponent();

            gamecontroller = new CardTableController(game);
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
                foreach (Player p in form.players)
                {
                    MakeBetForm makeBetForm = new MakeBetForm(p);
                    makeBetForm.ShowDialog();
                    game.addPlayer(p);
                }

                gamecontroller.PrepareGraphics(this.Width, this.Height, CreateGraphics());
                gamecontroller.MoveCardToDealer();
            }
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MainForm_Paint(object sender, PaintEventArgs e)
        {
            gamecontroller.PrepareGraphics( this.Width, this.Height, e.Graphics );
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


        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MainForm_Click(object sender, EventArgs e)
        {
            if (((MouseEventArgs)e).Button == System.Windows.Forms.MouseButtons.Right)
            {
                //game.GetDealer().PlayerHand.AddCard(game.GetDeck(0).PopCard());
                game.DealerActions();
                CreateGraphics().DrawImage(gamecontroller.GetShowTable(), 0, 0);
            }
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MainForm_MouseClick(object sender, MouseEventArgs e)
        {
            Rectangle[] hitrects = new Rectangle[7];
            for (int i = 0; i < 7; i++)
            {
                hitrects[i] = new Rectangle(25 + 105*i, 285, 30, 30);
                if ( hitrects[i].Contains(e.Location) && !game.IsStand( i ) )
                {
                    gamecontroller.PrepareGraphics(this.Width, this.Height, CreateGraphics());
                    gamecontroller.MoveCardToPlayer( i );
                }
            }

            Rectangle[] standrects = new Rectangle[7];
            for (int i = 0; i < 7; i++)
            {
                standrects[i] = new Rectangle(60 + 105 * i, 285, 30, 30);
                if (standrects[i].Contains(e.Location))
                {
                    game.PlayerStand(i);
                    CreateGraphics().DrawImage( gamecontroller.GetShowTable(), 0, 0 );
                }
            }

            Rectangle[] doublerects = new Rectangle[7];
            for (int i = 0; i < 7; i++)
            {
                doublerects[i] = new Rectangle(95 + 105 * i, 285, 30, 30);
                if (doublerects[i].Contains(e.Location) && !game.IsStand( i ) )
                {
                    try
                    {
                        game.PlayerDouble(i);
                        CreateGraphics().DrawImage( gamecontroller.GetShowTable(), 0, 0 );
                    }
                    catch (InvalidOperationException iex)
                    {
                        MessageBox.Show( "You've not enough money!" );
                    }
                }
            }
        }
    }
}
