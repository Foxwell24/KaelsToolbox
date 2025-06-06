using System.Data;

namespace KaelsToolBox_2.Database.SQLite3;

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

        manager.RunCommand(new($"CREATE TABLE IF NOT EXISTS {TableName} ({cols})"));
    }

    public void Save(T item)
    {
        List<int> skips = [];
        for (int i = 0; i < T.HeaderArgs.Length; i++)
            if (T.HeaderArgs[i].Contains("AUTOINCREMENT"))
                skips.Add(i);

        List<string> headers = [];
        for (int i = 0; i < T.Headers.Length; i++)
            if (!skips.Contains(i))
                headers.Add(T.Headers[i]);

        manager.RunCommand(new($"INSERT INTO {TableName} ({string.Join(", ", headers)}) VALUES({string.Join(", ", item.GetRow())})"));
    }

    public bool Load(int Id, out T? result)
    {
        Command command = new($"SELECT * FROM {TableName} WHERE Id = {Id}", true);
        manager.RunCommand(command);

        result = default;

        var results = command.GetResult().Result;
        if (results.Count == 0) return false;
        if (results.Count > 1) throw new Exception("More than 1 item with same ID, wtf happend");

        result = (T)T.Load(results[0]);

        return true;
    }

    public void LoadAll(out T[] result)
    {
        Command command = new($"SELECT * FROM {TableName}", true);
        manager.RunCommand(command);
        var results = command.GetResult().Result;

        result = [];
        foreach (var row in results)
        {
            result = [..result, (T)T.Load(row)];
        }
    }
}
