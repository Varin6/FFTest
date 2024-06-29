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

    private readonly IJsonImportService _jsonImportService;

    public ImportController(ILogger<ImportController> logger, IJsonImportService jsonImportService)
    {
        _logger = logger;
        _jsonImportService = jsonImportService;
    }

    public async Task<IActionResult> Index()
    {
        return View(new ImportViewModel());
    }

    
    [HttpPost]
    public async Task<IActionResult> Index(IFormFile jsonFile)
    {
        try
        {
            if (jsonFile == null || jsonFile.Length == 0)
            {
                _logger.LogError("No file uploaded.");
                return View(new ImportViewModel(){Message = "No file uploaded.", IsError = true});
            }

            using (var stream = new StreamReader(jsonFile.OpenReadStream()))
            {
                var jsonData = await stream.ReadToEndAsync();
                var message = await _jsonImportService.ImportDataAsync(jsonData);
                
                return View(new ImportViewModel(){Message = message});
            }

            
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Error importing data from Json file.");
            return View(new ImportViewModel(){Message = "Import unsuccessful. Please check the file and try again.", IsError = true});
        }
        
        
    }
    
    
    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}