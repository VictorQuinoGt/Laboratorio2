namespace HelloApi.Models.DTOs;

public class OrderCreateDto
{
    public int PersonId { get; set; }
    public List<OrderLineDto> Lines { get; set; } = new();
}