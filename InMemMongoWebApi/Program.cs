using InMemMongoWebApi;
using InMemMongoWebApi.Models;
using InMemMongoWebApi.Repositories;
using MongoDB.Driver;

var builder = WebApplication.CreateBuilder(args);

// Retrieve the connection string
string connectionString = builder.Configuration.GetConnectionString("AppConfig");

if (connectionString.StartsWith("Read-from-local"))
{
	// read from local file
}
else
{
	// Load configuration from Azure App Configuration
	builder.Configuration.AddAzureAppConfiguration(connectionString);

}

// Bind configuration "TestApp:Settings" section to the Settings object
builder.Services.Configure<Settings>(builder.Configuration.GetSection("TestApp:Settings"));

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.Configure<MongoDBSettings>(
		builder.Configuration.GetSection("MongoDBSettings")
);
builder.Services.AddSingleton<IMongoDatabase>(options => {
	var settings = builder.Configuration.GetSection("MongoDBSettings").Get<MongoDBSettings>();
	var client = new MongoClient(settings.ConnectionString);
	return client.GetDatabase(settings.DatabaseName);
});
builder.Services.AddSingleton<IUserRepository, UserRepository>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
	app.UseSwagger();
	app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
