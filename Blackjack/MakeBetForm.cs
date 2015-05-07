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
    public partial class MakeBetForm : Form
    {
        private Player player;

        public MakeBetForm( Player p )
        {
            InitializeComponent();

            player = p;
            this.labelPlayerName.Text = p.Name;
        }

        private void buttonOK_Click(object sender, EventArgs e)
        {
            player.Stake = int.Parse( this.textBoxPlayerStake.Text );
            DialogResult = DialogResult.OK;
        }
    }
}
