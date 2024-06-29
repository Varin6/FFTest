using DAL;
using Models;

namespace FullStackTechTest.Models.Specialities;

public class CreateViewModel
{
    public Speciality Speciality { get; set; }
    
    public static async Task<CreateViewModel> CreateAsync(string name = "")
    {
        var model = new CreateViewModel
        {
            Speciality = new Speciality(){Name = name}
        };
        
        return model;
    }
    
}