
using System.Diagnostics.Contracts;

using MongoDB.Bson;
using MongoDB.Driver;
using MS.Domain.Entities;
using MS.Repository.Interfaces;

namespace MS.Repository
{
    public class MyEntityRepository: IMyEntityRepository
    {
        private readonly IMongoCollection<MyEntity> _collection;

        public MyEntityRepository(IMongoDbConnection database)
        {
            Contract.Requires(database != null);

            _collection = database.GetDatabase().GetCollection<MyEntity>("MyEntities");
        }

        public async Task<List<MyEntity>> GetAllAsync()
        {
            try
            {
               return await _collection.Find(_ => true).ToListAsync();  
            } catch (Exception ex)
            {
                throw new Exception("An error occurred while retrieving entities.", ex);
            }
        }

        public async Task<MyEntity> GetByIdAsync(string id)
        {
            try
            {
                if(ObjectId.TryParse(id, out ObjectId objectId))
                {
                  return await _collection.Find(entity => entity._id  == objectId).FirstOrDefaultAsync();
                } 
                else
                {
                    throw new ArgumentException("Invalid ID format.", nameof(id));
                }
            }
            catch (Exception ex)
            {

                throw new Exception("An error occurred while retrieving the entity by ID.", ex);
            }
        }

        public async Task CreateAsync(MyEntity entity)
        {
            try
            {
                await _collection.InsertOneAsync(entity);
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while creating the entity.", ex);
            }
        }
    }
}