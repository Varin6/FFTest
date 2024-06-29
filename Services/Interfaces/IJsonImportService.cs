namespace Services.Interfaces;

public interface IJsonImportService
{
    Task ImportDataAsync(string jsonData);
}