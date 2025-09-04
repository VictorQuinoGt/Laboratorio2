using HelloApi.Models;
using Microsoft.EntityFrameworkCore;
using HelloApi.Data; // cambia a HelloApi.Data si corresponde

namespace HelloApi.Repositories;

public class ItemRepository(AppDbContext db) : IItemRepository
{
    private readonly AppDbContext _db = db;

    public async Task<List<Item>> GetAllAsync() =>
        await _db.Items.OrderBy(i => i.Name).ToListAsync();

    public Task<Item?> GetByIdAsync(int id) =>
        _db.Items.FirstOrDefaultAsync(i => i.Id == id);

    public Task<bool> ExistsByNameAsync(string name) =>
        _db.Items.AnyAsync(i => i.Name == name);

    public async Task<Item> AddAsync(Item entity)
    {
        entity.CreatedAt = DateTime.UtcNow;
        _db.Items.Add(entity);
        await _db.SaveChangesAsync();
        return entity;
    }

    public async Task<Item?> UpdateAsync(Item entity)
    {
        var current = await _db.Items.FindAsync(entity.Id);
        if (current is null) return null;

        current.Name      = entity.Name;
        current.Price     = entity.Price;
        current.UpdatedAt = DateTime.UtcNow;

        await _db.SaveChangesAsync();
        return current;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var current = await _db.Items.FindAsync(id);
        if (current is null) return false;

        _db.Items.Remove(current);
        try
        {
            await _db.SaveChangesAsync();
            return true;
        }
        catch (DbUpdateException)
        {
            // Por FK RESTRICT si tiene OrderDetails
            throw new InvalidOperationException("No se puede eliminar el item porque tiene órdenes asociadas.");
        }
    }
}
