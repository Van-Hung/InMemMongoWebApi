using AutoFixture.Xunit2;
using InMemMongoWebApi.Collections;
using InMemMongoWebApi.Repositories;

namespace InMemMongoWebApi.Test
{
	public class RealUser2Tests : UnitTestBase
	{
		private readonly UserRepository _userRepository2;

		public RealUser2Tests()
		{
			_userRepository2 = new UserRepository(_database2);
		}

		[Theory]
		[AutoData]
		public async Task GetUsersByNamePrefix_ReturnsCorrectUsers_WhenCalled(List<string> names)
		{
			foreach (var name in names)
			{
				// Arrange
				await _userRepository2.CreateNewUserAsync(new User { Id = "6419c7a5f23ddc04163af507", Name = "Anna", Age = 30 });
				await _userRepository2.CreateNewUserAsync(new User { Id = "6419c7aeffeeb0eeb8f9e5d6", Name = "John", Age = 40 });

				// Act
				var result = await _userRepository2.GetUsersByNamePrefix("ANN");

				// Assert
				result.ShouldNotBeNull();
				result.Count.ShouldBe(1);
			}
		}
	}
}
