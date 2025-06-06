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
    public int GetCommandQue() => commands.Count;

    public DataBaseManager(string DBFilePath, double UpdateDelaySeconds)
    {
        db = new($"Data Source={DBFilePath}");
        update_tick = new(TimeSpan.FromSeconds(UpdateDelaySeconds));

        Update();
    }

    public void RunCommand(Command command) => commands.Add(command);


    private async void Update()
    {
        while (await update_tick.WaitForNextTickAsync(CancellationToken.None))
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
                                //if (!int.TryParse(value, out _)) value = $"\"{value}\""; // if not an int, add "" around it - might want to be doing this on input, not output, idk
                                if (!int.TryParse(value, out _)) value = $"{value}"; // if not an int, add "" around it - might want to be doing this on input, not output, idk

                                row.Add(value);
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
