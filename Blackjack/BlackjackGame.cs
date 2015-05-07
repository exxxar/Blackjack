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

        protected List<Player> players = new List<Player>();			// aggregates the player
        private List<bool> stands = new List<bool>();

	    protected CardHolder dealer = new CardHolder( "Dealer" );			// composite for the dealer

	    protected Deck[] decks = new Deck [DECKS_COUNT];	// Shoes, DECKS_COUNT decks

        


        public BlackjackGame()
        {
            dealer.SetScoreCounter( new BlackjackScoreCounter() );

            for (int i = 0; i < DECKS_COUNT; i++)
                decks[i] = new Deck();
        }
	
	    public void addPlayer( Player p )
        {
            p.SetScoreCounter( new BlackjackScoreCounter() );
            players.Add( p );
            stands.Add( false );
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

        // показать игровой стол:
        //		- сколько карт в каждой колоде
        //		- хэнд дилера
        //		- хэнд игрока

	    public void ShowTable()
        {
            //std::cout << std::endl;

	        //for ( int i=0; i<4; i++ )
		    //    std::cout << "[" << decks[i].getCardsNumber() << "]  ";
		
	        //std::cout << std::endl;
		
	        dealer.ShowHand();

            foreach ( Player p in players )
	            p.ShowHand();
        }


	    public void PlayResults()
        {
            SetBonuses();			// сначала проверим и начислим, если нужно, бонусы игроку
									
														// далее обычное сравнение по очкам
	        if ( players[0].CountScore() < dealer.CountScore() )
	        {
		        DealerWins();
	        }
	        else if ( players[0].CountScore() == dealer.CountScore() )
	        {
		        //std::cout << std::endl << "Tie!" << std::endl;
	        }
	        else if ( players[0].CountScore() > dealer.CountScore() )
	        {
		        PlayerWins();
	        }
        }

	    
	    public void SetBonuses()
        {
            if ( CheckBlackJack( players[0] ) )				// отдельно рассматриваем случаи с блекджеком (там другие правила начисления выигрыша)
	        {
		        if ( !CheckBlackJack( dealer ) )					    // если при блекджеке игрока нет блекджека дилера,
		        {													    // то еще проверяем, по сколько у них карт:
			        if ( dealer.PlayerHand.GetCardsNumber() > 1 )	    // если у дилера больше 1 карты, т.е. он пытался добрать карты, чтобы "перебить" блекджек игрока	
				        players[0].BonusStake( 1.5 );					// то выигрыш игрока должен быть 3 к 2 

			        else if ( dealer.PlayerHand.GetCardsNumber() == 1				    // также есть особая ситуация в самом начале (с раздачи):
						        && players[0].PlayerHand.GetCardsNumber() == 2 )		// у дилера 1 карта, у игрока - 2
				        players[0].BonusStake( 1.5 );									// в таком случае тоже выигрыш игрока равен 3 к 2

			        if ( Check777( players[0] ) == true )				// еще если три семерки, доп. выигрыш игроку (1 к 1)
			        {
				        //std::cout << "777!" << std::endl;
				        players[0].BonusStake( 2 );
			        }
		        }
	        }
	        if ( CheckBlackJack( dealer ) )				// если блекджек был у дилера, то просто выводим эту информацию
	        {
		        //std::cout << dealer->getName() << " Blackjack!" << std::endl;
	        }
        }

	    public bool CheckBlackJack( CardHolder cardHolder )
        {
            return cardHolder.CountScore() == 21;
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
	
        public void PlayerWins()
        {
            //std::cout << std::endl << player->getName() << " wins!" << std::endl;
	        //std::cout << "The winning size: " << player->getStake() << std::endl;
	        players[0].WinStake();
        }

        public void DealerWins()
        {
            //std::cout << std::endl << player->getName() << " loses!" << std::endl;
	        //std::cout << "Losing size: " << player->getStake() << std::endl;
	        players[0].LoseStake();
        }


        public void DealerActions()
        {
            char choice = '0';

	        if ( CheckBlackJack( players[0] ) )		// самое хитрое тут: сразу проверяем, вдруг у игрока блекджек на 2 картах
	        {
		        //std::cout << player->getName() << " Blackjack!" << std::endl;

		        // тут еще проверка, что и у дилера может оказаться первой карта ценой 10 или 11
		        // (но это нужно только для случая 2 карт у игрока в хэнде)

		        if ( players[0].PlayerHand.GetCardsNumber() == 2 )
		        {
			        if ( dealer.CountScore() >= 10 )
			        {
				        if ( dealer.CountScore() == 11 )		// если у дилера туз, то можно предложить игроку 2 таких варианта:
				        {
					        do
					        {
						        //std::cout << "1. Take the winning 1 to 1" << std::endl;
						        //std::cout << "2. Keep playing" << std::endl;
						        //std::cin >> choice;
					        }
					        while ( choice != '1'  && choice != '2' );

					        if ( choice == '1' )				// можно просто взять выигрыш сразу (а если нет, то омжет быть выигрыш 3 к 2 (если у дилера не будет блекджека)
					        {
						        players[0].WinStake();
						        return;							// и в этом случае сразу выходим отсюда
					        }
				        }
			        }
			        else
			        {
				        return;				// если у игрока на 2 картах блекджек, а первая карта дилера меньше 10, то он сразу проигрывает
			        }
		        }
	        }

	        while ( dealer.CountScore() < 17 )			// дилер здесь добирает карты, пока у него нет 17
	        {
		        dealer.TakeCard( decks );				// здесь возможен эксепшн! (он перехватывается в функции уровнем выше)
	        }

	        ShowTable();
        }


        public void Shuffle()
        {
            char choice = '0';
	        int stake = 0;

        	do
	        {
		        //std::cout << "Your bet?" << std::endl;
		        //std::cin >> stake;
                stake = 200;
	        }
	        while ( stake < 100 || stake > players[0].Money );

	        players[0].Stake = stake;


	        dealer.ClearHand();				// обнулим хэнды
	        players[0].ClearHand();

                                            // перемешаем случайно все колоды в шузах
            foreach (Deck d in decks)
		        d.Shuffle();
		
	        dealer.TakeCard( decks );		// дилер дает себе 1 карту
		
	        players[0].TakeCard( decks );		    // игроку даются 2 карты
	        players[0].TakeCard( decks );
				
	        ShowTable();					// показываем ситуацию на игровом столе после раздачи


	        try			// блок try здесь, потому что перебор может произойти в любой момент времени как у игрока, так у дилера
	        {
		        // главный цикл, пока игрок не нажмет "Хватит" или у него не соберется 21
		        while ( choice != '2' && players[0].CountScore() != 21 )	
		        {
			        char thirdOption = '1';					// здесь третья опция (дабл) будет появляться только тогда, когда у игрока 
			        if ( players[0].CanDoubleStake() )			// достаточно денег, чтобы удвоить ставку. Иначе - третьей опции (дабл) не будет.
				        thirdOption = '3';

			        do	
			        {
				        //std::cout << "1. Hit" << std::endl;				// предлагаем стандартные опции + дабл (возможно)
				        //std::cout << "2. Stand" << std::endl;
				        if ( players[0].CanDoubleStake() )
                            ;//std::cout << "3. Double" << std::endl;
					
				        //std::cin >> choice;
			        }
			        while ( choice != '1'  && choice != '2' && choice != thirdOption );

			        switch ( choice )
			        {
				        case '3':
					        players[0].BonusStake( 2 );		// удвоить ставку, если это возможно
                            break;

				        case '1':
					        players[0].TakeCard( decks );   // взять одну карту
					        ShowTable();					// показать ситуацию на игровом столе после взятия карты
					        break;
			        }
		        }

		        DealerActions();				// имитация действий дилера

		        PlayResults();					// сформировать результаты игры
	        }
		
	        catch (BustException bustEx)		// если у кого-то произошел перебор, мгновенно управление передается сюда
	        {
		        ShowTable();					// показываем игровой стол (чтобы видеть, у кого случился перебор)

		        //std::cout << ex.getPlayerName() << " : " << ex.what() << std::endl;		// пишем у кого перебор
				
		        if ( bustEx.PlayerName == dealer.Name )		// если перебор у дилера, то:
		        {
			        SetBonuses();					// назначаем бонусы игроку (если был перебор дилера при блекджеке игрока)
			        PlayerWins();					// и игрок выигрывает, отдаем ему его ставку + , возможно, бонусы
		        }
		        else								// если перебор у игрока
		        {
			        DealerWins();					// то забираем у него ставку
		        }
	        }

	        //players[0].ShowMoney();			// показываем что там у игрока с фишками (деньгами) в целом после этой игры
        }


        public Card PlayerHit( int nPlayer, out int nDeck )
        {
            Random r = new Random();

            nDeck = r.Next(4);
            Card card = decks[ nDeck ].PopCard();
            players[ nPlayer ].PlayerHand.AddCard( card );

            return card;
        }

        public void PlayerStand( int nPlayer )
        {
            stands[nPlayer] = true;
        }

        public void PlayerDouble( int nPlayer )
        {
            if (players[nPlayer].CanDoubleStake())
                players[nPlayer].BonusStake(2);
            else
                throw new InvalidOperationException();
        }

        public bool IsStand(int nPlayer)
        {
            return stands[nPlayer];
        }
    }
}
