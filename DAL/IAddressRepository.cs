using Models;
using MySql.Data.MySqlClient;

namespace DAL;

public interface IAddressRepository
{
    Task<Address> GetForPersonIdAsync(int personId);
    Task SaveAsync(Address address);
    Task AddAsync(Address address);
    Task AddAsync(Address address, MySqlTransaction transaction);
}