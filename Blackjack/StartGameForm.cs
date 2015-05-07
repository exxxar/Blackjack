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
    public partial class StartGameForm : Form
    {
        public List<Player> players = new List<Player>();

        public StartGameForm()
        {
            InitializeComponent();
        }

        public void UpdateViews()
        {
            this.listBoxPlayers.Items.Clear();

            foreach (Player p in players)
                this.listBoxPlayers.Items.Add(
                    string.Format("{0} ({1})", p.Name, p.Money.ToString()) );
        }

        public void buttonAddPlayerClick(object sender, EventArgs e)
        {
            players.Add(new Player(this.textBoxPlayerName.Text, int.Parse(this.textBoxPlayerMoney.Text)) );
            UpdateViews();
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
