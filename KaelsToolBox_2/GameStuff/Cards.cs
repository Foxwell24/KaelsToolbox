using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KaelsToolBox_2.GameStuff;

public class Cards
{
    public Cards(int numberOfDecks = 1)
    {
        NumberOfDecks = numberOfDecks;
    }

    private int numberOfDecks;
		public int NumberOfDecks
    {
        get => numberOfDecks;
        set
        {
            numberOfDecks = value;

            availableCards.Clear();
            drawnCards.Clear();
            for (int i = 0; i < value; i++)
            {
                availableCards.AddRange(cards);
            }
        }
    }

    private List<Card> drawnCards = new();
    public List<Card> DrawnCards => [.. drawnCards];

    private List<Card> availableCards = new();
    public List<Card> AvailableCards => [.. availableCards];

    public Card[] Draw(int numberOfCards = 1)
    {
        List<Card> draw = new();

        for (int i = 0; i < numberOfCards; i++)
        {
            var card = availableCards[Random.Shared.Next(availableCards.Count)];
            drawnCards.Add(card);
            availableCards.Remove(card);
            draw.Add(card);
        }

        return [.. draw];
    }
    public void Shuffle()
    {
        availableCards.AddRange(drawnCards);
        drawnCards.Clear();
    }
    public static Card GetRandomCard()
    {
        return cards[Random.Shared.Next(cards.Length)];
    }

    
    #region CardValues

    public static readonly Card CardBack = new(0, "Back Of Card", Suit.Diamonds, "card_back");

    public static readonly Card[] cards = [
        new(2, "2 of Hearts", Suit.Hearts, "card_hearts_02"),
        new(3, "3 of Hearts", Suit.Hearts, "card_hearts_03"),
        new(4, "4 of Hearts", Suit.Hearts, "card_hearts_04"),
        new(5, "5 of Hearts", Suit.Hearts, "card_hearts_05"),
        new(6, "6 of Hearts", Suit.Hearts, "card_hearts_06"),
        new(7, "7 of Hearts", Suit.Hearts, "card_hearts_07"),
        new(8, "8 of Hearts", Suit.Hearts, "card_hearts_08"),
        new(9, "9 of Hearts", Suit.Hearts, "card_hearts_09"),
        new(10, "10 of Hearts", Suit.Hearts, "card_hearts_10"),
        new(11, "Jack of Hearts", Suit.Hearts, "card_hearts_J"),
        new(12, "Queen of Hearts", Suit.Hearts, "card_hearts_Q"),
        new(13, "King of Hearts", Suit.Hearts, "card_hearts_K"),
        new(14, "Ace of Hearts", Suit.Hearts, "card_hearts_A"),
        new(2, "2 of Diamonds", Suit.Diamonds, "card_diamonds_02"),
        new(3, "3 of Diamonds", Suit.Diamonds, "card_diamonds_03"),
        new(4, "4 of Diamonds", Suit.Diamonds, "card_diamonds_04"),
        new(5, "5 of Diamonds", Suit.Diamonds, "card_diamonds_05"),
        new(6, "6 of Diamonds", Suit.Diamonds, "card_diamonds_06"),
        new(7, "7 of Diamonds", Suit.Diamonds, "card_diamonds_07"),
        new(8, "8 of Diamonds", Suit.Diamonds, "card_diamonds_08"),
        new(9, "9 of Diamonds", Suit.Diamonds, "card_diamonds_09"),
        new(10, "10 of Diamonds", Suit.Diamonds, "card_diamonds_10"),
        new(11, "Jack of Diamonds", Suit.Diamonds, "card_diamonds_J"),
        new(12, "Queen of Diamonds", Suit.Diamonds, "card_diamonds_Q"),
        new(13, "King of Diamonds", Suit.Diamonds, "card_diamonds_K"),
        new(14, "Ace of Diamonds", Suit.Diamonds, "card_diamonds_A"),
        new(2, "2 of Clubs", Suit.Clubs, "card_clubs_02"),
        new(3, "3 of Clubs", Suit.Clubs, "card_clubs_03"),
        new(4, "4 of Clubs", Suit.Clubs, "card_clubs_04"),
        new(5, "5 of Clubs", Suit.Clubs, "card_clubs_05"),
        new(6, "6 of Clubs", Suit.Clubs, "card_clubs_06"),
        new(7, "7 of Clubs", Suit.Clubs, "card_clubs_07"),
        new(8, "8 of Clubs", Suit.Clubs, "card_clubs_08"),
        new(9, "9 of Clubs", Suit.Clubs, "card_clubs_09"),
        new(10, "10 of Clubs", Suit.Clubs, "card_clubs_10"),
        new(11, "Jack of Clubs", Suit.Clubs, "card_clubs_J"),
        new(12, "Queen of Clubs", Suit.Clubs, "card_clubs_Q"),
        new(13, "King of Clubs", Suit.Clubs, "card_clubs_K"),
        new(14, "Ace of Clubs", Suit.Clubs, "card_clubs_A"),
        new(2, "2 of Spades", Suit.Spades, "card_spades_02"),
        new(3, "3 of Spades", Suit.Spades, "card_spades_03"),
        new(4, "4 of Spades", Suit.Spades, "card_spades_04"),
        new(5, "5 of Spades", Suit.Spades, "card_spades_05"),
        new(6, "6 of Spades", Suit.Spades, "card_spades_06"),
        new(7, "7 of Spades", Suit.Spades, "card_spades_07"),
        new(8, "8 of Spades", Suit.Spades, "card_spades_08"),
        new(9, "9 of Spades", Suit.Spades, "card_spades_09"),
        new(10, "10 of Spades", Suit.Spades, "card_spades_10"),
        new(11, "Jack of Spades", Suit.Spades, "card_spades_J"),
        new(12, "Queen of Spades", Suit.Spades, "card_spades_Q"),
        new(13, "King of Spades", Suit.Spades, "card_spades_K"),
        new(14, "Ace of Spades", Suit.Spades, "card_spades_A")
    ];

    #endregion

    public enum Suit
    {
        Hearts,
        Diamonds,
        Clubs,
        Spades
    }

    public class Card
    {
        /// <summary>
        /// include the ending separator. e.g., "Images\\Cards\\" or "Images/Cards/"
        /// </summary>
        public static string CardDirectory = $"{Directory.GetCurrentDirectory()}\\GameStuff\\Cards\\";
        public int Value { get; init; }
        public string Name { get; init; }
        public Suit Suit { get; init; }
        public string TextureName { get; init; }
        public string TexturePath => $"{CardDirectory}{TextureName}";

        public Card(int value, string name, Suit suit, string textureName)
        {
            Value = value;
            Name = name;
            Suit = suit;
            TextureName = textureName + ".png";
        }

        public override bool Equals(object? obj)
        {
            return obj is Card card &&
                card.Value.Equals(Value) &&
                card.Suit.Equals(Suit);
        }

        public override int GetHashCode()
        {
            throw new NotImplementedException();
        }
    }
}
