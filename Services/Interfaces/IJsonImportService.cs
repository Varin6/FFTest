namespace Services.Interfaces;

public interface IJsonImportService
{
    Task<string?> ImportDataAsync(string jsonData);
}