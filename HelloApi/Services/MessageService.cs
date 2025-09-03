using HelloApi.Models;
using HelloApi.Models.DTOs;
using HelloApi.Repositories;

namespace HelloApi.Services;

public class MessageService : IMessageService
{
    private readonly IMessageRepository _repo;
    public MessageService(IMessageRepository repo) => _repo = repo;

    public async Task<IEnumerable<MessageReadDto>> GetAllAsync() =>
        (await _repo.GetAllAsync()).Select(MapToRead);

    public async Task<MessageReadDto?> GetByIdAsync(int id)
    {
        var entity = await _repo.GetByIdAsync(id);
        return entity is null ? null : MapToRead(entity);
    }

    public async Task<MessageReadDto> CreateAsync(MessageCreateDto dto)
    {
        var entity = new Message
        {
            MessageText = dto.Message,
            CreatedAt = DateTime.UtcNow
        };
        var saved = await _repo.AddAsync(entity);
        return MapToRead(saved);
    }

    public async Task<bool> UpdateAsync(int id, MessageUpdateDto dto)
    {
        var entity = await _repo.GetByIdAsync(id);
        if (entity is null) return false;
        entity.MessageText = dto.Message;
        entity.UpdatedAt = DateTime.UtcNow;
        return await _repo.UpdateAsync(entity);
    }

    public Task<bool> DeleteAsync(int id) => _repo.DeleteAsync(id);

    private static MessageReadDto MapToRead(Message m) => new()
    {
        Id = m.Id,
        Message = m.MessageText,
        CreatedAt = m.CreatedAt,
        UpdatedAt = m.UpdatedAt
    };
}
