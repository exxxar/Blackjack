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
    public partial class MakeBetForm : Form
    {
        private Player player;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="p"></param>
        public MakeBetForm( Player p )
        {
            InitializeComponent();

            player = p;
            this.labelPlayerName.Text = p.Name;
        }
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonOK_Click(object sender, EventArgs e)
        {
            try
            {
                player.Stake = int.Parse(this.textBoxPlayerStake.Text);

                if (player.Stake >= BlackjackGame.MIN_STAKE)
                {
                    DialogResult = DialogResult.OK;
                }
                else
                {
                    MessageBox.Show(string.Format("Your bet must be at least {0} $", BlackjackGame.MIN_STAKE));    
                }
            }
            catch (FormatException)
            {
                MessageBox.Show("Wrong number format (the money field must contain only digits)");
            }
            
        }
    }
}
