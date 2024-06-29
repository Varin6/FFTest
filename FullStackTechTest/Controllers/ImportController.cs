using System.Diagnostics;
using System.Text;
using DAL;
using Microsoft.AspNetCore.Mvc;
using FullStackTechTest.Models.Home;
using FullStackTechTest.Models.Import;
using FullStackTechTest.Models.Shared;
using Services.Interfaces;

namespace FullStackTechTest.Controllers;

public class ImportController : Controller
{
    private readonly ILogger<ImportController> _logger;
    private readonly IPersonRepository _personRepository;
    private readonly IAddressRepository _addressRepository;
    private readonly IJsonImportService _jsonImportService;

    public ImportController(ILogger<ImportController> logger, IPersonRepository personRepository, IAddressRepository addressRepository, IJsonImportService jsonImportService)
    {
        _logger = logger;
        _personRepository = personRepository;
        _addressRepository = addressRepository;
        _jsonImportService = jsonImportService;
    }

    public async Task<IActionResult> Index()
    {
        return View(new ImportViewModel());
    }

    
    [HttpPost]
    public async Task<IActionResult> Import(IFormFile jsonFile)
    {
        if (jsonFile == null || jsonFile.Length == 0)
        {
            return BadRequest("No file uploaded.");
        }

        using (var stream = new StreamReader(jsonFile.OpenReadStream()))
        {
            var jsonData = await stream.ReadToEndAsync();
            await _jsonImportService.ImportDataAsync(jsonData);
        }

        return Ok("Import successful.");
    }
    
    
    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}