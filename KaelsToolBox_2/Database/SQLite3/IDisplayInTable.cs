namespace KaelsToolBox_2.Database.SQLite3;

public interface IDisplayInTable
{
    // https://learn.microsoft.com/en-us/dotnet/standard/data/sqlite/types

    abstract static string[] Headers { get; }
    abstract static string[] HeaderArgs { get; }
    abstract static IDisplayInTable Load(List<string> db_result);

    int Id { get; }


    /// <summary>
    /// AUTOINCREMENT items will not save, and if are placed in here, program will throw exception
    /// </summary>
    /// <returns></returns>
    string[] GetRow();
}
