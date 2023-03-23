using AutoFixture.Xunit2;
using InMemMongoWebApi.Collections;
using InMemMongoWebApi.Repositories;

namespace InMemMongoWebApi.Test
{
	public class RealUserRepositoryTests: UnitTestBase
	{
		private readonly UserRepository _userRepository;

		public RealUserRepositoryTests()
		{
			_userRepository = new UserRepository(_database);
		}

		[Theory]
		[AutoData]
		public async Task GetAllAsync_ReturnsAllUsers_WhenCalled(List<string> names)
		{
			foreach (var name in names)
			{
				// Arrange
				await _userRepository.CreateNewUserAsync(new User { Id = "6419c7a5f23ddc04163af507", Name = "Anna", Age = 30 });
				await _userRepository.CreateNewUserAsync(new User { Id = "6419c7aeffeeb0eeb8f9e5d6", Name = "John", Age = 40 });

				// Act
				var result = await _userRepository.GetAllAsync();

				// Assert
				result.ShouldNotBeNull();
				result.Count.ShouldBe(2);
			}
		}
	}
}
