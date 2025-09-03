using HelloApi.Models;

namespace HelloApi.Repositories;

public interface IMessageRepository
{
    Task<IEnumerable<Message>> GetAllAsync();
    Task<Message?> GetByIdAsync(int id);
    Task<Message> AddAsync(Message message);
    Task<bool> UpdateAsync(Message message);
    Task<bool> DeleteAsync(int id);
}
