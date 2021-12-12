﻿// compile with: -doc:DocFileName.xml 
// I know getters and setters are ugly, and are not
// appropiate and that I should be doing
// better encapsulation.
// If I had more time I'll refactor everything...

using System.Security.Cryptography;
using System.Collections;

namespace Question2
{
    public class HighCard
    {
        public enum Status
        {
            Playing,
            Finished
        }

        internal Status state;
        internal int winner;

        public HighCard(int cardsPerSuite, int gameNumber)
        {
            deck = new(cardsPerSuite);
            wildCard = deck.wildCard(cardsPerSuite);
            state = Status.Playing;
            this.gameNumber = gameNumber;
        }

        /// <summary>This method is an implentation of the
        /// Fisher-Yates shuffle algorithm
        /// </summary>
        public IEnumerable<T> Shuffle<T>(IEnumerable<T> source)
        {
            var rng = Randomizer.Generator(0, source.Count() + 1);
            T[] elements = source.ToArray();
            for (int i = elements.Length - 1; i >= 0; i--)
            {
                int swapIndex = Randomizer.Generator(0, source.Count());
                yield return elements[swapIndex];
                elements[swapIndex] = elements[i];
            }
        }

        /// <summary>This method advances a hand and
        ///    determines if the game was finished
        /// </summary>
        public void DealHand()
        {
            if (state == Status.Playing && deck.deck.Count >= 2)
            {
                var sd = Shuffle(deck.deck).GetEnumerator();
                Card[] pc = new Card[2];

                for (int i = 0; i < 2; i++)
                {
                    sd.MoveNext();
                    pc[i] = new(sd.Current.Value, sd.Current.Suite);
                    Console.WriteLine("Player {0}: {1} of {2}", i + 1, sd.Current.Value, sd.Current.Suite);
                    deck.deck.RemoveAll(x => (x.Suite == sd.Current.Suite) && (x.Value == sd.Current.Value));
                }

                winner = (pc[0].Value == wildCard.Value && pc[0].Suite == wildCard.Suite ? 1 : (pc[1].Value == wildCard.Value && pc[1].Suite == wildCard.Suite) ? 2 : 0);
                if (winner != 0)
                {
                    Console.WriteLine("Player {0} got the wildcard: {1} of {2}", winner, pc[winner - 1].Value, pc[winner - 1].Suite);
                }


                if (pc[0].Value > pc[1].Value || winner == 1)
                {
                    winner = 1;
                    state = Status.Finished;
                }
                else if (pc[0].Value < pc[1].Value || winner == 2)
                {
                    winner = 2;
                    state = Status.Finished;
                }

                if (state == Status.Finished && winner != 0)
                {
                    Console.WriteLine("Player {0} won", winner);
                }

                Console.WriteLine("Press a key to continue...\n");
                Console.ReadKey();
            }
            else if (state == Status.Playing && deck.deck.Count < 2)
            {
                Console.WriteLine("Game tied");
                state = Status.Finished;
                Console.WriteLine("Press a key to continue...\n");
                Console.ReadKey();
            }
        }


        public Deck deck { get; private set; }
        public Card wildCard { get; private set; }
        public int gameNumber { get; private set; }
    }
    public class Card
    {
        public Card(int Value, Suites Suite)
        {
            this.Value = Value;
            this.Suite = Suite;
        }
        public enum Suites
        {
            Hearts,
            Diamonds,
            Clubs,
            Spades
        }
        public int Value { get; private set; }
        public Suites Suite { get; private set; }

    }

    public class Deck
    {
        public Deck(int cardsPerSuite)
        {
            List<Card> deck = new();
            for (int i = 0; i < cardsPerSuite * 4; i++)
            {
                Card.Suites suite = (Card.Suites)(Math.Floor((decimal)i / cardsPerSuite));
                int value = (i % cardsPerSuite) + 1;
                deck.Add(new Card(value, suite));
            }
            this.deck = deck;
        }

        /// <summary>This method generates a random wildcard
        /// within the values provided by the user input (cardsPerSuite)
        /// </summary>
        internal Card wildCard(int cardsPerSuite)
        {
            Card wildCard = new(Randomizer.Generator(1, cardsPerSuite + 1), (Card.Suites)Randomizer.Generator(0, 4));
            return wildCard;
        }

        public List<Card> deck { get; private set; }


    }

    class Randomizer
    {
        /// <summary>This method is the randomizer used for
        /// wildcards and for shuffling the decks
        /// </summary>
        internal static int Generator(int minValue, int maxValue)
        {
            int rand = RandomNumberGenerator.GetInt32(minValue, maxValue);
            return rand;
        }
    }

    class Shuffler
    {

    }

    class Program
    {
        static void Main(string[] args)
        {
            const int MINCARDSPERSUITE = 2;
            const int MAXCARDSPERSUITE = 20;
            int cardsPerSuite = helper("Cards per suite", MINCARDSPERSUITE, MAXCARDSPERSUITE);
            const int MINDECKS = 1;
            const int MAXDECKS = 5;
            int numberOfDecks = helper("Decks to play: ", MINDECKS, MAXDECKS);

            HighCard[] games = new HighCard[numberOfDecks];

            for (int i = 1; i <= numberOfDecks; i++)
            {
                games[i - 1] = new HighCard(cardsPerSuite, i);
            }

            do
            {
                foreach (HighCard g in games)
                {
                    if (g.state != HighCard.Status.Finished)
                    {
                        Console.WriteLine("Game number {0}", g.gameNumber);
                        g.DealHand();
                    }
                }
                Console.Clear();
            } while (!games.All(game => game.state == HighCard.Status.Finished));
        }

        /// <summary>This method is a helper used currently
        ///  for selecting decks and cards per suite according
        ///  to the user input range selected
        /// </summary>
        static int helper(string text, int MINVALUE, int MAXVALUE)
        {
            int value;
            do
            {
                while (Console.KeyAvailable) Console.ReadKey(false);
                Console.WriteLine(text);
                var input = Console.ReadLine();
                if (int.TryParse(input, out value))
                {
                    if (value < MINVALUE || value > MAXVALUE)
                    {
                        Console.WriteLine("Type a number between {0} and {1}", MINVALUE, MAXVALUE);
                    }
                }
            } while (value < MINVALUE || value > MAXVALUE);
            return value;
        }
    }
}