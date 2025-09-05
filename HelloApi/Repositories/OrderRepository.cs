using HelloApi.Models;
using Microsoft.EntityFrameworkCore;
using  HelloApi.Data; // cambia a HelloApi.Data si corresponde

namespace HelloApi.Repositories;

public class OrderRepository(AppDbContext db) : IOrderRepository
{
    private readonly AppDbContext _db = db;

    public Task<Order?> GetByIdAsync(int id, bool includeDetails = false)
    {
        IQueryable<Order> q = _db.Orders;
        if (includeDetails)
            q = q.Include(o => o.Person)
                 .Include(o => o.OrderDetails)
                    .ThenInclude(d => d.Item);
        return q.FirstOrDefaultAsync(o => o.Id == id);
    }

    public async Task<List<Order>> GetAllAsync(int page = 1, int pageSize = 20, bool includeDetails = false)
    {
        if (page < 1) page = 1;
        if (pageSize < 1) pageSize = 20;

        IQueryable<Order> q = _db.Orders.OrderByDescending(o => o.Id);
        if (includeDetails)
            q = q.Include(o => o.Person)
                 .Include(o => o.OrderDetails)
                    .ThenInclude(d => d.Item);

        return await q.Skip((page - 1) * pageSize)
                      .Take(pageSize)
                      .ToListAsync();
    }

    public async Task<int> GetNextOrderNumberAsync()
    {
        var last = await _db.Orders.MaxAsync(o => (int?)o.Number);
        return (last ?? 0) + 1;
    }

    public async Task<Order> AddAsync(Order order)
    {
        order.Number   = order.Number == 0 ? await GetNextOrderNumberAsync() : order.Number;
        order.CreatedAt = DateTime.UtcNow;

        // Set defaults en líneas
        if (order.OrderDetails is not null)
        {
            foreach (var d in order.OrderDetails)
            {
                d.OrderId   = order.Id; // aunque no se use, por claridad
                d.Order     = order;
                d.Total     = d.Price * d.Quantity;
                d.CreatedAt = d.CreatedAt == default ? DateTime.UtcNow : d.CreatedAt;
            }
        }

        _db.Orders.Add(order);
        await _db.SaveChangesAsync();
        return order;
    }

    public async Task<Order?> UpdateAsync(Order order)
    {
        // Se espera que 'order' esté trackeado; si no, lo adjuntamos
        var current = await _db.Orders
            .Include(o => o.OrderDetails)
            .FirstOrDefaultAsync(o => o.Id == order.Id);

        if (current is null) return null;

        // Header
        current.PersonId  = order.PersonId;
        current.Number    = order.Number == 0 ? current.Number : order.Number;
        current.UpdatedAt = DateTime.UtcNow;

        // Si el caller quiere reemplazar las líneas:
        if (order.OrderDetails is not null)
        {
            current.OrderDetails.Clear(); // por Cascade borra las anteriores

            foreach (var d in order.OrderDetails)
            {
                current.OrderDetails.Add(new OrderDetail
                {
                    OrderId   = current.Id,
                    ItemId    = d.ItemId,
                    Quantity  = d.Quantity,
                    Price     = d.Price,
                    Total     = d.Price * d.Quantity,
                    CreatedAt = DateTime.UtcNow
                });
            }
        }

        await _db.SaveChangesAsync();
        return current;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var current = await _db.Orders.FindAsync(id);
        if (current is null) return false;

        _db.Orders.Remove(current); // por Cascade se borran OrderDetails
        await _db.SaveChangesAsync();
        return true;
    }
}
