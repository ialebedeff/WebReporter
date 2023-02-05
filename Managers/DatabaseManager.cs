using Entities;
using MySql.Data.MySqlClient;
using System.Data.Common;
using System.Diagnostics;

namespace Managers
{
    public class DatabaseManager
    {
        public async Task<List<Dictionary<string, object>>> ExecuteCommandAsync(Message<DatabaseCommand> request)
        {
            using (var connection = await GetConnectionAsync(request.Data.Connection))
            {
                using (var command = new MySqlCommand(request.Data.Command, connection))
                {
                    Debug.WriteLine(string.Format("SQL-команда: {0}", command.CommandText));

                    using (CancellationTokenSource tokenSource = new CancellationTokenSource(TimeSpan.FromSeconds(5)))
                    {
                        try
                        {
                            using (var reader = await command.ExecuteReaderAsync(tokenSource.Token))
                            {
                                return await Convert(reader);
                            }
                        }
                        catch (Exception ex) 
                        {
                            Console.ForegroundColor = ConsoleColor.Red;
                            Console.WriteLine(ex.Message);
                            Console.ForegroundColor = ConsoleColor.White; 
                            return new List<Dictionary<string, object>>();
                        }
                    }
                }
            }
        }
        private async Task<List<Dictionary<string, object>>> Convert(DbDataReader reader)
        {
            var list = new List<Dictionary<string, object>>();

            while (await reader.ReadAsync())
            {
                list.Add(new Dictionary<string, object>()
                {

                });

                for (int i = 0; i < reader.FieldCount; i += 1)
                {
                    if (!list.Last().ContainsKey(reader.GetName(i)))
                    {
                        list.Last().Add(
                            reader.GetName(i),
                            reader.GetValue(i));
                    }
                    else
                    {
                        list.Last()[reader.GetName(i)] = reader.GetValue(i);
                    }
                }
            }

            return list;
        }
        private async Task<MySqlConnection?> GetConnectionAsync(DatabaseConnectionInfo databaseConnection)
        {
            if (databaseConnection is null)
                return null;

            var connection = new MySqlConnection($"server={databaseConnection.Server};user={databaseConnection.User};pwd={databaseConnection.Password};database={databaseConnection.Database};Pooling=True;");
            await connection.OpenAsync();

            return connection;
        }
    }
}