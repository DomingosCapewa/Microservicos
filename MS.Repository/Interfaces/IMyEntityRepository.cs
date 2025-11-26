

using MS.Domain.Entities;

namespace MS.Repository.Interfaces
{
    public interface IMyEntityRepository
    {
        Task<List<MyEntity>> GetAllAsync();
        Task<MyEntity> GetByIdAsync(string id);
        Task CreateAsync(MyEntity entity);
    }
}