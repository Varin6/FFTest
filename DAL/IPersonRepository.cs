using Models;
using MySql.Data.MySqlClient;

namespace DAL;

public interface IPersonRepository : IBaseRepository
{
    Task<List<Person>> ListAllAsync();
    Task<Person> GetByIdAsync(int personId);
    Task SaveAsync(Person person);
    Task<int> AddAsync(Person person);
    Task<int> AddAsync(Person person, MySqlTransaction transaction);
    Task<List<Person>> GetByGmcListAsync(List<int> gmcs);
    
}