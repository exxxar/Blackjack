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
    public class BlackjackResult
    {
        public int shuffleNo = 1;
        public List<string> playerNames = new List<string>();
        public List<PlayerResult> results = new List<PlayerResult>();
        public List<int> stake = new List<int>();
    }

    public partial class PlayersForm : Form
    {
        private List<BlackjackResult> results = new List<BlackjackResult>();

        private List<Player> players = new List<Player>();


        public PlayersForm()
        {
            InitializeComponent();

            players.Add( new Player( "Tim", 10000) );
            players.Add( new Player( "Roma", 12000) );
            players.Add( new Player( "Sanya", 20000) );

            BlackjackResult res = new BlackjackResult();
            res.shuffleNo = 1;
            res.playerNames.Add("Tim");
            res.results.Add( PlayerResult.WIN );
            res.stake.Add( 200 );
            results.Add( res );

            res = new BlackjackResult();
            res.shuffleNo = 2;
            res.playerNames.Add("Roma");
            res.results.Add(PlayerResult.WIN);
            res.stake.Add(500);
            results.Add(res);

            res = new BlackjackResult();
            res.shuffleNo = 3;
            res.playerNames.Add("Sanya");
            res.results.Add(PlayerResult.TIE);
            res.stake.Add( 400 );
            results.Add(res);
        }


        private void buttonOK_Click(object sender, EventArgs e)
        {
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
            players.Add( new Player("New", 12000) );
        }


        private void PlayersForm_Load(object sender, EventArgs e)
        {
            // get all players from stats!

            listViewPlayers.Columns.Add( "Name (Money)", 140 );

            for (int i = 0; i < results.Count; i++ )
                listViewPlayers.Columns.Add( "Shuffle" + results[i].shuffleNo, 90 );

            for (int i = 0; i < players.Count; i++ )
            {
                ListViewItem item = new ListViewItem(string.Format("{0} ({1} $)", players[i].Name, players[i].Money));

                for (int j = 0; j < results.Count; j++)
                    item.SubItems.Add("");

                this.listViewPlayers.Items.Add(item);
            }


            for (int i = 0; i < players.Count; i++)
            {
                for (int j = 0; j < results.Count; j++)
                {
                    int idx = results[j].playerNames.IndexOf(players[i].Name);
                    if (idx < 0) break;

                    for (int k=0; k<results.Count; k++ )
                    {
                        string sign = (results[j].results[k] == PlayerResult.LOSE) ? "-" : "+";
                        this.listViewPlayers.Items[idx].SubItems[j].Text = sign + results[j].stake[0]; // ???
                    }
                }
            }

            /*
            for (int i = 0; i < players.Count; i++)
            {
                for (int j = 0; j < results.Count; j++)
                {
                    int idx = results[j].playerNames.IndexOf(players[i].Name);
                    if (idx < 0)
                        return;

                    string sign = (results[j].results[k] == PlayerResult.LOSE) ? "-" : "+";
                    listViewPlayers.Items[i].SubItems[results[j].shuffleNo - 1].Text = sign + results[j].stake[k];

                    if (results[j].results[k] == PlayerResult.TIE)
                    {
                        listViewPlayers.Items[i].SubItems[results[j].shuffleNo].BackColor = Color.White;
                    }
                }
            }*/
        }
    }
}
