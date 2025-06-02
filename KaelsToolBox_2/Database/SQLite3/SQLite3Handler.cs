using Microsoft.Data.Sqlite;
using MongoDB.Driver;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KaelsToolBox_2.Database.SQLite3;

public class DataBaseManager
{
    private readonly SqliteConnection db;
    private readonly PeriodicTimer update_tick;
    private List<Command> commands = [];

    public DataBaseManager(string DBFilePath, double UpdateDelaySeconds)
    {
        db = new($"Data Source={DBFilePath}");
        update_tick = new(TimeSpan.FromSeconds(UpdateDelaySeconds));

        Update();
    }

    public void RunCommand(Command command) => commands.Add(command);


    private async void Update()
    {
        while (await update_tick.WaitForNextTickAsync())
        {
            if (commands.Count == 0) continue;
            Command command = commands[0];

            try
            {
                if (db.State != ConnectionState.Open)
                {
                    if (db.State == ConnectionState.Closed) db.Open();
                    else throw new Exception("DB not open or closed!");
                }


                var cmd = new SqliteCommand(command.CommandText, db);

                if (!command.IsRead) cmd.ExecuteNonQuery();
                else
                {
                    using var reader = cmd.ExecuteReader();
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            var row = new List<string>();
                            for (int i = 0; i < reader.FieldCount; i++)
                            {
                                if (reader.IsDBNull(i)) continue;
                                string col_name = reader.GetName(i);
                                string? value = reader.GetValue(i).ToString();
                                if (!int.TryParse(value, out _)) value = $"\"{value}\""; // if not an int, add "" around it - might want to be doing this on input, not output, idk

                                row.Add($"{col_name}:{value}");
                            }

                            command.Output.Add(row);
                        }
                    }
                    command.Completed = true;
                }

                commands.RemoveAt(0);
            }
            catch (Exception e)
            {
                throw new Exception($"Error when running command [{command.CommandText}] | error text - | {e.Message}");
            }
        }
    }
}

public class Command(string commandText, bool isRead)
{
    public readonly string CommandText = commandText;
    public readonly bool IsRead = isRead;

    public List<List<string>> Output = [];
    public bool Completed = false;

    public async Task<List<List<string>>> GetResult(int check_frequency_milliseconds = 10)
    {
        while (!Completed) await Task.Delay(check_frequency_milliseconds);
        return Output;
    }
}

public class DatabaseTable<T> where T : IDisplayInTable
{

    private readonly DataBaseManager manager;
    public readonly string TableName;
    public event EventHandler<T>? Added;

    public DatabaseTable(string tableName, DataBaseManager manager)
    {
        TableName = tableName;
        this.manager = manager;

        var header_and_args = T.Headers.Zip(T.HeaderArgs, (Header, Arg) => new { Header, Arg }) // zip the headers and args together so they are in a (header, arg) format
            .Select(zip => $"{zip.Header} {zip.Arg}"); // combine each (header, arg) into a single string of "header arg"

        string cols = string.Join(", ", header_and_args); // adds ", " inbetween each item and combines result into single string

        manager.RunCommand(new($"CREATE TABLE IF NOT EXISTS {TableName} ({cols})", false));
    }
}

public class Person : IDisplayInTable
{
    public static string[] Headers => ["Id", "Name", "Age"];
    public static string[] HeaderArgs => ["INTEGER PRIMARY KEY AUTOINCREMENT", "TEXT", "TEXT"];

    public readonly int Id;
    public string Name;
    public DateTime Age; // yyyy-MM-dd HH:mm:ss.FFFFFFF fomat


    public string[] GetRow() => [Name, Age.ToString("yyyy-MM-dd HH:mm:ss.FFFFFFF")];
    public void Save()
    {
        throw new NotImplementedException();
    }
}