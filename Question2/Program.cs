using System.Security.Cryptography;


namespace Question2
{
    class HighCard
    {
        internal HighCard(int cardsPerSuite, int numberOfPlayers)
        {

            Deck deck = new(cardsPerSuite);
            Card wildCard = deck.wildCard(cardsPerSuite);

        }

        int DealCard()
        {
            return 0;
        }
    }
    class Card
    {
        internal Card(int Value, Suites Suite)
        {
            this.Value = Value;
            this.Suite = Suite;
        }
        internal enum Suites
        {
            Hearts,
            Diamonds,
            Clubs,
            Spades
        }
        public int Value { get; private set; }
        public Suites Suite { get; private set; }

    }

    class Deck
    {
        internal Deck(int cardsPerSuite)
        {
            List<Card> deck = new List<Card>();
            for (int i = 0; i < cardsPerSuite * 4; i++)
            {
                Card.Suites suite = (Card.Suites)(Math.Floor((decimal)i / cardsPerSuite));
                int value = (i % cardsPerSuite) + 1;
                deck.Add(new Card(value, suite));
            }
        }

        internal Card wildCard(int cardsPerSuite)
        {
            Card wildCard = new Card(Randomizer.Generator(1, cardsPerSuite + 1), (Card.Suites)Randomizer.Generator(0, 4));
            return wildCard;
        }
        
        

    }

    class Randomizer {
        internal static int Generator(int minValue, int maxValue)
        {
            int rand = RandomNumberGenerator.GetInt32(minValue, maxValue);
            return rand;
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            const int MINPLAYERS = 2;
            const int MAXPLAYERS = 10;
            const int MINCARDSPERSUITE = 2;
            const int MAXCARDSPERSUITE = 20;
            int numberOfPlayers = 2;
            int cardsPerSuite = 13;
            numberOfPlayers = Helper("Players: ", numberOfPlayers, MINPLAYERS, MAXPLAYERS);
            cardsPerSuite = Helper("Cards per suite", cardsPerSuite, MINCARDSPERSUITE, MAXCARDSPERSUITE);

            HighCard game1 = new HighCard(cardsPerSuite, numberOfPlayers);
        }
        static int Helper(string text, int value, int MINVALUE, int MAXVALUE) {
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