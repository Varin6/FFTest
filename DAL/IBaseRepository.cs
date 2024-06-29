using MySql.Data.MySqlClient;

namespace DAL;

public interface IBaseRepository
{
    Task<MySqlTransaction> BeginTransactionAsync();
}