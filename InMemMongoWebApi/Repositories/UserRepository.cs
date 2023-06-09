﻿using InMemMongoWebApi.Collections;
using MongoDB.Driver;
using MongoDB.Driver.Linq;

namespace InMemMongoWebApi.Repositories
{
	public class UserRepository : IUserRepository
	{
		private readonly IMongoCollection<User> _userCollection;

		public UserRepository(IMongoDatabase mongoDatabase)
		{
			_userCollection = mongoDatabase.GetCollection<User>("users", null);
		}

		public async Task<List<User>> GetAllAsync()
		{
			return await ((await _userCollection.FindAsync(_ => true)).ToListAsync());
			//return await _userCollection.Find(_ => true).ToListAsync();
		}

		public async Task<List<User>> GetAllAsyncByLinq()
		{
			var queryableUserCollection = _userCollection.AsQueryable().Where(x => true);
			return await queryableUserCollection.ToListAsync();
		}

		public async Task<User> GetByIdAsync(string id)
		{
			return await _userCollection.Find(_ => _.Id == id).FirstOrDefaultAsync();
		}

		public async Task<List<User>> GetUsersByNamePrefix(string namePrefix)
		{
			namePrefix = namePrefix.ToLower();
			var result = await _userCollection.FindAsync(x => x.Name.ToLower().StartsWith(namePrefix));
			return await result.ToListAsync();
		}

		public async Task CreateNewUserAsync(User newUser)
		{
			await _userCollection.InsertOneAsync(newUser);
		}

		public async Task UpdateUserAsync(User userToUpdate)
		{
			await _userCollection.ReplaceOneAsync(x => x.Id == userToUpdate.Id, userToUpdate);
		}

		public async Task DeleteUserAsync(string id)
		{
			await _userCollection.DeleteOneAsync(x => x.Id == id);
		}
	}
}
