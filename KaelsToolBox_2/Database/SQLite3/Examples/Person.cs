

namespace KaelsToolBox_2.Database.SQLite3.Examples;

public class Person : IDisplayInTable
{
    public static string[] Headers => ["Id", "Name", "Age"];
    /// <summary>
    /// Probs want the first one to be for Id - "INTEGER PRIMARY KEY AUTOINCREMENT"
    /// </summary>
    public static string[] HeaderArgs => ["INTEGER PRIMARY KEY AUTOINCREMENT", "TEXT", "TEXT"];

    private Person(int id, string name, DateTime age)
    {
        Id = id;
        Name = name;
        Age = age;
    }
    public Person(string name, DateTime age)
    {
        Name = name;
        Age = age;
    }

    public int Id { get; init; }
    public string Name;
    public DateTime Age; // yyyy-MM-dd HH:mm:ss.FFFFFFF fomat

    public string[] GetRow() => [$"\"{Name}\"", $"\"{Age:yyyy-MM-dd HH:mm:ss.FFFFFFF}\""];
    public static IDisplayInTable Load(List<string> db_result) => new Person(int.Parse(db_result[0]), db_result[1], DateTime.Parse(db_result[2]));
}