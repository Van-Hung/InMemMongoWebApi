using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace InMemMongoWebApi.Collections
{
	public class Person
	{
		[BsonId]
		[BsonRepresentation(BsonType.ObjectId)]
		public string Id { get; set; }

		[BsonElement("name")]
		public string Name { get; set; }

		[BsonElement("age")]
		public int Age { get; set; }

		[BsonElement("phonenumbers")]
		public List<string> PhoneNumbers { get; set; }
	}
}
