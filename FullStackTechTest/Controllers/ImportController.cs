using System.Diagnostics;
using System.Text;
using DAL;
using Microsoft.AspNetCore.Mvc;
using FullStackTechTest.Models.Home;
using FullStackTechTest.Models.Import;
using FullStackTechTest.Models.Shared;

namespace FullStackTechTest.Controllers;

public class ImportController : Controller
{
    private readonly ILogger<ImportController> _logger;
    private readonly IPersonRepository _personRepository;
    private readonly IAddressRepository _addressRepository;

    public ImportController(ILogger<ImportController> logger, IPersonRepository personRepository, IAddressRepository addressRepository)
    {
        _logger = logger;
        _personRepository = personRepository;
        _addressRepository = addressRepository;
    }

    public async Task<IActionResult> Index()
    {
        return View(new ImportViewModel());
    }

    
    
    
    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}