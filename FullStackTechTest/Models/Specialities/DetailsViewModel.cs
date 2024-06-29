using DAL;
using Models;

namespace FullStackTechTest.Models.Specialities;

public class DetailsViewModel
{
    public Speciality Speciality { get; set; }
    public bool IsEditing { get; set; }

    public static async Task<DetailsViewModel> CreateAsync(int specialityId, bool isEditing, ISpecialityRepository specialityRepository)
    {
        var model = new DetailsViewModel
        {
            Speciality = await specialityRepository.GetByIdAsync(specialityId),
            IsEditing = isEditing
        };
        return model;
    }
}