using MySql.Data.MySqlClient;

namespace DAL
{
    public abstract class BaseRepository: IBaseRepository
    {
        protected string ConnectionString { get; } = Config.DbConnectionString;

        public async Task<MySqlTransaction> BeginTransactionAsync()
        {
            var connection = new MySqlConnection(ConnectionString);
            await connection.OpenAsync();
            return await connection.BeginTransactionAsync();
        }
    }
}