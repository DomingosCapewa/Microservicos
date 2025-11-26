

using MongoDB.Bson;
using MongoDB.Driver;
using MS.Domain.Entities;
using MS.Repository.Interfaces;

namespace MS.Service
{
    public class MyService
    {
        private readonly IMongoCollection<MyEntity> _collection;
        private readonly IMyEntityRepository _repository;
    
        public MyService(IMongoDbConnection mongoDbConnection, IMyEntityRepository repository)
        {
            _collection = mongoDbConnection.GetDatabase().GetCollection<MyEntity>("MyEntities");
            
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));

            if(_collection == null)
            {
                throw new Exception("Could not get the collection 'MyEntities' from MongoDB.");
            }
        }

        public async Task<List<MyEntity>> GetAllEntitiesAsync()
        {
            try
            {
               return await _collection.Find(entity => true).ToListAsync();   
            }
            catch (Exception ex)
            {
                throw new Exception("Error retrieving entities from MongoDB.", ex);
        }
    }
        public async Task<MyEntity> GetEntityByIdAsync(string id)
        {
            if(ObjectId.TryParse(id, out ObjectId objectId))
            {
              return await _collection.Find(entity => entity._id == objectId).FirstOrDefaultAsync();   
            }
          else
            {
                throw new ArgumentException("Invalid ID fromat. ID must be a valid ObjectId", nameof(id));
            }

        }

        public async Task CreateEntityAsync(MyEntity entity) => await _repository.CreateAsync(entity);
        }
}
