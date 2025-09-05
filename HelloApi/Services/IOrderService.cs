using HelloApi.Models.DTOs;

namespace HelloApi.Services;

public interface IOrderService
{
    Task<OrderReadDto?> GetByIdAsync(int id);
    Task<List<OrderReadDto>> GetAllAsync(int page = 1, int pageSize = 20);
    Task<OrderReadDto> CreateAsync(OrderCreateDto dto);
}
