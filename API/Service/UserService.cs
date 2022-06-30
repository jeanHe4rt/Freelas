using API.Models;
using API.Repository.Models;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace API.Service
{
    public class UserService
    {
        private readonly IMongoCollection<User> _collection;

        public UserService(IOptions<MongoSettings> mongoSettings)
        {
            var mongoClient = new MongoClient(mongoSettings.Value.ConnectionString);
            var mongoDatabase = mongoClient.GetDatabase(mongoSettings.Value.DatabaseName);
            _collection = mongoDatabase.GetCollection<User>(mongoSettings.Value.CollectionName);

        }

        public async Task<List<User>> GetAsync() =>
            await _collection.Find(_ => true).ToListAsync();

        public async Task<User?> GetAsync(string id) =>
            await _collection.Find(x => x.Id == id).FirstOrDefaultAsync();

        public async Task CreateAsync(User newUser) =>
            await _collection.InsertOneAsync(newUser);

        public async Task UpdateAsync(string id, User updatedUser) =>
            await _collection.ReplaceOneAsync(x => x.Id == id, updatedUser);

        public async Task RemoveAsync(string id) =>
            await _collection.DeleteOneAsync(x => x.Id == id);
    }
}
