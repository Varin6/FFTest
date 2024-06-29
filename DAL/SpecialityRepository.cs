using System.Data;
using System.Text;
using Models;
using MySql.Data.MySqlClient;

namespace DAL;

public class SpecialityRepository : BaseRepository, ISpecialityRepository
{
    public async Task<List<Speciality>> ListAllAsync()
    {
        var specialities = new List<Speciality>();
        
        var sql = new StringBuilder();
        sql.AppendLine("SELECT * FROM specialities");

        await using (var connection = new MySqlConnection(Config.DbConnectionString))
        {
            await connection.OpenAsync();
            
            var command = new MySqlCommand(sql.ToString(), connection);
            
            var reader = await command.ExecuteReaderAsync();
            while (await reader.ReadAsync())
            {
                specialities.Add(PopulateSpeciality(reader));
            }
        }

        return specialities;
    }
    
    
    
    public async Task<int> AddAsync(Speciality speciality)
    {
        var sql = new StringBuilder();
        sql.AppendLine("INSERT INTO specialities (Name)");
        sql.AppendLine("VALUES (@name);");
        sql.AppendLine("SELECT LAST_INSERT_ID();");

        await using (var connection = new MySqlConnection(Config.DbConnectionString))
        {
            await connection.OpenAsync();

            var command = new MySqlCommand(sql.ToString(), connection);
            command.Parameters.AddWithValue("name", speciality.Name);
            
            var specialityId = Convert.ToInt32(await command.ExecuteScalarAsync());
            return specialityId;
        }
    }
    
    
    public async Task SaveAsync(Speciality speciality)
    {
        var sql = new StringBuilder();
        sql.AppendLine("UPDATE specialities SET");
        sql.AppendLine("Name = @name");
        sql.AppendLine("WHERE Id = @specialityId");
        
        await using (var connection = new MySqlConnection(Config.DbConnectionString))
        {
            await connection.OpenAsync();

            var command = new MySqlCommand(sql.ToString(), connection);
            command.Parameters.AddWithValue("name", speciality.Name);
            command.Parameters.AddWithValue("specialityId", speciality.Id);

            await command.ExecuteNonQueryAsync();
        }
    }
    
    public async Task RemoveAsync(Speciality speciality)
    {
        
        // First delete all associations with persons
        
        var sql = new StringBuilder();
        sql.AppendLine("DELETE FROM person_specialities");
        sql.AppendLine("WHERE SpecialityId = @specialityId");

        await using (var connection = new MySqlConnection(Config.DbConnectionString))
        {
            await connection.OpenAsync();

            var command = new MySqlCommand(sql.ToString(), connection);
            command.Parameters.AddWithValue("specialityId", speciality.Id);

            await command.ExecuteNonQueryAsync();
        }
        
        // Then delete the speciality
        
        sql = new StringBuilder();
        sql.AppendLine("DELETE FROM specialities");
        sql.AppendLine("WHERE Id = @specialityId");
        
        await using (var connection = new MySqlConnection(Config.DbConnectionString))
        {
            await connection.OpenAsync();

            var command = new MySqlCommand(sql.ToString(), connection);
            command.Parameters.AddWithValue("specialityId", speciality.Id);

            await command.ExecuteNonQueryAsync();
        }
    }
    

    public async Task<Speciality> GetByIdAsync(int specialityId)
    {
        var speciality = new Speciality();
        
        var sql = new StringBuilder();
        sql.AppendLine("SELECT * FROM specialities WHERE Id = @specialityId");

        await using (var connection = new MySqlConnection(Config.DbConnectionString))
        {
            await connection.OpenAsync();

            var command = new MySqlCommand(sql.ToString(), connection);
            command.Parameters.AddWithValue("specialityId", specialityId);

            var reader = await command.ExecuteReaderAsync();
            if (await reader.ReadAsync())
            {
                speciality = PopulateSpeciality(reader);
            }
        }

        return speciality;
    }

    
    public async Task UpdatePersonSpecialitiesAsync(int personId, List<int> newSpecialityIds)
    {
        await using (var connection = new MySqlConnection(Config.DbConnectionString))
        {
            await connection.OpenAsync();

            using (var transaction = await connection.BeginTransactionAsync())
            {
                // Clear all existing specialities for the person
                var deleteSql = "DELETE FROM person_specialities WHERE PersonId = @personId";
                var deleteCommand = new MySqlCommand(deleteSql, connection, transaction);
                deleteCommand.Parameters.AddWithValue("personId", personId);
                await deleteCommand.ExecuteNonQueryAsync();

                // Add new specialities
                if (newSpecialityIds != null && newSpecialityIds.Any())
                {
                    var insertSql = new StringBuilder();
                    insertSql.AppendLine("INSERT INTO person_specialities (PersonId, SpecialityId) VALUES");

                    for (int i = 0; i < newSpecialityIds.Count; i++)
                    {
                        if (i > 0) insertSql.AppendLine(",");
                        insertSql.Append($"(@personId, @specialityId{i})");
                    }
                    insertSql.Append(";");

                    var insertCommand = new MySqlCommand(insertSql.ToString(), connection, transaction);
                    insertCommand.Parameters.AddWithValue("personId", personId);
                    for (int i = 0; i < newSpecialityIds.Count; i++)
                    {
                        insertCommand.Parameters.AddWithValue($"specialityId{i}", newSpecialityIds[i]);
                    }

                    await insertCommand.ExecuteNonQueryAsync();
                }

                await transaction.CommitAsync();
            }
        }
    }

    public async Task<List<Speciality>> GetSpecialitiesByPersonIdAsync(int personId)
    {
        var specialities = new List<Speciality>();

        var sql = new StringBuilder();
        sql.AppendLine("SELECT s.* FROM specialities s");
        sql.AppendLine("JOIN person_specialities ps ON s.Id = ps.SpecialityId");
        sql.AppendLine("WHERE ps.PersonId = @personId");

        await using (var connection = new MySqlConnection(Config.DbConnectionString))
        {
            await connection.OpenAsync();

            var command = new MySqlCommand(sql.ToString(), connection);
            command.Parameters.AddWithValue("personId", personId);

            var reader = await command.ExecuteReaderAsync();
            while (await reader.ReadAsync())
            {
                specialities.Add(PopulateSpeciality(reader));
            }
        }

        return specialities;
    }

    private Speciality PopulateSpeciality(IDataRecord data)
    {
        var speciality = new Speciality
        {
            Id = int.Parse(data["Id"].ToString()),
            Name = data["Name"].ToString()
        };
        return speciality;
    }
}


