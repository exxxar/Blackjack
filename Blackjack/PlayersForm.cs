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
        /// <summary>
        /// 
        /// </summary>
        public bool bChangedPlayer = false;

        /// <summary>
        /// 
        /// </summary>
        public PlayerStats gameStats { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public PlayersForm()
        {
            InitializeComponent();

            gameStats = new PlayerStats();
        }


        private void PlayersForm_Load(object sender, EventArgs e)
        {
            ShowStatistics();
        }


        private void buttonOK_Click(object sender, EventArgs e)
        {
            if (listViewPlayers.Items.Count > 0)
            {
                DialogResult = DialogResult.OK;
                Close();
            }
        }


        private void buttonRemove_Click(object sender, EventArgs e)
        {
            if (listViewPlayers.SelectedIndices.Count == 0)
                return;

            int selIdx = listViewPlayers.SelectedIndices[0];
            listViewPlayers.Items.RemoveAt( selIdx );
            gameStats.gameResults.RemoveAt( selIdx );

            bChangedPlayer = true;
        }


        private void buttonAdd_Click(object sender, EventArgs e)
        {
            if (GetActivePlayers().Count() == BlackjackGame.MAX_PLAYERS)
            {
                MessageBox.Show( string.Format(
                                    "You can't add a new player to the game!\nAll {0} players are in the game!",
                                    BlackjackGame.MAX_PLAYERS ) );
                return;
            }

            AddPlayerForm form = new AddPlayerForm();
            if (form.ShowDialog() == DialogResult.OK)
            {
                BlackjackResult res = new BlackjackResult();
                res.player = form.GetPlayer();
                res.results = new List<PlayerResult>();
                res.shuffles = new List<int>();
                res.stakes = new List<int>();

                gameStats.gameResults.Add( res );


                int nColumns = TotalColumns();

                ListViewItem item = new ListViewItem(string.Format("{0} ({1} $)", form.GetPlayer().Name, form.GetPlayer().Money) );
                listViewPlayers.Items.Add(item);

                for (int i = 0; i < nColumns; i++)
                {
                    item.SubItems.Add("");
                }

                ShowStatistics();

                bChangedPlayer = true;
            }
        }


        private void buttonEdit_Click(object sender, EventArgs e)
        {
            if (listViewPlayers.SelectedIndices.Count > 0)
            {
                int nPlayer = listViewPlayers.SelectedIndices[0];
                AddPlayerForm form = new AddPlayerForm( PlayerMode.PM_EDIT, gameStats.gameResults[nPlayer].player );
                form.ShowDialog();

                string s = string.Format("{0} ({1} $)", form.GetPlayer().Name, form.GetPlayer().Money );
                listViewPlayers.Items[nPlayer].Text = s;

                bChangedPlayer = true;
            }
        }


        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
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


        /// <summary>
        /// 
        /// </summary>
        private void ShowStatistics()
        {
            listViewPlayers.Columns.Clear();
            listViewPlayers.Items.Clear();


            int nColumns = TotalColumns();


            listViewPlayers.Columns.Add("Name (Money)", 140);

            for (int i = 0; i < nColumns; i++)
            {
                listViewPlayers.Columns.Add("Shuffle" + (i + 1), 80);
            }

            for (int j = 0; j < gameStats.gameResults.Count; j++)
            {
                ListViewItem item = new ListViewItem(string.Format("{0} ({1} $)",
                                                        gameStats.gameResults[j].player.Name,
                                                        gameStats.gameResults[j].player.Money));

                for (int i = 0; i < nColumns; i++)
                    item.SubItems.Add("");

                listViewPlayers.Items.Add(item);
            }
            

            for (int i = 0; i < gameStats.gameResults.Count; i++)
            {
                ListViewItem item = listViewPlayers.Items[i];

                int sn = 0;

                foreach (var res in gameStats.gameResults[i].results)
                {
                    int shuffleNo = gameStats.gameResults[i].shuffles[sn];

                    switch (res)
                    {
                        case PlayerResult.WIN:
                            item.SubItems[shuffleNo+1].Text = "+" + gameStats.gameResults[i].stakes[sn];
                            break;
                        case PlayerResult.LOSE:
                            item.SubItems[shuffleNo+1].Text = "-" + gameStats.gameResults[i].stakes[sn];
                            break;
                        case PlayerResult.STAY:
                            item.SubItems[shuffleNo+1].Text = string.Format("stay ({0})", gameStats.gameResults[i].stakes[sn]);
                            break;
                    }
                    sn++;
                }
            }
        } 
        

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public IEnumerable<Player> GetActivePlayers()
        {
            var list = gameStats.gameResults.Where(p => p.player.Money > 100).ToList();
            List<Player> players = new List<Player>();

            for (int i = 0; i < list.Count; i++)
                players.Add(list[i].player);

            return players;
        }
    }
}
