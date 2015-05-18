using System;
using System.Windows.Forms;


namespace Blackjack
{
    /// <summary>
    /// The modes of AddPlayerForm
    /// </summary>
    public enum PlayerMode
    {
        /// <summary>
        /// Adding new player
        /// </summary>
        PM_ADD,
        /// <summary>
        /// Editing existing player
        /// </summary>
        PM_EDIT
    }


    /// <summary>
    /// The class of the Form for adding/editing players
    /// </summary>
    public partial class AddPlayerForm : Form
    {
        private PlayerMode windowMode;
        private Player player = null;
        
        /// <summary>
        /// Parameterized constructor
        /// </summary>
        /// <param name="pm">Window mode (PM_ADD is default)</param>
        /// <param name="p"><see cref="Player"/> object</param>
        public AddPlayerForm( PlayerMode pm = PlayerMode.PM_ADD, Player p = null)
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
        /// Getter for changed player
        /// </summary>
        /// <returns><see cref="Player"/> object</returns>
        public Player GetPlayer()
        {
            return player;
        }


        /// <summary>
        /// The method is invoked after pressing "Add" (in PM_ADD mode) or "OK" (in PM_EDIT mode)
        /// </summary>
        private void buttonAddPlayerClick(object sender, EventArgs e)
        {
            // try parsin text in the "money" field
            try
            {
                // adding
                if (windowMode == PlayerMode.PM_ADD)
                {
                    player = new Player( this.textBoxPlayerName.Text,
                                            int.Parse(this.textBoxPlayerMoney.Text));
                }
                // editing
                else
                {
                    player.Name = this.textBoxPlayerName.Text;
                    player.Money = int.Parse(this.textBoxPlayerMoney.Text);
                }
                // if everything's OK then we close the form with DialogResult Ok
                DialogResult = DialogResult.OK;
            }
            // if somethings's wrong
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
