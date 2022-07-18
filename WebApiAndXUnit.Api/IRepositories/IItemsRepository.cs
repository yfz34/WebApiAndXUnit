using WebApiAndXUnit.Api.Entities;

namespace WebApiAndXUnit.Api.IRepositories
{
    public interface IItemsRepository
    {
        Task<ICollection<Item>> GetAllAsync(bool asNoTracking = false);
        Task<Item?> GetByIdAsync(int id, bool asNoTracking = false);
        Task CreateAsync(Item newItem);
        Task UpdateAsync(Item updateItem);
        Task RemoveAsync(int id);
    }
}