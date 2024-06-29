using DAL;
using Models;

namespace FullStackTechTest.Models.Import;

public class ImportViewModel
{
    public IFormFile JsonFile { get; set; }
    public string? Message { get; set; }
    public bool IsError { get; set; }
}