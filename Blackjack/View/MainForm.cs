using System;
using System.Drawing;
using System.Windows.Forms;


namespace Blackjack
{
    /// <summary>
    /// Main form class
    /// </summary>
    public partial class MainForm : Form
    {
        // Our design implies that we have such objects:
        
        // The game
        private BlackjackGame game = null;

        // The game controller (responds to user actions, communicates with visualizer)
        private CardTableController gamecontroller = null;
        
        // The visualizer of a card table
        private CardTableVisualizer gamevisualizer = null;
        
        // Stats
        private PlayerStats statistics = new PlayerStats();
        
        private int curShuffle = 0;
        

        /// <summary>
        /// Default constructor
        /// </summary>
        public MainForm()
        {
            InitializeComponent();
        }


        /// <summary>
        /// Method is called when the game's over and adds its result to the stats table
        /// </summary>
        public void FixGameResults()
        {
            // ... after the game is over
            for (int i = 0; i < game.GetPlayersCount(); i++)
                statistics.AddShuffleResult( game.GetPlayer(i), curShuffle );

            curShuffle++;
        }


        private void MainForm_Load(object sender, EventArgs e)
        {
            // firstly, show the Players form
            PlayersForm form = new PlayersForm();

            // if we added at least one player and pressed OK, then start:
            if (form.ShowDialog() == DialogResult.OK)
            {
                statistics = form.GameStats;

                // create new game object
                game = new BlackjackGame();

                // initialize visualizer
                gamevisualizer = new CardTableVisualizer(game);
                gamevisualizer.PrepareGraphics(this.Width, this.Height, CreateGraphics());

                // initialize controller:
                // pass the game and visualizer objects
                // and the FixGameResults() function as the function that will be called when the game's over
                gamecontroller = new CardTableController(game, gamevisualizer, FixGameResults);

                // set the list of active players for a new game
                game.SetPlayerList( form.GetActivePlayers() );

                // start new game
                NewGame();
            }
            else
            {
                Close();
            }
        }



        /// <summary>
        /// Method shows PlayersForm for editing players
        /// </summary>
        private void ChangePlayers()
        {
            // if the current game is not finished yet
            if ( !game.CheckGameFinished() )
            {
                MessageBox.Show("Please wait until the end of current game!");
                return;
            }

            PlayersForm form = new PlayersForm();
            form.GameStats = statistics;
            form.ShowDialog();

            statistics = form.GameStats;

            if (form.ChangedPlayer)
            {
                // set the list of active players for a new game
                game.SetPlayerList( form.GetActivePlayers() );

                // start new game
                NewGame();
            }
        }


        /// <summary>
        /// Method starts new game (new shuffle).
        /// Method checks if particular player has enough money to proceed and proposes to make bets
        /// </summary>
        private void NewGame()
        {
            // exclude all those players who have not enough money to cannot pay
            game.RemovePlayersMinStake();

            // check if there's at least one player who can do minimal bet
            if (game.GetPlayersCount() == 0)
            {
                MessageBox.Show( "There are no more players or all players have not enough money to play!" );
                gamevisualizer.Invalidate();
                return;
            }

            // propose to do the bet for each player
            for (int i = 0; i < game.GetPlayersCount(); i++)
            {
                MakeBetForm makeBetForm = new MakeBetForm(game.GetPlayer(i));
                makeBetForm.ShowDialog();
            }

            // Redraw the form
            gamevisualizer.Invalidate();
            
            // ...And start new game
            gamecontroller.StartNewShuffle();
        }
       

        private void MainForm_MouseClick(object sender, MouseEventArgs e)
        {
            // if the user clicked "Edit Players" icon
            if (gamevisualizer.PlayersHighlight)
            {
                ChangePlayers();
            }
            // if the user clicked "F2 - new game"
            else if (gamevisualizer.NewGameHighlight)
            {
                if (!game.CheckStates())
                {
                    MessageBox.Show("Please wait until the end of current game!");
                    return;
                }

                NewGame();
            }
            // otherwise user perhaps clicked on some action button
            else if ( !game.CheckGameFinished() )
            {
                try
                {
                    gamecontroller.UserActions(e.Location);
                }
                catch (InvalidOperationException)
                {
                    MessageBox.Show("You've not enough money to double down!");
                }
            }
        }


        private void MainForm_KeyDown(object sender, KeyEventArgs e)
        {
            // New game can be initiated by user pressing F2
            if (e.KeyCode == Keys.F2)
            {
                // if the current game is not finished yet
                if ( !game.CheckGameFinished() )
                {
                    MessageBox.Show("Please wait until the end of current game!");
                    return;
                }

                // otherwise
                NewGame();
            }
        }


        private void MainForm_MouseMove(object sender, MouseEventArgs e)
        {
            Graphics DC = CreateGraphics();
            Rectangle playerRect = new Rectangle(735, 5, 30, 40);
            Rectangle newGameRect = new Rectangle(60, 10, 180, 20);

            gamevisualizer.PlayersHighlight = (playerRect.Contains( e.Location) ) ? true : false;
            gamevisualizer.NewGameHighlight = (newGameRect.Contains(e.Location)) ? true : false;

            gamevisualizer.Invalidate();
        }


        private void MainForm_Paint(object sender, PaintEventArgs e)
        {
            e.Graphics.DrawImage(gamevisualizer.GetShowTable(), 0, 0);
        }


        private void MainForm_Resize(object sender, EventArgs e)
        {
            // if the form was resized...
            Graphics DC = CreateGraphics();
            // create new double buffered bitmap
            gamevisualizer.PrepareGraphics(this.Width, this.Height, DC);
            // and plot it
            DC.DrawImage( gamevisualizer.GetShowTable(), 0, 0 );
        }
    }
}
