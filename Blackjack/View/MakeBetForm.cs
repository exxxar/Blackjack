using System;
using System.Windows.Forms;


namespace Blackjack
{
    /// <summary>
    /// Form for making a bet
    /// </summary>
    public partial class MakeBetForm : Form
    {
        private Player player;

        /// <summary>
        /// Constructor takes the player object (player who makes the bet)
        /// </summary>
        /// <param name="p"></param>
        public MakeBetForm( Player p )
        {
            InitializeComponent();

            player = p;
            this.labelPlayerName.Text = p.Name;
        }
        

        private void buttonOK_Click(object sender, EventArgs e)
        {
            try
            {
                player.Stake = int.Parse(this.textBoxPlayerStake.Text);

                // check if the bet is big enough
                if (player.Stake >= BlackjackGame.MIN_STAKE)
                {
                    DialogResult = DialogResult.OK;
                }
                else
                {
                    MessageBox.Show(string.Format("Your bet must be at least {0} $", BlackjackGame.MIN_STAKE));    
                }
            }
            // check if money format was inappropriate
            catch (FormatException)
            {
                MessageBox.Show("Wrong number format (the money field must contain only digits)");
            }
            
        }
    }
}
