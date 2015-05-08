using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Blackjack
{
    

    public class BlackjackGame: ICardGame
    {
        // The number of decks is 4
        public const int DECKS_COUNT = 4;
        protected Deck[] decks = new Deck[DECKS_COUNT];	            // Shoes, DECKS_COUNT decks

        protected CardHolder dealer = new CardHolder("Dealer");		// composite for the dealer
        protected List<Player> players = new List<Player>();		// aggregates the player
        
        private List<PlayerState> playerStates = new List<PlayerState>();

        IScoreCounter scoreCounter = new BlackjackScoreCounter();

        public bool dealerBlackjack = false;



        public BlackjackGame()
        {
            dealer.SetScoreCounter( scoreCounter );

            for (int i = 0; i < DECKS_COUNT; i++)
                decks[i] = new Deck();
        }
	
	    public void addPlayer( Player p )
        {
            p.SetScoreCounter( scoreCounter );
            players.Add( p );
            playerStates.Add( PlayerState.HIT );
        }

        public Player GetPlayer(int pos)
        {
            return players[pos];
        }

        public CardHolder GetDealer()
        {
            return dealer;
        }

        public Deck GetDeck(int pos)
        {
            return decks[pos];
        }

        public int GetPlayersCount()
        {
            return players.Count;
        }


	    public int PlayResults( int nPlayer )
        {
            SetBonuses( nPlayer );			// сначала проверим и начислим, если нужно, бонусы игроку
									        // далее обычное сравнение по очкам
            // DEALER WINS
	        if ( players[ nPlayer ].CountScore() < dealer.CountScore() )
	        {
                players[ nPlayer ].LoseStake();
                return -1;
	        }
            // PLAYER WINS
	        else if ( players[ nPlayer ].CountScore() > dealer.CountScore() )
	        {
                players[ nPlayer ].WinStake();
                return 1;
	        }
            // TIE
            else
            {
                return 0;
            }
        }

	    
	    public void SetBonuses( int nPlayer )
        {
            if ( CheckBlackJack( players[ nPlayer ] ) )				// отдельно рассматриваем случаи с блекджеком (там другие правила начисления выигрыша)
	        {
		        if ( !CheckBlackJack( dealer ) )					    // если при блекджеке игрока нет блекджека дилера,
		        {													    // то еще проверяем, по сколько у них карт:
			        if ( dealer.PlayerHand.GetCardsNumber() > 1 )	    // если у дилера больше 1 карты, т.е. он пытался добрать карты, чтобы "перебить" блекджек игрока	
                        players[nPlayer].BonusStake(1.5);					// то выигрыш игрока должен быть 3 к 2 

			        else if ( dealer.PlayerHand.GetCardsNumber() == 1				    // также есть особая ситуация в самом начале (с раздачи):
                                && players[nPlayer].PlayerHand.GetCardsNumber() == 2)		// у дилера 1 карта, у игрока - 2
                        players[ nPlayer ].BonusStake(1.5);									// в таком случае тоже выигрыш игрока равен 3 к 2

                    if (Check777(players[ nPlayer ]) == true)				// еще если три семерки, доп. выигрыш игроку (1 к 1)
			        {
                        players[ nPlayer ].BonusStake(2);
			        }
		        }
	        }
	        if ( CheckBlackJack( dealer ) )				// если блекджек был у дилера, то просто выводим эту информацию
	        {
            //    throw new BlackjackException();
                dealerBlackjack = true;
	        }
        }

	    public bool CheckBlackJack( CardHolder cardHolder )
        {
            return (cardHolder.CountScore() == 21);
        }
	
        public bool Check777( CardHolder cardHolder )
        {
            if (cardHolder.CountScore() == 21)
            {
                Hand hand = cardHolder.PlayerHand;

                if (hand.GetCardsNumber() == 3)
                    if (hand[0].Rank == 7 && hand[1].Rank == 7 && hand[2].Rank == 7)
                        return true;
            }

            return false;
        }



        public Card ChooseCard( out int nDeck )
        {
            Random r = new Random();
            nDeck = r.Next(4);
            return decks[nDeck].PopCard();
        }


        public void Shuffle()
        {
            dealer.ClearHand();				// обнулим хэнды

            for (int i=0; i<players.Count; i++ )
            {
                players[i].ClearHand();
                SetPlayerState( i, PlayerState.HIT );
            }

                                            // перемешаем случайно все колоды в шузах
            foreach (Deck d in decks)
		        d.Shuffle();
        }


        public void PlayerHit( int nPlayer, Card card )
        {
            players[ nPlayer ].PlayerHand.AddCard( card );

            if (players[nPlayer].CountScore() > 21)
                throw new BustException( players[nPlayer].Name, players[nPlayer].CountScore() );
        }

        

        public void PlayerDouble( int nPlayer )
        {
            if (players[nPlayer].CanDoubleStake())
                players[nPlayer].BonusStake(2);
            else
                throw new InvalidOperationException();
        }


        public bool CheckAllBusted()
        {
            for (int i = 0; i < players.Count; i++)
                if ( GetPlayerState(i) != PlayerState.BUST )
                    return false;

            return true;
        }


        public bool CheckStates()
        {
            for (int i = 0; i < players.Count; i++)
                if (GetPlayerState(i) != PlayerState.BUST && GetPlayerState(i) != PlayerState.STAND)
                    return false;

            return true;
        }


        public void SetPlayerState( int nPlayer, PlayerState state )
        {
            playerStates[nPlayer] = state;
        }


        public PlayerState GetPlayerState(int nPlayer)
        {
            return playerStates[ nPlayer ];
        }


        public void DealerHit( int nPlayer )
        {
            //for (int i=0; i<players.Count; i++)
            if (CheckBlackJack(players[nPlayer]))		// самое хитрое тут: сразу проверяем, вдруг у игрока блекджек на 2 картах
            {
                // тут еще проверка, что и у дилера может оказаться первой карта ценой 10 или 11
                // (но это нужно только для случая 2 карт у игрока в хэнде)

                if (players[nPlayer].PlayerHand.GetCardsNumber() == 2)
                {
                    if (dealer.CountScore() >= 10)
                    {
                        if (dealer.CountScore() == 11)		// если у дилера туз, то можно предложить игроку 2 таких варианта:
                        {
                            System.Windows.Forms.DialogResult res = 
                                        System.Windows.Forms.MessageBox.Show("1", players[nPlayer].Name + " what?", System.Windows.Forms.MessageBoxButtons.YesNo);
                                    
                            if (res == System.Windows.Forms.DialogResult.Yes)		// можно просто взять выигрыш сразу (а если нет, то омжет быть выигрыш 3 к 2 (если у дилера не будет блекджека)
                            {
                                players[ nPlayer ].WinStake();
                                return;							                    // если берем, то в этом случае сразу выходим отсюда
                            }
                        }
                    }
                    else
                    {
                        return;				// если у игрока на 2 картах блекджек, а первая карта дилера меньше 10, то он сразу проигрывает
                    }
                }
            }
        }
    }
}
