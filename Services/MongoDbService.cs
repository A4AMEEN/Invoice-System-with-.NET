using MongoDB.Driver;
using myWebApi.Models;
using System;

namespace myWebApi.Services
{
    public class MongoDbService
    {
        private readonly IMongoCollection<InvoiceDto> _invoicesCollection;

        public MongoDbService(IConfiguration configuration)
        {
            var mongoClient = new MongoClient(
                configuration.GetValue<string>("MongoDB:ConnectionString"));

            var mongoDatabase = mongoClient.GetDatabase(
                configuration.GetValue<string>("MongoDB:DatabaseName"));

            _invoicesCollection = mongoDatabase.GetCollection<InvoiceDto>(
                configuration.GetValue<string>("MongoDB:InvoicesCollectionName"));
        }

        public async Task<List<InvoiceDto>> GetAsync() =>
            await _invoicesCollection.Find(_ => true).ToListAsync();

        public async Task<InvoiceDto?> GetAsync(string id) =>
            await _invoicesCollection.Find(x => x.Id == id).FirstOrDefaultAsync();

        public async Task CreateAsync(InvoiceDto newInvoice)
        {
            // Generate an ID if none is provided
            if (string.IsNullOrEmpty(newInvoice.Id))
            {
                newInvoice.Id = Guid.NewGuid().ToString();
            }
            
            // Set invoice date if not already set
            if (newInvoice.InvoiceDate == DateTime.MinValue)
            {
                newInvoice.InvoiceDate = DateTime.UtcNow;
            }
            
            await _invoicesCollection.InsertOneAsync(newInvoice);
        }

        public async Task UpdateAsync(string id, InvoiceDto updatedInvoice) =>
            await _invoicesCollection.ReplaceOneAsync(x => x.Id == id, updatedInvoice);

        public async Task RemoveAsync(string id) =>
            await _invoicesCollection.DeleteOneAsync(x => x.Id == id);
    }
}