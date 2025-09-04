using HelloApi.Models;

namespace HelloApi.Repositories;

public interface IItemRepository
{
    Task<List<Item>> GetAllAsync();
    Task<Item?> GetByIdAsync(int id);
    Task<bool> ExistsByNameAsync(string name);
    Task<Item> AddAsync(Item entity);
    Task<Item?> UpdateAsync(Item entity);
    Task<bool> DeleteAsync(int id);
}
