using System.Data;
using System.Text;
using Models;
using MySql.Data.MySqlClient;

namespace DAL;

public class AddressRepository : BaseRepository, IAddressRepository
{
    public async Task<Address> GetForPersonIdAsync(int personId)
    {
        var address = new Address();
        
        var sql = new StringBuilder();
        sql.AppendLine("SELECT * FROM addresses");
        sql.AppendLine("WHERE PersonId = @personId");

        await using (var connection = new MySqlConnection(Config.DbConnectionString))
        {
            await connection.OpenAsync();
            
            var command = new MySqlCommand(sql.ToString(), connection);
            command.Parameters.AddWithValue("personId", personId);
            
            var reader = await command.ExecuteReaderAsync();
            if (await reader.ReadAsync())
            {
                address = PopulateAddress(reader);
            }
        }

        return address;
    }

    public async Task SaveAsync(Address address)
    {
        var sql = new StringBuilder();
        sql.AppendLine("UPDATE addresses SET");
        sql.AppendLine("Line1 = @line1,");
        sql.AppendLine("City = @city,");
        sql.AppendLine("Postcode = @postcode");
        sql.AppendLine("WHERE Id = @addressId");
        
        await using (var connection = new MySqlConnection(Config.DbConnectionString))
        {
            await connection.OpenAsync();

            var command = new MySqlCommand(sql.ToString(), connection);
            command.Parameters.AddWithValue("line1", address.Line1);
            command.Parameters.AddWithValue("city", address.City);
            command.Parameters.AddWithValue("postcode", address.Postcode);
            command.Parameters.AddWithValue("addressId", address.Id);

            await command.ExecuteNonQueryAsync();
        }
    }

    private Address PopulateAddress(IDataRecord data)
    {
        var address = new Address
        {
            Id = int.Parse(data["Id"].ToString()),
            PersonId = int.Parse(data["Id"].ToString()),
            Line1 = data["Line1"].ToString(),
            City = data["City"].ToString(),
            Postcode = data["Postcode"].ToString()
        };
        return address;
    }
    
    
    public async Task AddAsync(Address address)
    {
        var sql = new StringBuilder();
        sql.AppendLine("INSERT INTO addresses (Line1, City, Postcode, PersonId)");
        sql.AppendLine("VALUES (@line1, @city, @postcode, @personId);");

        await using (var connection = new MySqlConnection(Config.DbConnectionString))
        {
            await connection.OpenAsync();

            var command = new MySqlCommand(sql.ToString(), connection);
            command.Parameters.AddWithValue("line1", address.Line1);
            command.Parameters.AddWithValue("city", address.City);
            command.Parameters.AddWithValue("postcode", address.Postcode);
            command.Parameters.AddWithValue("personId", address.PersonId);

            await command.ExecuteNonQueryAsync();
        }
    }

    public async Task AddAsync(Address address, MySqlTransaction transaction)
    {
        var sql = new StringBuilder();
        sql.AppendLine("INSERT INTO addresses (Line1, City, Postcode, PersonId)");
        sql.AppendLine("VALUES (@line1, @city, @postcode, @personId);");

        var command = new MySqlCommand(sql.ToString(), transaction.Connection, transaction);
        command.Parameters.AddWithValue("line1", address.Line1);
        command.Parameters.AddWithValue("city", address.City);
        command.Parameters.AddWithValue("postcode", address.Postcode);
        command.Parameters.AddWithValue("personId", address.PersonId);

        await command.ExecuteNonQueryAsync();
    }
}