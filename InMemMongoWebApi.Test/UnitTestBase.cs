namespace InMemMongoWebApi.Test
{
	public abstract class UnitTestBase: IDisposable
	{
		private readonly string _databaseName = "testdb";
		private readonly MongoClient _client;
		protected IMongoDatabase _database;
		protected IMongoDatabase _database2;

		protected UnitTestBase()
		{
			var uri = "mongodb://localhost:8007";
			_client = new MongoClient(uri);
			_database = _client.GetDatabase(_databaseName);
			_database2 = _client.GetDatabase("testdb2");
		}

		public void Dispose()
		{
			_client.DropDatabase(_databaseName);
			_client.DropDatabase("testdb2");
		}
	}
}
