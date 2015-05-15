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
    public partial class AddPlayerForm : Form
    {
        private Player player = null;


        public AddPlayerForm()
        {
            InitializeComponent();
        }

        public Player GetAddedPlayer()
        {
            return player;
        }

        public void buttonAddPlayerClick(object sender, EventArgs e)
        {
            try
            {
                player = new Player(this.textBoxPlayerName.Text, int.Parse(this.textBoxPlayerMoney.Text));
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
