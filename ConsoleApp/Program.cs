using KaelsToolBox_2.Web.Database.MongoDB;

namespace ConsoleApp;

internal class Program
{
    static void Main(string[] args)
    {
        Connection connection = GetDatabase();

        string database_name = "TestingDB";
        string database_main = "Main";

        /*var add_loop = Task.Run(() =>
        {
            while (true)
            {
                connection.Insert(database_name, database_main, new DatabaseObject());
                Thread.Sleep(1000);
            }
        });*/

        var change_loop = Task.Run(() =>
        {
            int counter = 0;
            while (true)
            {
                var all = connection.GetAll<DatabaseObject>(database_name, database_main);
                var chosen = all[Random.Shared.Next(all.Count)];

                connection.Update(database_name, database_main, chosen with { Note = counter++.ToString("#00")});
                Thread.Sleep(1100);
            }
        });

        connection.Watch<DatabaseObject>(database_name, database_main, changed => Console.Out.WriteLineAsync(changed.ToString()));

        Task.WaitAll([change_loop]);

        //Console.ReadKey();
    }

    private static Connection GetDatabase()
    {
        string user = "kael";
        string pword = "%23Foxwell24";
        string address = "invoicing.mjfbjr4.mongodb.net";
        var c = new Connection($"mongodb+srv://{user}:{pword}@{address}/?retryWrites=true&w=majority&appName=Invoicing");

        if (!c.Connect()) throw new Exception("Could not connect to Database");

        return c;
    }
}