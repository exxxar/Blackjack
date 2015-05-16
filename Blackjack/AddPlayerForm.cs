using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Blackjack
{
    /// <summary>
    /// 
    /// </summary>
    public enum PlayerMode
    {
        PM_ADD,
        PM_EDIT
    }


    /// <summary>
    /// 
    /// </summary>
    public partial class AddPlayerForm : Form
    {
        private PlayerMode windowMode;
        private Player player = null;
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="pm"></param>
        /// <param name="p"></param>
        public AddPlayerForm( PlayerMode pm = PlayerMode.PM_ADD, Player p  = null)
        {
            InitializeComponent();

            windowMode = pm;

            if (pm == PlayerMode.PM_EDIT)
            {
                windowMode = PlayerMode.PM_EDIT;
                this.buttonAddPlayer.Text = "OK";
                player = p;

                this.textBoxPlayerName.Text = p.Name;
                this.textBoxPlayerMoney.Text = p.Money.ToString();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public Player GetPlayer()
        {
            return player;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void buttonAddPlayerClick(object sender, EventArgs e)
        {
            try
            {
                if (windowMode == PlayerMode.PM_ADD)
                    player = new Player(this.textBoxPlayerName.Text, int.Parse(this.textBoxPlayerMoney.Text));
                else
                {
                    player.Name = this.textBoxPlayerName.Text;
                    player.Money = int.Parse(this.textBoxPlayerMoney.Text);
                }
                DialogResult = DialogResult.OK;
            }
            catch (FormatException)
            {
                MessageBox.Show( "Wrong number format (the money field must contain only digits)" );
            }
        }

        private void buttonOK_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.OK;
        }

        private void buttonCancel_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
