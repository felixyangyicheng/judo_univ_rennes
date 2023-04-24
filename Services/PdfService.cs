using Microsoft.Extensions.Options;
using judo_univ_rennes.Contracts;
using judo_univ_rennes.Data;
using MongoDB.Driver;
using judo_univ_rennes.Configurations;

namespace judo_univ_rennes.Services
{
    public class PdfService : IPdfRepo
    {
        private readonly IMongoCollection<PdfModel> _collection;

        public PdfService(
            IOptions<ConnectionStringModel> dbSettings)
        {
            var mongoClient = new MongoClient(
                dbSettings.Value.ConnectionString);

            var mongoDatabase = mongoClient.GetDatabase(
                dbSettings.Value.DatabaseName);

            _collection = mongoDatabase.GetCollection<PdfModel>(
                dbSettings.Value.ThreedCollectionName);
        }

        public async Task<bool> CreateAsync(PdfModel obj)
        {
            return _collection.InsertOneAsync(obj).IsCompletedSuccessfully;
        }

        public async Task<List<PdfModel>> GetAsync() =>
            await _collection.Find(_ => true).ToListAsync();

        public async Task<PdfModel?> GetAsync(string id) =>
            await _collection.Find(x => x.Id == id).FirstOrDefaultAsync();

        public async Task<PdfModel?> GetByNameAsync(string name) =>
            await _collection.Find(x => x.FileName == name).FirstOrDefaultAsync();

        public PdfModel GetChanges()
        {
            throw new NotImplementedException();
        }

        public async Task RemoveAsync(string id) =>
            await _collection.DeleteOneAsync(x => x.Id == id);

        public async Task<List<PdfModel>> SearchByNameAsync(string name) =>
            await _collection.Find(x => x.FileName == name).ToListAsync();

        public async Task<bool> UpdateAsync(string id, PdfModel obj)
        {
            return _collection.ReplaceOneAsync(x => x.Id == id, obj).IsCompletedSuccessfully;

        }
    }
}
