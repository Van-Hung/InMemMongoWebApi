using InMemMongoWebApi.Collections;

namespace InMemMongoWebApi.Repositories
{
	public interface IUserRepository
	{
		Task<List<User>> GetAllAsync();
		Task<User> GetByIdAsync(string id);
		Task CreateNewUserAsync(User newUser);
		Task UpdateUserAsync(User userToUpdate);
		Task DeleteUserAsync(string id);
	}
}
