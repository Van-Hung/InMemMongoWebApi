using InMemMongoWebApi.Collections;

namespace InMemMongoWebApi.Repositories
{
	public interface IPersonRepository
	{
		Task<List<Person>> GetAllAsync();
		Task<Person> GetByIdAsync(string id);
		Task CreateNewPersonAsync(Person newPerson);
		Task UpdatePersonAsync(Person personToUpdate);
		Task DeletePersonAsync(string id);
	}
}
