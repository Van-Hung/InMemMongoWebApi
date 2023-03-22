using InMemMongoWebApi.Collections;
using InMemMongoWebApi.Repositories;

namespace InMemMongoWebApi.Test
{
	public class TransactionTests
	{
		private readonly IUserRepository _userRepository;
		private readonly MongoClient _client;

		public TransactionTests()
		{
			string uri = "mongodb://localhost:8007";
			_client = new MongoClient(uri);
			var database = _client.GetDatabase("transactiondb");
			_userRepository = new UserRepository(database);
		}

		[Fact]
		public async Task Test_2Docs()
		{
			// Arrange
			await _userRepository.CreateNewUserAsync(new User { Id = "6419c7a5f23ddc04163af507", Name = "Anna", Age = 30 });

			// Act
			using (var session = _client.StartSession())
			{
				// Step 2: Optional. Define options to use for the transaction.
				var transactionOptions = new TransactionOptions(
						readPreference: ReadPreference.Primary,
						readConcern: ReadConcern.Local,
						writeConcern: WriteConcern.WMajority);
				// Step 3: Define the sequence of operations to perform inside the transactions
				var cancellationToken = CancellationToken.None; // normally a real token would be used
				var result = session.WithTransaction(
						(s, ct) =>
						{
							_userRepository.CreateNewUserAsync(new User { Id = "6419c7aeffeeb0eeb8f9e5d6", Name = "John", Age = 40 }).GetAwaiter().GetResult();
							_userRepository.CreateNewUserAsync(new User { Id = "6419c7a5f23ddc04163af507", Name = "Mithril", Age = 350 }).GetAwaiter().GetResult();

							return "Inserted 2 documents into the same collection";
						},
						transactionOptions,
						cancellationToken);
			}

			// Assert
			var users = await _userRepository.GetAllAsync();
			users.ShouldNotBeNull();
			users.Count.ShouldBe(1);
		}
	}
}
