using HelloApi.Models;
using Microsoft.EntityFrameworkCore;
using HelloApi.Data;

namespace HelloApi.Repositories;

public class OrderRepository(AppDbContext db) : IOrderRepository
{
    private readonly AppDbContext _db = db;

    public async Task<Order?> GetByIdAsync(int id, bool includeDetails = false)
    {
        IQueryable<Order> q = _db.Orders;
        if (includeDetails)
            q = q.Include(o => o.Person)
                 .Include(o => o.OrderDetails).ThenInclude(od => od.Item);
        return await q.FirstOrDefaultAsync(o => o.Id == id);
    }

    public async Task<List<Order>> GetAllAsync(int page = 1, int pageSize = 20, bool includeDetails = false)
    {
        IQueryable<Order> q = _db.Orders.OrderByDescending(o => o.CreatedAt);
        if (includeDetails)
            q = q.Include(o => o.Person)
                 .Include(o => o.OrderDetails).ThenInclude(od => od.Item);
        return await q.Skip((page - 1) * pageSize).Take(pageSize).ToListAsync();
    }

    public async Task<int> GetNextOrderNumberAsync()
    {
        var last = await _db.Orders.OrderByDescending(o => o.Number).FirstOrDefaultAsync();
        return (last?.Number ?? 0) + 1;
    }

    public async Task<Order> AddAsync(Order order)
    {
        _db.Orders.Add(order);
        await _db.SaveChangesAsync();
        return order;
    }

    public async Task<Order?> UpdateAsync(Order order)
    {
        var current = await _db.Orders.Include(o => o.OrderDetails).FirstOrDefaultAsync(o => o.Id == order.Id);
        if (current is null) return null;

        current.PersonId = order.PersonId;
        current.Number   = order.Number;

        // Actualización básica de líneas: borra y re-inserta (simple)
        _db.OrderDetails.RemoveRange(current.OrderDetails);
        if (order.OrderDetails is not null)
        {
            foreach (var d in order.OrderDetails)
            {
                d.Order = current;
                d.Total = d.Price * d.Quantity;
                _db.OrderDetails.Add(d);
            }
        }
        await _db.SaveChangesAsync();
        return current;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var current = await _db.Orders.FindAsync(id);
        if (current is null) return false;
        _db.Orders.Remove(current);
        await _db.SaveChangesAsync();
        return true;
    }
}

