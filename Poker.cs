using System;
using System.Collections.Generic;
using System.Linq;

namespace PokerHands
{
    public class Player
    {
        public List<Card> cards
        {
            get;
            set;
        }
        public Card winningCard
        {
            get;
            set;
        }

        public Player()
        {
            this.cards = new List<Card>();
            this.winningCard = null;
        }

        public void TakeCard(Card card)
        {
            this.cards.Add(card);
        }

        public bool Ready()
        {
            return this.cards.Count == 5;
        }

        public bool HasFlush()
        {
            for (int i = 0; i < this.cards.Count - 1; i++)
            {
                Card card = this.cards[i];
                Card nextCard = this.cards[i + 1];
                if (card.suit != nextCard.suit) return false;
            }
            List<Card> sortedCards = SortCards();
            winningCard = (sortedCards[sortedCards.Count - 1]);
            return true;
        }

        public bool HasThreeOfAKind()
        {
            for (int i = 0; i < this.cards.Count; i++)
            {
                int value = this.cards[i].value;
                if (this.cards.FindAll(card => card.value.Equals(value)).Count == 3)
                {
                    winningCard = this.cards[i];
                    return true;
                }
            }
            return false;
        }

        public bool HasAPair()
        {
            List<Card> matches = new List<Card>();
            for (int i = 0; i < this.cards.Count; i++)
            {
                int value = this.cards[i].value;
                if (this.cards.FindAll(card => card.value.Equals(value)).Count == 2) matches.Add(this.cards[i]);
            }
            if (matches.Count == 2)
            {
                winningCard = matches[0];
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool HasTwoPair()
        {
            List<Card> matches = new List<Card>();
            for (int i = 0; i < this.cards.Count; i++)
            {
                int value = this.cards[i].value;
                if (this.cards.FindAll(card => card.value.Equals(value)).Count == 2) matches.Add(this.cards[i]);
            }
            if (matches.Count == 4)
            {
                winningCard = matches.OrderBy(card => card.value).ToList()[0];
                return true;
            }
            else
            {
                return false;
            }
        }

        public string Hand()
        {
            if (HasFlush()) return "flush";
            if (HasThreeOfAKind()) return "three of a kind";
            if (HasTwoPair()) return "two pair";
            if (HasAPair()) return "pair";
            List<Card> sortedCards = SortCards();
            winningCard = sortedCards[sortedCards.Count - 1];
            return "high card";
        }

        public List<Card> SortCards()
        {
            return this.cards.OrderBy(card => card.value).ToList();
        }
    }
}
namespace PokerHands
{
    public class Card
    {
        public string suit
        {
            get;
            set;
        }
        public int value
        {
            get;
            set;
        }

        public Card(string suit, int value)
        {
            this.suit = suit;
            this.value = value;
        }
    }
}

namespace PokerHands
{
    public class Game
    {
        public static void Main()
        {
            string player1Name = "White";
            Player player1 = new Player();
            string player2Name = "Black";
            Player player2 = new Player();
            Game poker = new Game();
            poker.AddPlayer(player1);
            poker.AddPlayer(player2);

            for (int i = 1; i < 6; i++)
            {
                Console.WriteLine("Enter the card suit for " + player1Name + " choose from 's', 'c', 'd' or 'h'");
                string suit = Console.ReadLine();
                Console.WriteLine("Enter the card value for " + player1Name + " choose from numbers 2 to 14");
                int value = Convert.ToInt16(Console.ReadLine());
                Card player1Card = new Card(suit, value);
                player1.TakeCard(player1Card);
            }

            for (int i = 1; i < 6; i++)
            {
                Console.WriteLine("Enter the card suit for " + player2Name + " choose from 's', 'c', 'd' or 'h'");
                string suit = Console.ReadLine();
                Console.WriteLine("Enter the card value for " + player2Name + " choose from numbers 2 to 14");
                int value = Convert.ToInt16(Console.ReadLine());
                Card player2Card = new Card(suit, value);
                player2.TakeCard(player2Card);
            }

            Player winner = poker.Winner();
            if (winner == player1)
            {
                string winnerName = player1Name;
                Console.WriteLine("The winner is " + winnerName);
                Console.WriteLine(player1.Hand());

            }
            else
            {
                string winnerName = player2Name;
                Console.WriteLine("The winner is " + winnerName);
                Console.WriteLine(player2.Hand());
            }

        }


        public List<Player> players
        {
            get;
            set;
        }
        public Dictionary<string, int> handRankings
        {
            get;
            set;
        }

        public Game()
        {
            this.players = new List<Player>();
            this.handRankings = new Dictionary<string, int>();
            handRankings.Add("flush", 1);
            handRankings.Add("three of a kind", 2);
            handRankings.Add("two pair", 3);
            handRankings.Add("pair", 4);
            handRankings.Add("high card", 5);
        }

        public void AddPlayer(Player player)
        {
            this.players.Add(player);
        }

        public Player Winner()
        {
            Player winner = this.players[0];
            for (int i = 0; i < this.players.Count; i++)
            {
                Player currentPlayer = this.players[i];
                string playersHand = this.players[i].Hand();
                if (this.handRankings[playersHand] < this.handRankings[winner.Hand()])
                {
                    winner = currentPlayer;
                }
                else if (this.handRankings[playersHand] == this.handRankings[winner.Hand()])
                {
                    winner = _EqualHandRankings(currentPlayer, winner);
                }
            }
            return winner;
        }

        private Player _EqualHandRankings(Player player1, Player player2)
        {
            return player1.winningCard.value > player2.winningCard.value ? player1 : player2;
        }
    }
}
