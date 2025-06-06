namespace KaelsToolBox_2.Database.SQLite3;

public class Command(string commandText, bool isRead = false)
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
