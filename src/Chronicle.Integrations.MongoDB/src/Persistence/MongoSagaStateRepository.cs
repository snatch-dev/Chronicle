using System;
using System.Threading.Tasks;
using MongoDB.Driver;

namespace Chronicle.Integrations.MongoDB.Persistence
{
    internal sealed class MongoSagaStateRepository : ISagaStateRepository
    {
        private const string CollectionName = "SagaData";
        private readonly IMongoCollection<MongoSagaState> _collection;

        public MongoSagaStateRepository(IMongoDatabase database)
            => _collection = database.GetCollection<MongoSagaState>(CollectionName);

        public async Task<ISagaState> ReadAsync(SagaId id, Type type)
             => await _collection
                     .Find(sld => sld.MongoId == id.Id && sld.SagaType == type.FullName)
                     .FirstOrDefaultAsync();

        public async Task WriteAsync(ISagaState sagaState)
        {
            await _collection.DeleteOneAsync(sld => sld.MongoId == sagaState.Id.Id && sld.SagaType == sagaState.Type.FullName);
            await _collection.InsertOneAsync(new MongoSagaState
            {
                MongoId = sagaState.Id.Id,
                SagaType = sagaState.Type.FullName,
                State = sagaState.State,
                Data = sagaState.Data
            });
        }
    }
}
