using KaelsToolBox_2.Database.SQLite3;
using KaelsToolBox_2.Database.SQLite3.Examples;

namespace ConsoleApp;

internal class Program
{
    static void Main(string[] args)
    {
        DataBaseManager manager = new("D:\\Dev\\Application Files\\Databases\\SQLite3.db", 0.1);
        DatabaseTable<Person> table = new("PersonTable", manager);
        DatabaseTable<Book> bookTable = new("Bookshelf", manager);

        table.LoadAll(out Person[] people);
        bookTable.LoadAll(out Book[] books);

        while (manager.GetCommandQue() != 0)
        {
            //Console.WriteLine(manager.GetCommandQue());
            Thread.Sleep(10);
        }

        foreach (Person person in people)
        {
            Console.WriteLine($"{person.Id}, {person.Name}");
        }

        foreach(Book book in books)
        {
            Console.WriteLine($"{book.Id}, {book.Title}");
        }

        Console.ReadLine();
    }
}

public class Book(string title, string description, string author, int pages) : IDisplayInTable
{
    public static string[] Headers => ["Id", "Title", "Description", "Author", "Pages"];
    public static string[] HeaderArgs => ["INTEGER PRIMARY KEY AUTOINCREMENT", "TEXT", "TEXT", "TEXT", "INTEGER"];

    public int Id { get; init; }
    public string Title = title;
    public string Description = description;
    public string Author = author;
    public int Pages = pages;

    public string[] GetRow() => [$"\"{Title}\"", $"\"{Description}\"", $"\"{Author}\"", $"{Pages}"];

    public static IDisplayInTable Load(List<string> db_result)
    {
        return new Book(db_result[1], db_result[2], db_result[3], int.Parse(db_result[4])) { Id = int.Parse(db_result[0]) };
    }
}