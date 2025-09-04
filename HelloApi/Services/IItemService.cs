using HelloApi.Models.DTOs;

namespace HelloApi.Services;

public interface IItemService
{
    Task<List<ItemReadDto>> GetAllAsync();
    Task<ItemReadDto?> GetByIdAsync(int id);
    Task<ItemReadDto> CreateAsync(ItemCreateDto dto);
    Task<ItemReadDto?> UpdateAsync(int id, ItemUpdateDto dto);
    Task<bool> DeleteAsync(int id);
}
