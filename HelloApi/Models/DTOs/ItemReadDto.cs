namespace HelloApi.Models.DTOs;

public class ItemReadDto
{
    public int Id       { get; set; }
    public string Name  { get; set; } = "";
    public decimal Price{ get; set; }
    public DateTime CreatedAt { get; set; }
}