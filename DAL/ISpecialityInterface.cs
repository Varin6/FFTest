﻿using System.Collections.Generic;
using System.Threading.Tasks;
using Models;

namespace DAL;

public interface ISpecialityRepository : IBaseRepository
{
    Task<List<Speciality>> ListAllAsync();
    Task<Speciality> GetByIdAsync(int specialityId);
    Task<int> AddAsync(Speciality speciality);
    Task SaveAsync(Speciality speciality);
    Task RemoveAsync(Speciality speciality);
    Task AddSpecialityToPersonAsync(int personId, int specialityId);
    Task RemoveSpecialityFromPersonAsync(int personId, int specialityId);
    Task<List<Speciality>> GetSpecialitiesByPersonIdAsync(int personId);
}