using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KaelsToolBox_2.Web
{
    public class DB_Row : EventArgs
    {
        public string[] Data {  get; }

        public DB_Row(string[] data)
        {
            Data = data;
        }
    }

    public interface IDB_Item
    {
        string Identifier { get; set; }
        string Password { get; set; }
        EventHandler<DB_Row> RowAdded { get; set; }

        void HandleRow(DB_Row row);
    }

    public class DatabaseConnection
    {
        MySqlConnection connection;

        string server = "127.0.0.1";
        string port = "3306";
        string database = "people";
        string username = "root";
        string password = "my-secret-pw";

        string connectionStr => $"SERVER={server};PORT={port};DATABASE={database};UID={username};PASSWORD={password};";

        public DatabaseConnection(string server, string port, string database, string username, string password)
        {
            this.server = server;
            this.port = port;
            this.database = database;
            this.username = username;
            this.password = password;

            connection = new MySqlConnection(connectionStr);
        }

        /// <summary>
        /// Add an item to the Database
        /// </summary>
        /// <param name="table">the table in the database to insert into</param>
        /// <param name="columns">the columns that we give values for</param>
        /// <param name="values">the values to insert in the row</param>
        /// <param name="uniqueKey">if provided, insure no other item with Value already exists in the column of Key</param>
        /// <param name="uniqueValue">if provided, insure no other item with Value already exists in the column of Key</param>
        /// <returns></returns>
        public Error Add(string table, string[] columns, string[] values, string? uniqueKey, string? uniqueValue)
        {
            // check if everything exists
            if (table.Length == 0 || columns.Length == 0 || columns.Length != values.Length)
                return Error.Null;

            string columnsString = string.Empty;
            foreach (string column in columns) columnsString += $"`{column}`,";
            columnsString.TrimEnd(',');

            string valuesString = string.Empty;
            foreach (string value in values) valuesString += $"'{value}',";
            valuesString.TrimEnd(',');

            string query = $"INSERT INTO `{database}`.`{table}` ({columnsString}) VALUES({valuesString});";

            connection.Open();

            // check no duplicate
            if (uniqueKey is not null && uniqueValue is not null)
            {
                string query_check = $"SELECT '{uniqueKey}' FROM `{database}`.`{table}` WHERE `{uniqueKey}` = '{uniqueValue}';";

                var result = new MySqlCommand(query_check, connection).ExecuteReader();
                if (result.HasRows)
                {
                    connection.Close();
                    return Error.ExistingAccount;
                }
                result.Close();
            }            

            // add item
            new MySqlCommand(query, connection).ExecuteNonQuery();
            connection.Close();

            return Error.None;
        }

        //public Error CheckPassword(UserDB user)
        //{
        //    string query = "SELECT `password` FROM `people`.`users` WHERE " +
        //        $"`username` = '{user.username}' AND " +
        //        $"`email` = '{user.email}';";

        //    connection.Open();
        //    var response = new MySqlCommand(query, connection).ExecuteReader();
        //    var list = new List<string>();
        //    while (response.Read()) list.Add((string)response["password"]);
        //    connection.Close();

        //    if (list.Count > 1) return Error.MultipleAccountsFound;
        //    if (user.password is null) return Error.Null;
        //    if (user.password.Equals(list[0])) return Error.None;

        //    return Error.None;
        //}

        public enum Error
        {
            None,
            MultipleAccountsFound,
            ExistingAccount,
            Null
        }
    }

}
