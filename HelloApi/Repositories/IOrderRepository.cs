using HelloApi.Models;

namespace HelloApi.Repositories;

public interface IOrderRepository
{
    Task<Order?> GetByIdAsync(int id, bool includeDetails = false);
    Task<List<Order>> GetAllAsync(int page = 1, int pageSize = 20, bool includeDetails = false);
    Task<int> GetNextOrderNumberAsync();
    Task<Order> AddAsync(Order order);
    Task<Order?> UpdateAsync(Order order);
    Task<bool> DeleteAsync(int id);
}
