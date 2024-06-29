using DAL;
using Models;

namespace FullStackTechTest.Models.Home;

public class DetailsViewModel
{
    public Person Person { get; set; }
    public Address Address { get; set; }
    public List<int> SelectedSpecialityIds { get; set; }
    public List<Speciality> SelectedSpecialities { get; set; }
    public List<Speciality> Specialities { get; set; }
    public bool IsEditing { get; set; }

    public static async Task<DetailsViewModel> CreateAsync(
        int personId, 
        bool isEditing, 
        IPersonRepository personRepository, 
        IAddressRepository addressRepository, 
        ISpecialityRepository specialityRepository
        )
    {
        
        var selectedSpecialities = await specialityRepository.GetSpecialitiesByPersonIdAsync(personId);
        
        var model = new DetailsViewModel
        {
            Person = await personRepository.GetByIdAsync(personId),
            Address = await addressRepository.GetForPersonIdAsync(personId),
            SelectedSpecialityIds = selectedSpecialities.Select(x => x.Id).ToList(),
            SelectedSpecialities = selectedSpecialities,
            Specialities = await specialityRepository.ListAllAsync(),
            IsEditing = isEditing
        };
        return model;
    }
}