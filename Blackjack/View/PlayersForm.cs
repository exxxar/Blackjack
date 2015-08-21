using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;


namespace Blackjack
{
    /// <summary>
    /// Form with stats of active players
    /// </summary>
    public partial class PlayersForm : Form
    {
        /// <summary>
        /// Property that indicates if some player's info was changed after loading of the form
        /// </summary>
        public bool ChangedPlayer { get; set; }

        /// <summary>
        /// Game stats property
        /// </summary>
        public PlayerStats GameStats { get; set; }


        /// <summary>
        /// Default constructor
        /// </summary>
        public PlayersForm()
        {
            InitializeComponent();
            GameStats = new PlayerStats();
            ChangedPlayer = false;
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
            }
        }


        private void buttonRemove_Click(object sender, EventArgs e)
        {
            if (listViewPlayers.SelectedIndices.Count == 0)
                return;

            int selIdx = listViewPlayers.SelectedIndices[0];
            listViewPlayers.Items.RemoveAt( selIdx );
            GameStats.gameResults.RemoveAt( selIdx );

            ChangedPlayer = true;
        }


        private void buttonAdd_Click(object sender, EventArgs e)
        {
            // check if all MAX_PLAYERS players are in the game already
            if (GetActivePlayers().Count() == BlackjackGame.MAX_PLAYERS)
            {
                MessageBox.Show( string.Format(
                                    "You can't add a new player to the game!\nAll {0} players are in the game!",
                                    BlackjackGame.MAX_PLAYERS ) );
                return;
            }

            // try adding new player
            AddPlayerForm form = new AddPlayerForm();
            if (form.ShowDialog() == DialogResult.OK)
            {
                BlackjackResult res = new BlackjackResult();
                res.player = form.GetPlayer();
                res.results = new List<PlayerResult>();
                res.shuffles = new List<int>();
                res.stakes = new List<int>();

                GameStats.gameResults.Add( res );

                // find the current shuffle (current column in the stats table)
                int nColumns = TotalColumns();

                ListViewItem item = new ListViewItem(string.Format("{0} ({1} $)", form.GetPlayer().Name, form.GetPlayer().Money) );
                listViewPlayers.Items.Add(item);

                for (int i = 0; i < nColumns; i++)
                {
                    item.SubItems.Add("");
                }

                ShowStatistics();

                ChangedPlayer = true;
            }
        }


        private void buttonEdit_Click(object sender, EventArgs e)
        {
            if (listViewPlayers.SelectedIndices.Count > 0)
            {
                int nPlayer = listViewPlayers.SelectedIndices[0];
                AddPlayerForm form = new AddPlayerForm( PlayerMode.PM_EDIT,
                                                            GameStats.gameResults[nPlayer].player );
                form.ShowDialog();

                string s = string.Format("{0} ({1} $)", form.GetPlayer().Name, form.GetPlayer().Money );
                listViewPlayers.Items[nPlayer].Text = s;

                ChangedPlayer = true;
            }
        }


        /// <summary>
        /// Find the last shuffle number in stats (this will be the last column)
        /// </summary>
        /// <returns></returns>
        private int TotalColumns()
        {
            int nColumns = 0;

            try
            {
                // not shuffles.Count! rather last shuffleNo! and additional check for shuffles.Count != 0
                nColumns = GameStats.gameResults.Max( t =>
                                                        {
                                                            if (t.shuffles.Count > 0) return t.shuffles.Last();
                                                            else return -1;
                                                        });
                nColumns++;
            }
            catch (InvalidOperationException)
            {
                nColumns = 0;
            }

            return nColumns;
        }


        /// <summary>
        /// Main function here: show full stats table
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

            for (int j = 0; j < GameStats.gameResults.Count; j++)
            {
                ListViewItem item = new ListViewItem(string.Format("{0} ({1} $)",
                                                        GameStats.gameResults[j].player.Name,
                                                        GameStats.gameResults[j].player.Money));

                for (int i = 0; i < nColumns; i++)
                    item.SubItems.Add("");

                listViewPlayers.Items.Add(item);
            }
            

            for (int i = 0; i < GameStats.gameResults.Count; i++)
            {
                ListViewItem item = listViewPlayers.Items[i];

                int sn = 0;

                foreach (var res in GameStats.gameResults[i].results)
                {
                    int shuffleNo = GameStats.gameResults[i].shuffles[sn];

                    switch (res)
                    {
                        case PlayerResult.WIN:
                            item.SubItems[shuffleNo+1].Text = "+" + GameStats.gameResults[i].stakes[sn];
                            break;
                        case PlayerResult.LOSE:
                            item.SubItems[shuffleNo+1].Text = "-" + GameStats.gameResults[i].stakes[sn];
                            break;
                        case PlayerResult.STAY:
                            item.SubItems[shuffleNo+1].Text = string.Format("stay ({0})", GameStats.gameResults[i].stakes[sn]);
                            break;
                    }
                    sn++;
                }
            }
        } 
        

        /// <summary>
        /// Method returns all players who've got enough money to make minimal bet
        /// </summary>
        /// <returns>Collection of active players</returns>
        public IEnumerable<Player> GetActivePlayers()
        {
            var list = GameStats.gameResults.Where(p => p.player.Money > BlackjackGame.MIN_STAKE).ToList();
            List<Player> players = new List<Player>();

            for (int i = 0; i < list.Count; i++)
                players.Add(list[i].player);

            return players;
        }
    }
}
