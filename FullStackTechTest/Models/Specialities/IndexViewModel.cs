using DAL;
using Models;

namespace FullStackTechTest.Models.Specialities;

public class IndexViewModel
{
    public List<Speciality> SpecialityList { get; set; }

    public static async Task<IndexViewModel> CreateAsync(ISpecialityRepository specialityRepository)
    {
        var model = new IndexViewModel
        {
            SpecialityList = await specialityRepository.ListAllAsync()
        };
        return model;
    }
}