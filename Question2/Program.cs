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

        public void DealHand()
        {
            if (state == Status.Playing && deck.deck.Count >= 2)
            {
                var h = Shuffler.Shuffle(deck.deck);
                var g = h.GetEnumerator();
                g.MoveNext();
                int player1Card = g.Current.Value;
                Console.WriteLine("Player 1: {0}, {1}", g.Current.Value, g.Current.Suite);
                g.MoveNext();
                int player2Card = g.Current.Value;
                Console.WriteLine("Player 1: {0}, {1}", g.Current.Value, g.Current.Suite);
                if (player1Card > player2Card)
                {
                    winner = 1;
                    state = Status.Finished;
                }
                else if (player1Card < player2Card)
                {
                    winner = 2;
                    state = Status.Finished;
                }

                if (state == Status.Finished)
                {
                    Console.WriteLine("Player {0} won the game", winner);
                }
                Console.WriteLine("Press a key to continue...\n");
                Console.ReadKey();
                deck.deck.RemoveAll(x => (x.Suite == g.Current.Suite) && (x.Value == g.Current.Value));
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
        internal Card(int Value, Suites Suite)
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

        internal Card wildCard(int cardsPerSuite)
        {
            Card wildCard = new(Randomizer.Generator(1, cardsPerSuite + 1), (Card.Suites)Randomizer.Generator(0, 4));
            return wildCard;
        }

        public List<Card> deck { get; private set; }


    }

    class Randomizer
    {
        internal static int Generator(int minValue, int maxValue)
        {
            int rand = RandomNumberGenerator.GetInt32(minValue, maxValue);
            return rand;
        }
    }

    class Shuffler
    {
        public static IEnumerable<T> Shuffle<T>(IEnumerable<T> source)
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