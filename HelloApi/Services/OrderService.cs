using HelloApi.Models;
using HelloApi.Models.DTOs;
using Microsoft.EntityFrameworkCore;
using HelloApi.Data; 

namespace HelloApi.Services;

public class OrderService(AppDbContext db) : IOrderService
{
    private readonly AppDbContext _db = db;

    public async Task<OrderReadDto?> GetByIdAsync(int id)
    {
        var o = await _db.Orders
            .Include(x => x.Person)
            .Include(x => x.OrderDetails).ThenInclude(od => od.Item)
            .FirstOrDefaultAsync(x => x.Id == id);

        return o is null ? null : MapToReadDto(o);
    }

    public async Task<List<OrderReadDto>> GetAllAsync(int page = 1, int pageSize = 20)
    {
        if (page < 1) page = 1;
        if (pageSize < 1) pageSize = 20;

        var orders = await _db.Orders
            .Include(x => x.Person)
            .Include(x => x.OrderDetails).ThenInclude(od => od.Item)
            .OrderByDescending(x => x.Id)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        return orders.Select(MapToReadDto).ToList();
    }

    public async Task<OrderReadDto> CreateAsync(OrderCreateDto dto)
    {
        // Validar person
        var person = await _db.Persons.FindAsync(dto.PersonId)
                     ?? throw new KeyNotFoundException("Person no existe");

        var order = new Order
        {
            
            PersonId = person.Id,
            Number = await NextOrderNumberAsync(),
            CreatedAt = DateTime.UtcNow,
            OrderDetails = new List<OrderDetail>()
        };

        if (dto.Lines is null || dto.Lines.Count == 0)
            throw new ArgumentException("La orden debe tener al menos una línea.");

        foreach (var l in dto.Lines)
        {
            var item = await _db.Items.FindAsync(l.ItemId)
                       ?? throw new KeyNotFoundException($"Item {l.ItemId} no existe");

            var price = l.Price ?? item.Price;
            if (l.Quantity <= 0) throw new ArgumentException("Quantity debe ser > 0");

            order.OrderDetails.Add(new OrderDetail
            {
                OrderId = order.Id,
                ItemId = item.Id,
                Quantity = l.Quantity,
                Price = price,
                Total = price * l.Quantity,
                CreatedAt = DateTime.UtcNow
            });
        }

        _db.Orders.Add(order);
        await _db.SaveChangesAsync();

        // Recargar con includes para map completo
        var created = await _db.Orders
            .Include(x => x.Person)
            .Include(x => x.OrderDetails).ThenInclude(od => od.Item)
            .FirstAsync(x => x.Id == order.Id);

        return MapToReadDto(created);
    }

    private async Task<int> NextOrderNumberAsync()
    {
        var last = await _db.Orders.MaxAsync(o => (int?)o.Number);
        return (last ?? 0) + 1;
    }

    private static OrderReadDto MapToReadDto(Order o)
    {
        var lines = o.OrderDetails.Select(d => new OrderReadLineDto
        {
            ItemId = d.ItemId,
            ItemName = d.Item?.Name ?? "",
            Quantity = d.Quantity,
            Price = d.Price,
            Total = d.Total
        }).ToList();

        return new OrderReadDto
        {
            Id = o.Id,
            Number = o.Number,
            PersonId = o.PersonId,
            PersonName = o.Person != null ? $"{o.Person.FirstName} {o.Person.LastName}" : "",
            CreatedAt = o.CreatedAt,
            Lines = lines,
            GrandTotal = lines.Sum(x => x.Total)
        };
    }
}
