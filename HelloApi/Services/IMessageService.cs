using HelloApi.Models.DTOs;

namespace HelloApi.Services;

public interface IMessageService
{
    Task<IEnumerable<MessageReadDto>> GetAllAsync();
    Task<MessageReadDto?> GetByIdAsync(int id);
    Task<MessageReadDto> CreateAsync(MessageCreateDto dto);
    Task<bool> UpdateAsync(int id, MessageUpdateDto dto);
    Task<bool> DeleteAsync(int id);
}
