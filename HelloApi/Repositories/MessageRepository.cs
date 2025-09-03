using HelloApi.Data;
using HelloApi.Models;
using Microsoft.EntityFrameworkCore;

namespace HelloApi.Repositories;

public class MessageRepository : IMessageRepository
{
    private readonly AppDbContext _db;
    public MessageRepository(AppDbContext db) => _db = db;

    public async Task<IEnumerable<Message>> GetAllAsync() =>
        await _db.Messages.AsNoTracking().OrderByDescending(m => m.Id).ToListAsync();

    public async Task<Message?> GetByIdAsync(int id) =>
        await _db.Messages.FindAsync(id);

    public async Task<Message> AddAsync(Message message)
    {
        _db.Messages.Add(message);
        await _db.SaveChangesAsync();
        return message;
    }

    public async Task<bool> UpdateAsync(Message message)
    {
        _db.Messages.Update(message);
        return await _db.SaveChangesAsync() > 0;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var entity = await _db.Messages.FindAsync(id);
        if (entity is null) return false;
        _db.Messages.Remove(entity);
        return await _db.SaveChangesAsync() > 0;
    }
}
