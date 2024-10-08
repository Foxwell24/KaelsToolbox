﻿using KaelsToolBox_2.Math;
using KaelsToolBox_2.Web.Database.MongoDB;

namespace ConsoleApp;

internal class Program
{
    static void Main(string[] args)
    {
        var i = Normalizer.Floats([1f, 1f, 1f, 1f, 1f, 5f, 10f, 10f]);

        float total = 0f;
        foreach (var item in i)
        {
            Console.WriteLine(item);
            total += item;
        }
        Console.WriteLine("---");
        Console.WriteLine(total);

        Console.ReadLine();

        return;

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
            while (true)
            {
                var all = connection.GetAll<DatabaseObject>(database_name, database_main);
                var chosen = all[Random.Shared.Next(all.Count)];

                connection.Update(database_name, database_main, chosen);
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