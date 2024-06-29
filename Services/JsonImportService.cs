using DAL;
using Models;
using Newtonsoft.Json;
using Services.Interfaces;

namespace Services;

public class JsonImportService : IJsonImportService
    {
        private readonly IPersonRepository _personRepository;
        private readonly IAddressRepository _addressRepository;

        public JsonImportService(IPersonRepository personRepository, IAddressRepository addressRepository)
        {
            _personRepository = personRepository;
            _addressRepository = addressRepository;
        }

        public async Task ImportDataAsync(string jsonData)
        {
            var persons = JsonConvert.DeserializeObject<List<PersonDto>>(jsonData);

            // Get existing GMC numbers from the database
            var existingPersons = await _personRepository.GetByGmcListAsync(persons.Select(p => p.GMC).ToList());

            // Filter out duplicates
            var newPersons = persons.Where(p => !existingPersons.Any(ep => ep.GMC == p.GMC)).ToList();

            if (newPersons.Any())
            {
                // Start a transaction
                await using (var transaction = await _personRepository.BeginTransactionAsync())
                {
                    try
                    {
                        foreach (var personDto in newPersons)
                        {
                            var person = new Person
                            {
                                FirstName = personDto.FirstName,
                                LastName = personDto.LastName,
                                GMC = personDto.GMC,
                            };

                            var personId = await _personRepository.AddAsync(person, transaction);

                            foreach (var addressDto in personDto.Address)
                            {
                                var address = new Address
                                {
                                    Line1 = addressDto.Line1,
                                    City = addressDto.City,
                                    Postcode = addressDto.Postcode,
                                    PersonId = personId
                                };

                                await _addressRepository.AddAsync(address, transaction);
                            }
                        }

                        // Commit transaction
                        await transaction.CommitAsync();
                    }
                    catch
                    {
                        // Rollback transaction in case of an error
                        await transaction.RollbackAsync();
                        throw;
                    }
                }
            }
        }

        private class PersonDto
        {
            public string FirstName { get; set; }
            public string LastName { get; set; }
            public int GMC { get; set; }
            public List<AddressDto> Address { get; set; }
        }

        private class AddressDto
        {
            public string Line1 { get; set; }
            public string City { get; set; }
            public string Postcode { get; set; }
        }
    }