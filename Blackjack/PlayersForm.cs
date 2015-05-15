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
    public partial class PlayersForm : Form
    {
        public PlayerStats gameStats { get; set; }


        public PlayersForm()
        {
            InitializeComponent();
        }


        private void buttonOK_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.OK;
            Close();
        }


        private void buttonRemove_Click(object sender, EventArgs e)
        {
            if (listViewPlayers.SelectedIndices.Count == 0)
                return;

            int selIdx = listViewPlayers.SelectedIndices[0];
            listViewPlayers.Items.RemoveAt( selIdx );
        }


        private void buttonAdd_Click(object sender, EventArgs e)
        {
            AddPlayerForm form = new AddPlayerForm();
            if (form.ShowDialog() == DialogResult.OK)
            {
                BlackjackResult res = new BlackjackResult();
                res.player = form.GetAddedPlayer();
                res.results = new List<PlayerResult>();
                res.shuffles = new List<int>();
                res.stakes = new List<int>();

                gameStats.gameResults.Add( res );

                ShowStatistics();
            }
        }


        private void PlayersForm_Load(object sender, EventArgs e)
        {
            gameStats = new PlayerStats();

            // get all players from stats!
            ShowStatistics();
        }


        private int TotalColumns()
        {
            int nColumns = 0;

            try
            {
                nColumns = gameStats.gameResults.Max(t => t.shuffles.Count);
            }
            catch (InvalidOperationException)
            {
                nColumns = 0;
            }

            return nColumns;
        }


        private void ShowStatistics()
        {
            listViewPlayers.Columns.Clear();
            listViewPlayers.Items.Clear();

            
            int nColumns = TotalColumns();


            listViewPlayers.Columns.Add("Name (Money)", 140);
            
            for (int i = 0; i < nColumns; i++)
                listViewPlayers.Columns.Add("Shuffle" + (i + 1), 80);

            for (int i = 0; i < gameStats.gameResults.Count; i++)
            {
                ListViewItem item = new ListViewItem(string.Format("{0} ({1} $)", 
                                                        gameStats.gameResults[i].player.Name,
                                                        gameStats.gameResults[i].player.Money));
                
                int k = 0;
                int pos = 0;
                foreach (var res in gameStats.gameResults[i].results)
                {
                    switch (res)
                    {
                        case PlayerResult.WIN:
                            item.SubItems[pos].Text = "+" + gameStats.gameResults[i].stakes[k++];
                            break;
                        case PlayerResult.LOSE:
                            item.SubItems[pos].Text = "-" + gameStats.gameResults[i].stakes[k++];
                            break;
                        case PlayerResult.STAY:
                            item.SubItems[pos].Text = "stay";
                            k++;
                            break;
                    }

                    pos++;
                }

                listViewPlayers.Items.Add( item );
            }
        }

        public IEnumerable<Player> GetActivePlayers()
        {
            var list = gameStats.gameResults.Where(p => p.player.Money > 100).ToList();
            List<Player> players = new List<Player>();

            for (int i = 0; i < list.Count; i++ )
                players.Add(list[i].player);

            return players;
        }
    }
}
