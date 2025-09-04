using HelloApi.Models;
using HelloApi.Models.DTOs;
using Microsoft.EntityFrameworkCore;
using HelloApi.Data; // Ajusta si tu DbContext está en HelloApi.Data

namespace HelloApi.Services;

public class ItemService(AppDbContext db) : IItemService
{
    private readonly AppDbContext _db = db;

    public async Task<List<ItemReadDto>> GetAllAsync()
    {
        return await _db.Items
            .OrderBy(i => i.Name)
            .Select(i => new ItemReadDto {
                Id = i.Id, Name = i.Name, Price = i.Price, CreatedAt = i.CreatedAt
            })
            .ToListAsync();
    }

    public async Task<ItemReadDto?> GetByIdAsync(int id)
    {
        return await _db.Items
            .Where(i => i.Id == id)
            .Select(i => new ItemReadDto {
                Id = i.Id, Name = i.Name, Price = i.Price, CreatedAt = i.CreatedAt
            })
            .FirstOrDefaultAsync();
    }

    public async Task<ItemReadDto> CreateAsync(ItemCreateDto dto)
    {
        var entity = new Item {
            Name = dto.Name,
            Price = dto.Price,
            CreatedAt = DateTime.UtcNow
        };
        _db.Items.Add(entity);
        await _db.SaveChangesAsync();

        return new ItemReadDto {
            Id = entity.Id, Name = entity.Name, Price = entity.Price, CreatedAt = entity.CreatedAt
        };
    }

    public async Task<ItemReadDto?> UpdateAsync(int id, ItemUpdateDto dto)
    {
        var entity = await _db.Items.FindAsync(id);
        if (entity is null) return null;

        entity.Name = dto.Name;
        entity.Price = dto.Price;
        entity.UpdatedAt = DateTime.UtcNow;

        await _db.SaveChangesAsync();

        return new ItemReadDto {
            Id = entity.Id, Name = entity.Name, Price = entity.Price, CreatedAt = entity.CreatedAt
        };
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var entity = await _db.Items.FindAsync(id);
        if (entity is null) return false;

        _db.Items.Remove(entity);
        try
        {
            await _db.SaveChangesAsync();
            return true;
        }
        catch (DbUpdateException)
        {
            // RESTRICT en OrderDetails->Item impide borrar con ventas históricas
            throw new InvalidOperationException("No se puede eliminar el item porque tiene órdenes asociadas.");
        }
    }
}
