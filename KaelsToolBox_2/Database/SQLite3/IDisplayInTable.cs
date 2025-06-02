namespace KaelsToolBox_2.Database.SQLite3;

public interface IDisplayInTable
{
    // https://learn.microsoft.com/en-us/dotnet/standard/data/sqlite/types

    abstract static string[] Headers { get; }
    abstract static string[] HeaderArgs { get; }

    string[] GetRow();
    void Save();
}
