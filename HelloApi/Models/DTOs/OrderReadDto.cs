namespace HelloApi.Models.DTOs;

public class OrderReadDto
{
    public int Id            { get; set; }
    public int Number        { get; set; }
    public int PersonId      { get; set; }
    public string PersonName { get; set; } = "";
    public DateTime CreatedAt{ get; set; }
    public List<OrderReadLineDto> Lines { get; set; } = new();
    public decimal GrandTotal { get; set; }
}
