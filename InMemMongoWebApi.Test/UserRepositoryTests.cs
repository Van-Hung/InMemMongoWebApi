using InMemMongoWebApi.Collections;
using InMemMongoWebApi.Repositories;

namespace InMemMongoWebApi.Test
{
	public class UserRepositoryTests
	{
		private readonly Mock<IMongoCollection<User>> _mockCollection;
		private readonly Mock<IMongoDatabase> _mockDatabase;
		private readonly UserRepository _userRepository;
		private readonly List<User> _users;

		public UserRepositoryTests()
		{
			_mockCollection = new Mock<IMongoCollection<User>>();
			_users = new List<User>
				{
						new User { Id = "6419c7a5f23ddc04163af507", Name = "Anna", Age = 30 },
						new User { Id = "6419c7aeffeeb0eeb8f9e5d6", Name = "John", Age = 40 }
				};
			_mockCollection.Object.InsertMany(_users);
			_mockDatabase = new Mock<IMongoDatabase>();
			_mockDatabase.Setup(x => x.GetCollection<User>("users", null)).Returns(_mockCollection.Object);
			_userRepository = new UserRepository(_mockDatabase.Object);
		}

		[Fact]
		public async Task GetAllAsync_ReturnsAllUsers_WhenCalled()
		{
			// Arrange
			var mockCursor = new Mock<IAsyncCursor<User>>();
			mockCursor.Setup(_ => _.Current).Returns(_users);
			mockCursor
					.SetupSequence(_ => _.MoveNext(It.IsAny<CancellationToken>()))
					.Returns(true)
					.Returns(false);
			mockCursor
					.SetupSequence(_ => _.MoveNextAsync(It.IsAny<CancellationToken>()))
					.Returns(Task.FromResult(true))
					.Returns(Task.FromResult(false));

			_mockCollection
					.Setup(x => x.FindAsync(
									It.IsAny<FilterDefinition<User>>(),
									It.IsAny<FindOptions<User, User>>(),
									It.IsAny<CancellationToken>()
							))
					.ReturnsAsync(mockCursor.Object);

			// Act
			var result = await _userRepository.GetAllAsync();

			// Assert
			result.ShouldNotBeNull();
			result.Count.ShouldBe(_users.Count);
		}
	}
}
