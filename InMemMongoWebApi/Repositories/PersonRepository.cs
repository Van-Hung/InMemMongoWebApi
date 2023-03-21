using InMemMongoWebApi.Collections;
using MongoDB.Driver;

namespace InMemMongoWebApi.Repositories
{
	public class PersonRepository : IPersonRepository
	{
		private readonly IMongoCollection<Person> _personCollection;

		public PersonRepository(IMongoDatabase mongoDatabase)
		{
			_personCollection = mongoDatabase.GetCollection<Person>("person");
		}

		public async Task<List<Person>> GetAllAsync()
		{
			return await _personCollection.Find(_ => true).ToListAsync();
		}

		public async Task<Person> GetByIdAsync(string id)
		{
			return await _personCollection.Find(_ => _.Id == id).FirstOrDefaultAsync();
		}

		public async Task CreateNewPersonAsync(Person newPerson)
		{
			await _personCollection.InsertOneAsync(newPerson);
		}

		public async Task UpdatePersonAsync(Person personToUpdate)
		{
			await _personCollection.ReplaceOneAsync(x => x.Id == personToUpdate.Id, personToUpdate);
		}

		public async Task DeletePersonAsync(string id)
		{
			await _personCollection.DeleteOneAsync(x => x.Id == id);
		}
	}
}
